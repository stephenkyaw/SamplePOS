namespace Domain.Entities
{
    public class ProductType : BaseEntity
    {
        public string ProductTypeId { get; set; }
        public string Code { get; set; }
        public string Descripton { get; set; }
        public virtual ICollection<Product> Products { get; set;}
    }
}
