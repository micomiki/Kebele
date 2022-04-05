using Kebele.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Kebele.Models
{
    public class SSN
    {
        [Key]
        [Display(Name = "City Code")]
        [Required]

        public int CityCode { get; set; }

        [Display(Name = "Current Code")]
        [Required]
        public int CurrentNumber { get; set; }
    }
}
