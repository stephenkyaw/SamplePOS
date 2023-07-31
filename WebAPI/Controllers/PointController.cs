using Application.DTOs.Point;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WebAPI.ViewModels.Point;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PointController : ControllerBase
    {
        private readonly IPointService _pointService;

        public PointController(IPointService pointService)
        {
            _pointService = pointService;
        }

        [HttpPost("AddPoint")]
        public async Task<IActionResult> AddPoint(CreatePointViewModel model)
        {
            if (ModelState.IsValid)
            {
                var pointDto = new PointDto() { MemberCode = model.MemberCode, CouponCode = model.CouponCode, ReceiptNumber = model.ReceiptNumber };

                foreach (var item in model.Items)
                {
                    pointDto.Items.Add(new PointItemDto(item.ProductCode, item.ProductType, item.Price, item.Quantity, item.TotalPrice));
                }

                bool result = await _pointService.AddPoint(pointDto);

                if (result)
                {
                    return Ok(result);
                }

                return Ok(false);
            }
            return BadRequest();
        }
    }
}
