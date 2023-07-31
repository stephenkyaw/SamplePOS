using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{

    /// <summary>
    /// Use For Redis
    /// </summary>
    public class Purchase
    {
        public string PurchaseId { get; set; } = string.Empty;
        public string MemberCode { get; set; } = string.Empty;
        public string CouponCode { get; set; } = string.Empty;
        public string CustoemrId { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public decimal? TotalAmount { get; set; }
        public int TotalItemQuantity { get; set; }
        public List<PurchaseItem> Items { get; set; } = new();
    }
    public class PurchaseItem
    {
        public string PurchaseId { get;set;} = string.Empty;

        [Required(ErrorMessage = "Required ProductId.")]
        public string ProductId { get; set; }

        [Required(ErrorMessage = "Required ProductName.")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "Required ProductType.")]
        public string ProductType { get; set; }

        [Required(ErrorMessage = "Required Price.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Required Quantity.")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Required Quantity.")]
        public decimal TotalAmount { get; set; }
    }
}
