namespace Domain.Entities
{
    public class Product : BaseEntity
    {
        public Product()
        {
            ProductId = Guid.NewGuid().ToString();
        }

        public Product(string _code,string _description,decimal _buyingPrice,decimal _sellingPrice,int _quantity)
        {
            ProductId = Guid.NewGuid().ToString();
            Code = _code;
            Description = _description;
            BuyingPrice = _buyingPrice;
            SellingPrice = _sellingPrice;
            Quantity = _quantity;
        }

        public string ProductId { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string ProductTypeId { get; set; }
        public virtual ProductType ProductType { get; set; }
        public decimal BuyingPrice { get; set; }
        public decimal SellingPrice { get; set; }
        public int Quantity { get; set; }
        public virtual ICollection<SaleOrderItem> SaleOrderItems { get; set; }
    }
}
