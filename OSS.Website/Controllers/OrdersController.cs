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
    public class OrdersController : Controller
    {
        private const string JSON_MEDIA_TYPE = "application/json";
        private readonly Uri ordersUri = new Uri("https://localhost:44336/api/Orders");

        // GET: Orders
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

                HttpResponseMessage response = await client.GetAsync(ordersUri);

                if (!response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(HomeController.Error), "Home");
                }

                string jsonResponse = await response.Content.ReadAsStringAsync();

                var responseData = JsonConvert.DeserializeObject<IEnumerable<OrderViewModel>>(jsonResponse);

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

                UserViewModel u = await GetUserById(user.Id);
                if (u == null)
                {
                    return View();
                }

                var serializedContent = JsonConvert.SerializeObject(u);
                var stringContent = new StringContent(serializedContent, Encoding.UTF8, JSON_MEDIA_TYPE);

                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(ordersUri + "/search"),
                    Content = stringContent
                };

                HttpResponseMessage response = await client.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(HomeController.Error), "Home");
                }

                string jsonResponse = await response.Content.ReadAsStringAsync();

                var responseData = JsonConvert.DeserializeObject<IEnumerable<OrderViewModel>>(jsonResponse);

                TempData["FoundOrders"] = responseData;
                return View();
            }
        }

        // GET: Orders/Details/5
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

                HttpResponseMessage response = await client.GetAsync($"{ordersUri}/{id}");

                if (!response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(HomeController.Error), "Home");
                }

                string jsonResponse = await response.Content.ReadAsStringAsync();

                var responseData = JsonConvert.DeserializeObject<OrderViewModel>(jsonResponse);

                return View(responseData);
            }
        }

        // GET: Orders/Create
        [HttpGet]
        public async Task<ActionResult> Create()
        {
            ViewBag.Sellers = await GetSellersDropdownItemsAsync();
            ViewBag.Products = await GetProductsDropdownItemsAsync();

            return View();
        }

        // POST: Orders/Create
        [HttpPost]
        public async Task<ActionResult> Create(OrderViewModel order)
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

                    order.User = await GetUserById(order.User.Id);
                    order.Product = await GetProductById(order.Product.Id);

                    if (order.Quantity > order.Product.Quantity)
                    {
                        TempData["Error"] = "You cannot order bigger quantity than the product's available quantity";
                        return RedirectToAction(nameof(Create));
                    }

                    var serializedContent = JsonConvert.SerializeObject(order);
                    var stringContent = new StringContent(serializedContent, Encoding.UTF8, JSON_MEDIA_TYPE);

                    HttpResponseMessage response = await client.PostAsync(ordersUri, stringContent);

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

        // GET: Orders/Edit/5
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

                HttpResponseMessage response = await client.GetAsync($"{ordersUri}/{id}");

                if (!response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(HomeController.Error), "Home");
                }

                string jsonResponse = await response.Content.ReadAsStringAsync();

                var responseData = JsonConvert.DeserializeObject<OrderViewModel>(jsonResponse);
                ViewBag.Sellers = await GetSellersDropdownItemsAsync();
                ViewBag.Products = await GetProductsDropdownItemsAsync();

                return View(responseData);
            }
        }

        // POST: Orders/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(int id, OrderViewModel order)
        {
            order.Id = id;

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

                    order.User = await GetUserById(order.User.Id);
                    order.Product = await GetProductById(order.Product.Id);

                    var serializedContent = JsonConvert.SerializeObject(order);
                    var stringContent = new StringContent(serializedContent, Encoding.UTF8, JSON_MEDIA_TYPE);

                    HttpResponseMessage response = await client.PutAsync($"{ordersUri}/{id}", stringContent);

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

        // GET: Orders/Delete/5
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

                HttpResponseMessage response = await client.GetAsync($"{ordersUri}/{id}");

                if (!response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(HomeController.Error), "Home");
                }

                string jsonResponse = await response.Content.ReadAsStringAsync();

                var responseData = JsonConvert.DeserializeObject<OrderViewModel>(jsonResponse);

                return View(responseData);
            }
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(int id)
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

                    HttpResponseMessage response = await client.DeleteAsync($"{ordersUri}/{id}");

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

        private async Task<ProductViewModel> GetProductById(int id)
        {
            using (var client = new HttpClient())
            {
                var token = HttpContext.Session.GetString("JwtToken");
                if (token == null)
                {
                    return null;
                }

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                Uri productsUri = new Uri($"https://localhost:44336/api/Products/{id}");

                HttpResponseMessage productResponse = await client.GetAsync(productsUri);

                if (!productResponse.IsSuccessStatusCode)
                {
                    return null;
                }

                string productJsonResponse = await productResponse.Content.ReadAsStringAsync();

                var product = JsonConvert.DeserializeObject<ProductViewModel>(productJsonResponse);

                return product;
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

        private async Task<IEnumerable<SelectListItem>> GetProductsDropdownItemsAsync()
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

                HttpResponseMessage productsResponse = await client.GetAsync(productsUri);

                if (!productsResponse.IsSuccessStatusCode)
                {
                    return Enumerable.Empty<SelectListItem>();
                }

                string productsJsonResponse = await productsResponse.Content.ReadAsStringAsync();

                var products = JsonConvert.DeserializeObject<IEnumerable<UserViewModel>>(productsJsonResponse);

                return products.Select(user => new SelectListItem(user.Name, user.Id.ToString()));
            }
        }
    }
}
