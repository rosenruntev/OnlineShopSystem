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
    public class ProductsController : Controller
    {
        private const string JSON_MEDIA_TYPE = "application/json";
        private readonly Uri productsUri = new Uri("https://localhost:44336/api/Products");

        // GET: Products
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

                HttpResponseMessage response = await client.GetAsync(productsUri);

                if (!response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(HomeController.Error), "Home");
                }

                string jsonResponse = await response.Content.ReadAsStringAsync();

                var responseData = JsonConvert.DeserializeObject<IEnumerable<ProductViewModel>>(jsonResponse);

                return View(responseData);
            }
        }

        [HttpGet]
        public ActionResult Search()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Search(ProductViewModel product)
        {
            using (var client = new HttpClient())
            {
                var token = HttpContext.Session.GetString("JwtToken");
                if (token == null)
                {
                    return RedirectToAction("Index", "Home");
                }

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await client.GetAsync(productsUri + "/search?name=" + product.Name);

                if (!response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(HomeController.Error), "Home");
                }

                string jsonResponse = await response.Content.ReadAsStringAsync();

                var responseData = JsonConvert.DeserializeObject<IEnumerable<ProductViewModel>>(jsonResponse);

                TempData["FoundProducts"] = responseData;
                return View();
            }
        }

        // GET: Products/Details/5
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

                HttpResponseMessage response = await client.GetAsync($"{productsUri}/{id}");

                if (!response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(HomeController.Error), "Home");
                }

                string jsonResponse = await response.Content.ReadAsStringAsync();

                var responseData = JsonConvert.DeserializeObject<ProductViewModel>(jsonResponse);

                return View(responseData);
            }
        }

        // GET: Products/Create
        [HttpGet]
        public async Task<ActionResult> Create()
        {
            ViewBag.Sellers = await GetSellersDropdownItemsAsync();

            return View();
        }

        // POST: Products/Create
        [HttpPost]
        public async Task<ActionResult> Create(ProductViewModel product)
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

                    product.Seller = await GetUserById(product.Seller.Id);
                    var serializedContent = JsonConvert.SerializeObject(product);
                    var stringContent = new StringContent(serializedContent, Encoding.UTF8, JSON_MEDIA_TYPE);

                    HttpResponseMessage response = await client.PostAsync(productsUri, stringContent);

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

        // GET: Products/Edit/5
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

                HttpResponseMessage response = await client.GetAsync($"{productsUri}/{id}");

                if (!response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(HomeController.Error), "Home");
                }

                string jsonResponse = await response.Content.ReadAsStringAsync();

                var responseData = JsonConvert.DeserializeObject<ProductViewModel>(jsonResponse);
                ViewBag.Sellers = await GetSellersDropdownItemsAsync();

                return View(responseData);
            }
        }

        // POST: Products/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(int id, ProductViewModel product)
        {
            product.Id = id;

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

                    product.Seller = await GetUserById(product.Seller.Id);
                    var serializedContent = JsonConvert.SerializeObject(product);
                    var stringContent = new StringContent(serializedContent, Encoding.UTF8, JSON_MEDIA_TYPE);

                    HttpResponseMessage response = await client.PutAsync($"{productsUri}/{id}", stringContent);

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

        // GET: Products/Delete/5
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

                HttpResponseMessage response = await client.GetAsync($"{productsUri}/{id}");

                if (!response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(HomeController.Error), "Home");
                }

                string jsonResponse = await response.Content.ReadAsStringAsync();

                var responseData = JsonConvert.DeserializeObject<ProductViewModel>(jsonResponse);

                return View(responseData);
            }
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var count = await GetOrdersCountByProductId(id);
                if (count > 0)
                {
                    TempData["Error"] = "There are still pending orders for that product.";
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

                    HttpResponseMessage response = await client.DeleteAsync($"{productsUri}/{id}");

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

        private async Task<IEnumerable<SelectListItem>> GetSellersDropdownItemsAsync()
        {
            using (var client = new HttpClient())
            {
                var token = HttpContext.Session.GetString("JwtToken");
                if (token == null)
                {
                    return null;
                }

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                Uri usersUri = new Uri("https://localhost:44336/api/Users");

                HttpResponseMessage usersResponse = await client.GetAsync(usersUri);

                if (!usersResponse.IsSuccessStatusCode)
                {
                    return Enumerable.Empty<SelectListItem>();
                }

                string usersJsonResponse = await usersResponse.Content.ReadAsStringAsync();

                var users = JsonConvert.DeserializeObject<IEnumerable<UserViewModel>>(usersJsonResponse);

                return users.Select(user => new SelectListItem(user.Name, user.Id.ToString()));
            }
        }

        private async Task<UserViewModel> GetUserById(int id)
        {
            using (var client = new HttpClient())
            {
                var token = HttpContext.Session.GetString("JwtToken");
                if (token == null)
                {
                    return null;
                }

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                Uri userUri = new Uri($"https://localhost:44336/api/Users/{id}");

                HttpResponseMessage userResponse = await client.GetAsync(userUri);

                if (!userResponse.IsSuccessStatusCode)
                {
                    return null;
                }

                string userJsonResponse = await userResponse.Content.ReadAsStringAsync();

                var user = JsonConvert.DeserializeObject<UserViewModel>(userJsonResponse);

                return user;
            }
        }

        private async Task<int?> GetOrdersCountByProductId(int id)
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

                return responseData.Where(o => o.Product.Id == id).Count();
            }
        }
    }
}
