using Firebase.Auth;
using Firebase.Storage;
using Grpc.Core;
using Kebele.Data;
using Kebele.Extensions;
using Kebele.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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

namespace Kebele.Controllers
{
    public class CitizenController : Controller
    {
      
        
        /// <summary>
    /// api receive methods
    /// </summary>
        private readonly HttpClient client = null;
        private string CitizenApiUrl = "";
        private string CitizenApiUrlci = "";
        private readonly ApplicationDbContext _context;

        
        public CitizenController(HttpClient client, IConfiguration config, ApplicationDbContext context)
        {
            
            this.client = client;
            CitizenApiUrl = config.GetValue<string>("AppSettings:CitizenApiUrl");
            CitizenApiUrlci = config.GetValue<string>("AppSetting:CitizenApiUrl");
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
           
            HttpResponseMessage response = await client.GetAsync(CitizenApiUrl);
            string stringdata = await response.Content.ReadAsStringAsync();
            var option = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            
            
            List<Citizen> data = JsonSerializer.Deserialize<List<Citizen>>(stringdata, option);
           

            return View(data);
        }
        public async Task<IActionResult> Create()
        {
            //CitizenApiUrlci = config.GetValue<string>("AppSettings:CitizenApiUrl");
            HttpResponseMessage responseci = await client.GetAsync(CitizenApiUrlci);
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
            if (ModelState.IsValid)
            {
                Citizen c = new Citizen();
              
                string stringdata = JsonSerializer.Serialize(citizen);
                var contentdata = new StringContent(stringdata, System.Text.Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(CitizenApiUrl, contentdata);
                if (response.IsSuccessStatusCode)
                {
                    ViewBag.Message = "Citizen inserted successfully!";
                    _context.Add(citizen);
                    await _context.SaveChangesAsync();

                }
                else
                {
                    ViewBag.Message = "Error While Calling Api";
                }

            }
            return Content(citizen.Age.ToString());
        }
        


    }
}
