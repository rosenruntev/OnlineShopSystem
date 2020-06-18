using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OSS.Website.Models;
using RestSharp;

namespace OSS.Website.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            ModelState.Clear();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> IndexAsync(AccountViewModel account)
        {
            ModelState.Clear();
            using (var client = new HttpClient())
            {
                string JSON_MEDIA_TYPE = "application/json";
                Uri loginUri = new Uri("https://localhost:44336/api/Login");

                var serializedContent = JsonConvert.SerializeObject(account);
                var stringContent = new StringContent(serializedContent, Encoding.UTF8, JSON_MEDIA_TYPE);

                HttpResponseMessage response = await client.PostAsync(loginUri, stringContent);

                if (!response.IsSuccessStatusCode)
                {
                    TempData["LoginMessage"] = "Invalid username or password";
                    return RedirectToAction(nameof(HomeController.Index), "Home");
                }

                string jsonResponse = await response.Content.ReadAsStringAsync();

                HttpContext.Session.SetString("JwtToken", jsonResponse);

                TempData["LoginMessage"] = "You have logged in successfully";

                return View();
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
