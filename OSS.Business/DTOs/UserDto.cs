namespace OSS.Business.DTOs
{
    public class UserDto : BaseDto
    {
        public string Name { get; set; }

        public int Age { get; set; }

        public decimal BankBalance { get; set; }

        public override bool IsValid()
        {
            if (Id < 0 || Name == null || Name.Length > 20 || Age < 18 || Age > 110 || BankBalance < 0 || CreatedOn == null)
            {
                return false;
            }

            return true;
        }
    }
}
