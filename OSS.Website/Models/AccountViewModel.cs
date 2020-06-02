using System.ComponentModel.DataAnnotations;

namespace OSS.Website.Models
{
    public class AccountViewModel
    {
        [Required]
        public string Username { get; set; }

        public string Email { get; set; }

        public string FullName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
