using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using OSS.Website.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace OSS.Website.Controllers
{
    public class UsersController : Controller
    {
        private const string JSON_MEDIA_TYPE = "application/json";
        private readonly Uri usersUri = new Uri("https://localhost:44336/api/Users");

        // GET: Users
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            using (var client = new HttpClient())
            {
                var token = HttpContext.Session.GetString("JwtToken");
                if (token == null)
                {
                    return RedirectToAction("Index", "Home");
                }

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await client.GetAsync(usersUri);

                if (!response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(HomeController.Error), "Home");
                }

                string jsonResponse = await response.Content.ReadAsStringAsync();

                var responseData = JsonConvert.DeserializeObject<IEnumerable<UserViewModel>>(jsonResponse);

                return View(responseData);
            }
        }

        [HttpGet]
        public ActionResult Search()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Search(UserViewModel user)
        {
            using (var client = new HttpClient())
            {
                var token = HttpContext.Session.GetString("JwtToken");
                if (token == null)
                {
                    return RedirectToAction("Index", "Home");
                }

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await client.GetAsync(usersUri + "/search?name=" + user.Name);

                if (!response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(HomeController.Error), "Home");
                }

                string jsonResponse = await response.Content.ReadAsStringAsync();

                var responseData = JsonConvert.DeserializeObject<IEnumerable<UserViewModel>>(jsonResponse);

                TempData["FoundUsers"] = responseData;
                return View();
            }
        }

        // GET: Users/Details/5
        [HttpGet]
        public async Task<ActionResult> Details(int id)
        {
            using (var client = new HttpClient())
            {
                var token = HttpContext.Session.GetString("JwtToken");
                if (token == null)
                {
                    return RedirectToAction("Index", "Home");
                }

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await client.GetAsync($"{usersUri}/{id}");

                if (!response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(HomeController.Error), "Home");
                }

                string jsonResponse = await response.Content.ReadAsStringAsync();

                var responseData = JsonConvert.DeserializeObject<UserViewModel>(jsonResponse);

                return View(responseData);
            }
        }

        // GET: Users/Create
        [HttpGet]
        public async Task<ActionResult> Create()
        {
            return View();
        }

        // POST: Users/Create
        [HttpPost]
        public async Task<ActionResult> Create(UserViewModel user)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var token = HttpContext.Session.GetString("JwtToken");
                    if (token == null)
                    {
                        return RedirectToAction("Index", "Home");
                    }

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    var serializedContent = JsonConvert.SerializeObject(user);
                    var stringContent = new StringContent(serializedContent, Encoding.UTF8, JSON_MEDIA_TYPE);

                    HttpResponseMessage response = await client.PostAsync(usersUri, stringContent);

                    if (!response.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(HomeController.Error), "Home");
                    }

                    return RedirectToAction(nameof(Index));
                }
            }
            catch
            {
                return RedirectToAction(nameof(HomeController.Error), "Home");
            }
        }

        // GET: Users/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            using (var client = new HttpClient())
            {
                var token = HttpContext.Session.GetString("JwtToken");
                if (token == null)
                {
                    return RedirectToAction("Index", "Home");
                }

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await client.GetAsync($"{usersUri}/{id}");

                if (!response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(HomeController.Error), "Home");
                }

                string jsonResponse = await response.Content.ReadAsStringAsync();

                var responseData = JsonConvert.DeserializeObject<UserViewModel>(jsonResponse);

                return View(responseData);
            }
        }

        // POST: Users/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(int id, UserViewModel user)
        {
            user.Id = id;

            try
            {
                using (var client = new HttpClient())
                {
                    var token = HttpContext.Session.GetString("JwtToken");
                    if (token == null)
                    {
                        return RedirectToAction("Index", "Home");
                    }

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    var serializedContent = JsonConvert.SerializeObject(user);
                    var stringContent = new StringContent(serializedContent, Encoding.UTF8, JSON_MEDIA_TYPE);

                    HttpResponseMessage response = await client.PutAsync($"{usersUri}/{id}", stringContent);

                    if (!response.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(HomeController.Error), "Home");
                    }

                    return RedirectToAction(nameof(Index));
                }
            }
            catch
            {
                return RedirectToAction(nameof(HomeController.Error), "Home");
            }
        }

        // GET: Users/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            using (var client = new HttpClient())
            {
                var token = HttpContext.Session.GetString("JwtToken");
                if (token == null)
                {
                    return RedirectToAction("Index", "Home");
                }

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await client.GetAsync($"{usersUri}/{id}");

                if (!response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(HomeController.Error), "Home");
                }

                string jsonResponse = await response.Content.ReadAsStringAsync();

                var responseData = JsonConvert.DeserializeObject<UserViewModel>(jsonResponse);

                return View(responseData);
            }
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var sellingProductsCount = await GetProductsCountBySellerId(id);
                var pendingOrdersCount = await GetOrdersCountByUserId(id);
                if (sellingProductsCount > 0 || pendingOrdersCount > 0)
                {
                    TempData["Error"] = "You cannot delete user who has products for sale or pending orders.";
                    return RedirectToAction(nameof(Delete));
                }

                using (var client = new HttpClient())
                {
                    var token = HttpContext.Session.GetString("JwtToken");
                    if (token == null)
                    {
                        return RedirectToAction("Index", "Home");
                    }

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    HttpResponseMessage response = await client.DeleteAsync($"{usersUri}/{id}");

                    if (!response.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(HomeController.Error), "Home");
                    }
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction(nameof(HomeController.Error), "Home");
            }
        }

        private async Task<int?> GetProductsCountBySellerId(int id)
        {
            using (var client = new HttpClient())
            {
                var token = HttpContext.Session.GetString("JwtToken");
                if (token == null)
                {
                    return null;
                }

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                Uri productsUri = new Uri("https://localhost:44336/api/Products");
                HttpResponseMessage response = await client.GetAsync(productsUri);

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                string jsonResponse = await response.Content.ReadAsStringAsync();

                var responseData = JsonConvert.DeserializeObject<IEnumerable<ProductViewModel>>(jsonResponse);

                return responseData.Where(p => p.Seller.Id == id).Count();
            }
        }

        private async Task<int?> GetOrdersCountByUserId(int id)
        {
            using (var client = new HttpClient())
            {
                var token = HttpContext.Session.GetString("JwtToken");
                if (token == null)
                {
                    return null;
                }

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                Uri ordersUri = new Uri("https://localhost:44336/api/Orders");
                HttpResponseMessage response = await client.GetAsync(ordersUri);

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                string jsonResponse = await response.Content.ReadAsStringAsync();

                var responseData = JsonConvert.DeserializeObject<IEnumerable<OrderViewModel>>(jsonResponse);

                return responseData.Where(o => o.User.Id == id).Count();
            }
        }
    }
}
