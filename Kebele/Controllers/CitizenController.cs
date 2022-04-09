using Firebase.Auth;
using Firebase.Storage;
using Grpc.Core;
using Kebele.Data;
using Kebele.Extensions;
using Kebele.Models;
//using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNet.Identity;

namespace Kebele.Controllers
{
    [Authorize]
    public class CitizenController : Controller
    {
      
        
        /// <summary>
    /// api receive methods
    /// </summary>
        private readonly HttpClient client = null;
        private string CitizenApiUrl = "";
        private string CitizenApiUrlCity = "";
        private string CitizenApiUrlSSN = "";
        private readonly ApplicationDbContext _context;
        private CitiesController _city;
        private readonly UserManager<IdentityUser> _userManager;
        

        public CitizenController(HttpClient client, IConfiguration config, ApplicationDbContext context, CitiesController city, UserManager<IdentityUser> userManager)
        {
            _city = city;
            this.client = client;
            CitizenApiUrl = config.GetValue<string>("AppSettingCitizen:CitizenApiUrl");
            CitizenApiUrlCity = config.GetValue<string>("AppSettingCity:CitizenApiUrl");
            CitizenApiUrlSSN = config.GetValue<string>("AppSettingSSN:CitizenApiUrl");
            _context = context;
            _userManager = userManager;
            
        }
        public async Task<IActionResult> fil()
        {
           
            HttpResponseMessage response = await client.GetAsync(CitizenApiUrl);
            string stringdata = await response.Content.ReadAsStringAsync();
            var option = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            
            
            List<Citizen> data = JsonSerializer.Deserialize<List<Citizen>>(stringdata, option);
            ViewBag.Success = TempData["Success"];

            return View(data);
        }
        public async Task<IActionResult> Create()
        {
            //CitizenApiUrlci = config.GetValue<string>("AppSettings:CitizenApiUrl");
            HttpResponseMessage responseci = await client.GetAsync(CitizenApiUrlCity);
            string stringdataci = await responseci.Content.ReadAsStringAsync();
            var option = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<City> data = JsonSerializer.Deserialize<List<City>>(stringdataci, option);
            //Ciity c = new City();
            // CitiesController c = new CitiesController(client, config); 
           
            Citizen citizens = new Citizen
            {
               
                Citiy = data.ConvertToSelectList(0)
                


            };
           

            return View(citizens);
        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Citizen citizen)
        {
            //ModelState.AddModelError("Error", "Citizen already Registerd");
            if (ModelState.IsValid)
            {
                // read city data api
                HttpResponseMessage responseCity = await client.GetAsync(CitizenApiUrlCity);
                string stringdatacity = await responseCity.Content.ReadAsStringAsync();
                var option = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                List<City> data = JsonSerializer.Deserialize<List<City>>(stringdatacity, option);

                HttpResponseMessage responseSSN = await client.GetAsync(CitizenApiUrlSSN);
                string stringdataSSN = await responseSSN.Content.ReadAsStringAsync();
                var optionSSN = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                //var identical = data.FirstOrDefault(m => m. == citizen.CityCode);

                var ident = _context.Citizens.Any(m => m.First_Name == citizen.First_Name && m.Mid_Name == citizen.Mid_Name && m.Last_Name == citizen.Last_Name && m.DOB == citizen.DOB);
                if (ident)
                {
                    ViewBag.Message = "Citizen Already Registerd";
                    return View();

                }
                else
                {
                    ///  assign curent kebele user to citizen value
                    var userid = _userManager.GetUserId(HttpContext.User);
                    ApplicationUser user = (ApplicationUser)_userManager.FindByIdAsync(userid).Result;
                    citizen.Kebele = user.Kebele;
                    citizen.Woreda = user.Woreda;
                    citizen.CityCode = user.CityCode;
                    citizen.City = user.City;
                    citizen.SubCity = user.SubCity;
                    List<SSN> SSNData = JsonSerializer.Deserialize<List<SSN>>(stringdataSSN, optionSSN);
                    var currentssn = SSNData.FirstOrDefault(m => m.CityCode == citizen.CityCode);
                    int ssn = int.Parse(currentssn.CityCode.ToString() + currentssn.CurrentNumber.ToString());
                    //var current_user = await _userManager.GetUserAsync(User);
                    //var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // will give the user's userId
                    //var userName = User.FindFirstValue(ClaimTypes.Name); // will give the user's userName

                    //ApplicationUser applicationUser = await UserManager.GetUserAsync(User);
                    //        citizen.Kebele = applicationUser.Kebele;
                    //        citizen.Woreda = applicationUser.Woreda;
                    //string userEmail = applicationUser?.Email;

                    // Write citizen to ssnbcims using api
                    string stringdata = JsonSerializer.Serialize(citizen);
                    var contentdata = new StringContent(stringdata, System.Text.Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(CitizenApiUrl, contentdata);
                    if (response.IsSuccessStatusCode)
                    {
                        
                        var dob = citizen.DOB;
                        int age = 0;
                        age = DateTime.Now.Subtract(dob).Days;
                        age = age / 365;
                        citizen.Age = age;
                        var citn = data.FirstOrDefault(m => m.Code == citizen.CityCode);
                        citizen.City = citn.Name;

                        // ssn update


                        citizen.SSN = ssn;

                        _context.Add(citizen);
                        await _context.SaveChangesAsync();
                        TempData["Success"] = "Citizen Registered Successfuly";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ViewBag.Message = "Error While Calling Api";
                    }


                }
            }


            return RedirectToAction(nameof(Index));


        }
        
        public async Task<IActionResult> Index(string city , string SubCity , string Woreda, int Kebele)
        {
            var userid = _userManager.GetUserId(HttpContext.User);
            ApplicationUser user = (ApplicationUser)_userManager.FindByIdAsync(userid).Result;

            city = user.City;
            SubCity = user.SubCity;
            Woreda = user.Woreda;
            Kebele = user.Kebele;

            HttpResponseMessage response = await client.GetAsync($"{CitizenApiUrl}/{city}/{SubCity}/{Woreda}/{Kebele}");
            string stringdata = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
             };
            ViewBag.Success = TempData["Success"];
            List<Citizen> data = JsonSerializer.Deserialize<List<Citizen>>(stringdata, options);

            return View(data);
        }
      



    }
}
