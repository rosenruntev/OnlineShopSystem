using System;
using System.ComponentModel.DataAnnotations;

namespace OSS.Website.Models
{
    public class UserViewModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string Name { get; set; }

        [Required]
        [Range(18, 110)]
        public int Age { get; set; }

        [Required]
        public decimal BankBalance { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? UpdatedOn { get; set; }
    }
}
