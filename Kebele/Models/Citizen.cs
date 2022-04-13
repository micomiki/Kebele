﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;

namespace Kebele.Models
{
    public class Citizen
    {

       [Key,DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SSN { get; set; }
        [Display(Name = "Image Path")]
        
        public string Image { get; set; }

        [RegularExpression(@"^[A-Za-z]+[a-zA-Z\s]*$", ErrorMessage = "Input Field Accept Alphabet Only")]
        [Display(Name = "First Name")]
        [Required]
        [DataType(DataType.Text)]
        public string First_Name { get; set; }
        [RegularExpression(@"^[A-Za-z]+[a-zA-Z\s]*$", ErrorMessage = "Input Field Accept Alphabet Only")]
        [Display(Name = "Middle Name")]
        [Required]
        [DataType(DataType.Text)]
        public string Mid_Name { get; set; }
        [RegularExpression(@"^[A-Za-z]+[a-zA-Z\s]*$", ErrorMessage = "Input Field Accept Alphabet Only")]
        [Display(Name = "Last Name")]
        [Required]
        [DataType(DataType.Text)]
        public string Last_Name { get; set; }

        [Display(Name = "Date Of Birth")]
        [Required]
        [DataType(DataType.Date)]
        public DateTime DOB { get; set; }

        [Display(Name = "City Code")]
        //[Required]
        // [DataType(DataType.Text)]
        public int CityCode { get; set; }
        [Display(Name = "City")]
        public string City { get; set; }

       
        [DataType(DataType.Text)]
        public string SubCity { get; set; }


        public string Woreda { get; set; }

        public int Kebele { get; set; }

        [Display(Name = "Blood Type")]
        [Required]
        [DataType(DataType.Text)]
        [StringLength(10)]
        public string Blood_Type { get; set; }

        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        
        
        public int Age { get; set; }
        [NotMapped]
        public virtual ICollection<SelectListItem> Citiy { get; set; }
        


    }
}
