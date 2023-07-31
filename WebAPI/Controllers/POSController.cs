using Application.Interfaces;
using Domain.Entities;
using ImageMagick;
using Microsoft.AspNetCore.Mvc;
using QRCoder;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using WebAPI.Common;
using WebAPI.ViewModels.Point;
using ZXing;
using ZXing.Magick;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class POSController : ControllerBase
    {
        private const string POINT_API_URL = "https://localhost:7017/api/Point";

        private readonly IPOSService _pOSService;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<POSController> _logger;

        public POSController(IPOSService pOSService, IHttpClientFactory httpClientFactory, ILogger<POSController> logger)
        {
            _pOSService = pOSService;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        [HttpPost("ScanMemberQRCode")]
        public async Task<ActionResult<ApiResponse<Purchase>>> ScanMemberQRCode(IFormFile imageFile, string purchaseId)
        {
            var response = new ApiResponse<Purchase>();

            if (imageFile.Length > 0 && !string.IsNullOrEmpty(purchaseId))
            {
                try
                {
                    string memberQRCode = GetQRCode(imageFile);

                    //updated to purchase
                    var result = await _pOSService.ScanMemberQRCode(memberQRCode, purchaseId);

                    if (result == null)
                    {
                        response.Code = StatusCodes.Status204NoContent;
                        response.Status = ApiResponseStatus.Success.ToString();
                        response.Message = ApiResponseMessage.RecordNotFound.ToString();

                        return response;
                    }

                    response.Code = StatusCodes.Status200OK;
                    response.Data = result;

                    return response;
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }

            response.Code = StatusCodes.Status400BadRequest;
            response.Status = ApiResponseStatus.Error.ToString();
            response.Message = ApiResponseMessage.InvalidRequestFormat.ToString();

            return BadRequest();
        }

        [HttpPost("ScanCuponQRCode")]
        public async Task<ActionResult<ApiResponse<Purchase>>> ScanCuponQRCode(IFormFile imageFile, string purchaseId)
        {
            var response = new ApiResponse<Purchase>();

            if (imageFile.Length > 0 && !string.IsNullOrEmpty(purchaseId))
            {
                try
                {
                    string cuponQRCode = GetQRCode(imageFile);

                    var result = await _pOSService.ScanCuponQRCode(cuponQRCode, purchaseId);

                    if (result == null)
                    {
                        response.Code = StatusCodes.Status204NoContent;
                        response.Status = ApiResponseStatus.Success.ToString();
                        response.Message = ApiResponseMessage.RecordNotFound.ToString();

                        return response;
                    }

                    response.Code = StatusCodes.Status200OK;
                    response.Data = result;

                    return response;

                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }

            response.Code = StatusCodes.Status400BadRequest;
            response.Status = ApiResponseStatus.Error.ToString();
            response.Message = ApiResponseMessage.InvalidRequestFormat.ToString();


            return BadRequest();
        }

        /// <summary>
        /// using ZXing,ZXing.Magick, ImageMagick
        /// </summary>
        /// <param name="imageFile"></param>
        /// <returns></returns>
        private string GetQRCode(IFormFile imageFile)
        {
            using (var stream = imageFile.OpenReadStream())
            {
                using (var image = new MagickImage(stream))
                {
                    var reader = new BarcodeReader();
                    reader.Options.PossibleFormats = new List<BarcodeFormat> { BarcodeFormat.QR_CODE };
                    var result = reader.Decode(image);

                    if (result != null)
                    {
                        return result.Text;
                    }
                }
            }

            return string.Empty;
        }

        [HttpPost("AddPurcahse")]
        public async Task<ActionResult<ApiResponse<Purchase>>> AddPurcahse(Purchase purchase)
        {
            var response = new ApiResponse<Purchase>();

            if (ModelState.IsValid)
            {
                var result = await _pOSService.Purchase(purchase);

                if (result == null)
                {
                    response.Code = StatusCodes.Status204NoContent;
                    response.Status = ApiResponseStatus.Success.ToString();
                    response.Message = ApiResponseMessage.RecordNotFound.ToString();

                    return response;
                }


                //Sale Order Process - not implement yet


                //POST to Point API - should be used message-broker like RabbitMQ                
                try
                {
                    if (!string.IsNullOrEmpty(result.MemberCode))
                    {
                        CreatePointViewModel pointViewModel = CreatePointViewModelBind(result);

                        var jsonContent = JsonSerializer.Serialize(pointViewModel);
                        var httpClient = _httpClientFactory.CreateClient();
                        httpClient.BaseAddress = new Uri(POINT_API_URL);
                        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                        var responseMessage = await httpClient.PostAsync(POINT_API_URL + "/AddPoint", content);

                        if (responseMessage.IsSuccessStatusCode)
                        {
                            var responseContent = await responseMessage.Content.ReadAsStringAsync();
                            _logger.LogInformation("Success to post point. Message : {}", responseContent);
                        }
                        else
                        {
                            _logger.LogInformation("Failed to post point. Status code: " + responseMessage.StatusCode);
                        }
                    }


                }
                catch (HttpRequestException ex)
                {
                    response.Code = StatusCodes.Status400BadRequest;
                    response.Status = ex.Message;
                    response.Message = ApiResponseMessage.UnexpectedErrorOccurred.ToString();
                    return response;
                }

                response.Code = StatusCodes.Status200OK;
                response.Data = result;

            }

            response.Code = StatusCodes.Status400BadRequest;
            response.Status = ApiResponseStatus.Error.ToString();
            response.Message = ApiResponseMessage.InvalidRequestFormat.ToString();


            return response;
        }

        private CreatePointViewModel CreatePointViewModelBind(Purchase result)
        {
            var pointViewModel = new CreatePointViewModel()
            {
                MemberCode = result.MemberCode,
                CouponCode = result.CouponCode,
                ReceiptNumber = result.PurchaseId, // should be auto generated voucher no
            };

            foreach (var item in result.Items)
            {
                pointViewModel.Items.Add(new CreatePointItemViewModel { Price = item.Price, ProductCode = item.ProductName, ProductType = item.ProductType, Quantity = item.Quantity, TotalPrice = item.TotalAmount });
            }

            return pointViewModel;
        }

        /// <summary>
        /// Using QRCoder Package 
        /// Only For Testing
        /// </summary>
        /// <returns></returns>
        [HttpGet("GenerateMemberQRCode")]
        public IActionResult GenerateMemberQRCode()
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode("member_001", QRCodeGenerator.ECCLevel.Q);
            PngByteQRCode qrCode = new PngByteQRCode(qrCodeData);
            byte[] qrCodeAsPngByteArr = qrCode.GetGraphic(20);

            return File(qrCodeAsPngByteArr, "image/png", "member_001.png");

        }
    }
}
