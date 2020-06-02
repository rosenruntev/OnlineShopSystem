namespace OSS.Business.DTOs
{
    public class ProductDto : BaseDto
    {
        public string Name { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public UserDto Seller { get; set; }

        public string Description { get; set; }

        public override bool IsValid()
        {
            if (Id < 0 || Name == null || Name.Length > 20 || Price < 0 || Quantity < 0 || !Seller.IsValid() ||
                (Description != null && Description.Length > 100) || CreatedOn == null)
            {
                return false;
            }

            return true;
        }
    }
}
