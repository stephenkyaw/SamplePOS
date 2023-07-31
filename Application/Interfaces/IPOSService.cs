using Domain.Entities;

namespace Application.Interfaces
{
    public interface IPOSService
    {
        Task<Purchase> ScanMemberQRCode(string memberQRCodeCode,string purchaseId);

        Task<Purchase> ScanCuponQRCode(string couponQRCode,string purchaseId);

        Task<Purchase> Purchase(Purchase purchase);
    }
}
