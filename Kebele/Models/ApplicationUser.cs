using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Kebele.Models
{
    public class ApplicationUser:IdentityUser
    {
        public int CityCode { get; set; }

        [DataType(DataType.Text)]
        [Required]
        public string City { get; set; }
        [RegularExpression(@"^[A-Za-z]+[a-zA-Z\s]*$",ErrorMessage ="Input Field Accept Alphabet Only")]
        [DataType(DataType.Text)]
        [Required]
        public string SubCity { get; set; }
        [RegularExpression(@"^[A-Za-z]+[a-zA-Z\s]*$", ErrorMessage = "Input Field Accept Alphabet Only")]
        [DataType(DataType.Text)]
        [Required]
        public string Woreda { get; set; }
        
        [Required]
        public int Kebele { get; set; }
        [NotMapped]
        public virtual ICollection<SelectListItem> Citiy { get; set; }
    }
}
