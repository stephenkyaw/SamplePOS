using Application.DTOs.Point;
using Application.Interfaces;
using Domain.Interfaces;

namespace Application.Services
{
    public class PointService : IPointService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PointService(IProductRepository productRepository, ICustomerRepository customerRepository, IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _customerRepository = customerRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> AddPoint(PointDto pointDto)
        {
            var customer = await _customerRepository.GetAsync(x => x.MemberCode.ToLower() == pointDto.MemberCode.ToLower());

            if(customer != null)
            {
                var total_Non_Alcohol_Items_Price = pointDto.Items.Where(x => x.ProductType == "Non-Alcohol").Sum(x => x.Quantity * x.Price);

                if (total_Non_Alcohol_Items_Price > 100)
                {
                    customer.TotalPoints = customer.TotalPoints + (total_Non_Alcohol_Items_Price / 10);
                    customer.TotalPurchasedAmount = customer.TotalPurchasedAmount + total_Non_Alcohol_Items_Price; //only for get point amount\

                    //update to database
                     _customerRepository.Update(customer);

                    await _unitOfWork.SaveChangesAsync();

                    return true;
                }
            }

            return false;
            
        }

       
    }
}
