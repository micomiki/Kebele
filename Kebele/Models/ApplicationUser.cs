using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Kebele.Models
{
    public class ApplicationUser:IdentityUser
    {
        [DataType(DataType.Text)]
        [Required]
        public string City { get; set; }

        [DataType(DataType.Text)]
        [Required]
        public string SubCity { get; set; }

        [DataType(DataType.Text)]
        [Required]
        public string Woreda { get; set; }
        
        [Required]
        public int Kebele { get; set; }
    }
}
