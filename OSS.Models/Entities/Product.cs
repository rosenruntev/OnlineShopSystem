namespace OSS.Models.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public virtual User Seller { get; set; }

        public string Description { get; set; }
    }
}
