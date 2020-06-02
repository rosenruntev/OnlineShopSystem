using OSS.Business.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OSS.Website.Models
{
    public class OrderViewModel
    {
        public int Id { get; set; }

        [Required]
        public virtual UserViewModel User { get; set; }

        [Required]
        public virtual ProductViewModel Product { get; set; }

        [Required]
        public int Quantity { get; set; }

        public decimal TotalPrice { get; set; }

        public string Remarks { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? UpdatedOn { get; set; }
    }
}
