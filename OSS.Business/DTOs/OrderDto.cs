namespace OSS.Business.DTOs
{
    public class OrderDto : BaseDto
    {
        public UserDto User { get; set; }

        public ProductDto Product { get; set; }

        public int Quantity { get; set; }

        public decimal TotalPrice { get; set; }

        public string Remarks { get; set; }

        public override bool IsValid()
        {
            if (Id < 0 || !User.IsValid() || !Product.IsValid() || Quantity < 0 || TotalPrice < 0 ||
                (Remarks != null && Remarks.Length > 100) || CreatedOn == null)
            {
                return false;
            }

            return true;
        }
    }
}
