namespace Application.DTOs.Point
{
    public class PointDto
    {
        public string MemberCode { get; set; }
        public string CouponCode { get; set; }
        public string ReceiptNumber { get; set; }
        public List<PointItemDto> Items { get; set; } = new();
    }

    public class PointItemDto
    {
        public PointItemDto(string productCode, string productType, decimal price, int quantity, decimal totalPrice)
        {
            ProductCode = productCode;
            ProductType = productType;
            Price = price;
            Quantity = quantity;
            TotalPrice = totalPrice;
        }

        public string ProductCode { get; set; }
        public string ProductType { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }

        
    }
}
