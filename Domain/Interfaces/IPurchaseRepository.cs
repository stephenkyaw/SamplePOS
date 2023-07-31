namespace Domain.Interfaces
{
    public interface IPurchaseRepository
    {
        Task<Purchase> GetPurchaseByIdAsync(string PurchaseId);
        Task DeletePurchaseByIdAsync(string PurchaseId);
        Task<Purchase> AddPurchaseAsync(Purchase purchase);
        Task<Purchase> UpdatePurchaseAsync(Purchase purchase);
    }
}
