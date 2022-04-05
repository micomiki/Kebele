using Kebele.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Kebele.Controllers
{
    public class CitiesController : Controller
    {
        private readonly HttpClient client = null;
        private string CitizenApiUrl = "";
        public CitiesController(HttpClient client, IConfiguration config)
        {
            this.client = client;
            CitizenApiUrl = config.GetValue<string>("AppSetting:CitizenApiUrl");
        }
        public async Task<IActionResult> Index()
        {
            HttpResponseMessage response = await client.GetAsync(CitizenApiUrl);
            string stringdata = await response.Content.ReadAsStringAsync();
            var option = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<City> data = JsonSerializer.Deserialize<List<City>>(stringdata, option);
            return View(data);
        }
    }
}
