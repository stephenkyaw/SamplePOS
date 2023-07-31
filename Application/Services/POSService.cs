using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class POSService : IPOSService
    {
        private readonly IPurchaseRepository _purchaseRepository;
        private readonly ICustomerRepository _customerRepository;

        public POSService(IPurchaseRepository purchaseRepository, ICustomerRepository customerRepository)
        {
            _purchaseRepository = purchaseRepository;
            _customerRepository = customerRepository;
        }



        public async Task<Purchase> Purchase(Purchase purchase)
        {
            purchase.TotalItemQuantity = purchase.Items.Sum(x => x.Quantity);
            purchase.TotalAmount = purchase.Items.Sum(x => x.TotalAmount);

            //check new purchase or old purchase
            if (string.IsNullOrEmpty(purchase.PurchaseId))
            {
                //new 
                return await _purchaseRepository.AddPurchaseAsync(purchase);
            }
            else
            {
                if(!string.IsNullOrEmpty(purchase.MemberCode))
                {
                    var existingPurchase = await _purchaseRepository.GetPurchaseByIdAsync(purchase.PurchaseId);

                    purchase.CustoemrId = existingPurchase.CustoemrId;
                    purchase.MemberCode = existingPurchase.MemberCode;
                    purchase.CustomerName = existingPurchase.CustomerName;
                    purchase.CouponCode = existingPurchase.CouponCode;

                    //update
                    return await _purchaseRepository.UpdatePurchaseAsync(purchase);

                }

                //update
                return await _purchaseRepository.UpdatePurchaseAsync(purchase);

            }
        }

        public async Task<Purchase> ScanCuponQRCode(string cuponQRCode, string purchaseId)
        {
            if (!string.IsNullOrEmpty(cuponQRCode) && !string.IsNullOrEmpty(purchaseId))
            {

                var purchase = await _purchaseRepository.GetPurchaseByIdAsync(purchaseId);


                //Cupon Item Quantity Limit is 30
                if (purchase.Items.Count <= 30)
                {
                    purchase.CouponCode = cuponQRCode;
                }

                //update to redis cache
                return await _purchaseRepository.UpdatePurchaseAsync(purchase);
            }
            return null;
        }
        
        public async Task<Purchase> ScanMemberQRCode(string memberQRCodeCode, string purchaseId)
        {
            if (!string.IsNullOrEmpty(memberQRCodeCode) && !string.IsNullOrEmpty(purchaseId))
            {
                var purchase = await _purchaseRepository.GetPurchaseByIdAsync(purchaseId);

                var customer = await _customerRepository.GetAsync(x => x.MemberCode == memberQRCodeCode);

                // check customer's membercode
                if(customer != null )
                {
                    purchase.MemberCode = memberQRCodeCode;
                    purchase.CustoemrId = customer.CustomerId;
                    purchase.CustomerName = customer.Name;
                }

                //update to redis cache
                return await _purchaseRepository.UpdatePurchaseAsync(purchase);
            }

            return null;
        }
    }
}
