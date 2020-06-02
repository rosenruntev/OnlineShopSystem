namespace OSS.Models.Entities
{
    public class Order : BaseEntity
    {
        public virtual User User { get; set; }

        public virtual Product Product { get; set; }

        public int Quantity { get; set; }

        public decimal TotalPrice { get; set; }

        public string Remarks { get; set; }
    }
}
