using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kebele.Utility
{
    public static class Helper
    {
        public static string Aplus = "A+";
        public static string A = "A";
        public static string Aminus = "A-";
        public static string Bplus = "B+";
        public static string B = "B";
        public static string Bminus = "B-";
        public static string ABplus = "AB+";
        public static string AB = "AB";
        public static string ABminus = "AB-";
        public static string Oplus = "O+";
        public static string O = "O";
        public static string Ominus = "O-";


        public static List<SelectListItem> GetBloodType() 
        {
            return new List<SelectListItem>
           {
            new SelectListItem{ Value=Helper.Aplus, Text=Helper.Aplus},
            new SelectListItem{ Value=Helper.A, Text=Helper.A},
            new SelectListItem{ Value=Helper.Aminus, Text=Helper.Aminus},
            new SelectListItem{ Value=Helper.ABplus, Text=Helper.ABplus},
            new SelectListItem{ Value=Helper.AB, Text=Helper.AB},
            new SelectListItem{ Value=Helper.Aminus, Text=Helper.Aminus},
            new SelectListItem{ Value=Helper.Bplus, Text=Helper.Bplus},
            new SelectListItem{ Value=Helper.B, Text=Helper.B},
            new SelectListItem{ Value=Helper.Bminus, Text=Helper.Bminus},
            new SelectListItem{ Value=Helper.Oplus, Text=Helper.Oplus},
            new SelectListItem{ Value=Helper.O, Text=Helper.O},
            new SelectListItem{ Value=Helper.Ominus, Text=Helper.Ominus},
           };
        }
    }
}
