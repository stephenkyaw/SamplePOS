namespace Domain.Entities
{
    public class Customer : BaseEntity
    {
        public string CustomerId { get; set; }
        public string Name { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public virtual ICollection<SaleOrder> SaleOrders { get; set; }

        //Add Member Info
        public string MemberCode { get; set; }
        public decimal TotalPoints { get; set; } = 0;
        public decimal TotalPurchasedAmount { get; set; } = 0;
        
        //Identity UserId
        public string UserId { get; set; }
    }
}
