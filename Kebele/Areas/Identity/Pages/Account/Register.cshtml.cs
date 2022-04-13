using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Kebele.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.Rendering;
using Kebele.Extensions;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Text.Json;

namespace Kebele.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private string CitizenApiUrlCity = "";
        private readonly HttpClient client = null;

        public RegisterModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender, IConfiguration config, HttpClient client)
        {
            this.client = client;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            CitizenApiUrlCity = config.GetValue<string>("AppSettingCity:CitizenApiUrl");
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
            public int CityCode { get; set; }
            
            
            [DataType(DataType.Text)]
            [Required]
            public string City { get; set; }
            [RegularExpression(@"^[A-Za-z]+[a-zA-Z\s]*$", ErrorMessage = "Input Field Accept Alphabet Only")]
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
        public List<SelectListItem> Options { get; set; }
        public async Task OnGetAsync(string returnUrl = null)
        {
            
            //CitizenApiUrlci = config.GetValue<string>("AppSettings:CitizenApiUrl");
            HttpResponseMessage responseci = await client.GetAsync(CitizenApiUrlCity);
            string stringdataci = await responseci.Content.ReadAsStringAsync();
            var option = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<City> data = JsonSerializer.Deserialize<List<City>>(stringdataci, option);
            Options = data.ConvertToSelectList(0);
            //Ciity c = new City();
            // CitiesController c = new CitiesController(client, config); 

            //Input.Citiy = data.ConvertToSelectList(0);
           // RegisterModel.InputModel rg = new RegisterModel.InputModel{
             //   Citiy = data.ConvertToSelectList(0)
                
            //};
            
            //this.Input.Citiy = rg.Citiy;
            //var inp = Input.Citiy;
            //this.Input.Citiy
            //ViewData["las"] = Input.Citiy;
           // ViewData["las"] = 

            //InputModel inp = new InputModel {

            //    Citiy = data.ConvertToSelectList(0)
            //};


            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            
            
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                HttpResponseMessage responseCity = await client.GetAsync(CitizenApiUrlCity);
                string stringdatacity = await responseCity.Content.ReadAsStringAsync();
                var option = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                List<City> data = JsonSerializer.Deserialize<List<City>>(stringdatacity, option);
                var citn = data.FirstOrDefault(m => m.Code == Input.CityCode);
                Input.City = citn.Name;
                Input.CityCode = citn.Code;
                var user = new ApplicationUser { 
                    UserName = Input.Email, 
                    Email = Input.Email,
                    City = Input.City,
                    SubCity = Input.SubCity,
                    Woreda = Input.Woreda,
                    Kebele = Input.Kebele,
                    CityCode = Input.CityCode
                    

                };
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
