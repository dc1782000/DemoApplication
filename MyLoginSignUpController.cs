using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Test.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Test.Controllers
{
    public class MyLoginSignUpController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;


        public MyLoginSignUpController(IHttpClientFactory httpClientFactory)
        {

            _httpClientFactory = httpClientFactory;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Admin(ResponseClass res)
        {
            return View(res);
        }


        public IActionResult Registration()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> RegistrationAsync(ViewModelSignUpDetails model)
        {
            var client = _httpClientFactory.CreateClient("YourApiClient");
            if (model.Id == 0 || ModelState.IsValid)
            {
                if (model.Password == model.ConfirmPassword)
                {
                    var jsonContent = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

                    var response = await client.PostAsync("Signup", jsonContent);
                    TempData["message"] = "Sign up Successfully";
                }
                else
                {
                    TempData["message"] = "password not match";
                }
            }
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> LoginAsync(Login login)
        {
            var client = _httpClientFactory.CreateClient("YourApiClient");
            if (login.Email != null || ModelState.IsValid)
            {

                var jsonContent = new StringContent(JsonConvert.SerializeObject(login), Encoding.UTF8, "application/json");

                var response = await client.PostAsync("Login", jsonContent);
                if (response.IsSuccessStatusCode)
                {

                    var responseContent = await response.Content.ReadAsStringAsync();

                    // Deserialize the response content to a class that represents the structure of the response
                    var responseObject = JsonConvert.DeserializeObject<ResponseClass>(responseContent);
                    string s = responseObject.Token;
                    var res = await client.GetAsync($"CategoryGetByEmail?Email={login.Email}");

                    if (res.IsSuccessStatusCode)
                    {

                        // Read the response content as a string
                        var apiResponse = await res.Content.ReadAsStringAsync();

                        // Deserialize the JSON response to a model
                        var data = JsonConvert.DeserializeObject<Status>(apiResponse);

                        // Assuming 'data' is your Status object
                        var serializedData = JsonConvert.SerializeObject(data);

                        // Create a cookie with the serialized data
                        var cookieOptions = new CookieOptions
                        {
                            // You can set additional options like expiration, domain, path, etc.
                            Expires = DateTimeOffset.UtcNow.AddHours(1), // Example: Expires in 1 hour
                            HttpOnly = false,
                            Secure = true, // Set to true if using HTTPS
                            SameSite = SameSiteMode.Strict // Adjust as needed
                        };
                        // Read the response content as a string
                        
                        // Save the cookie
                        Response.Cookies.Append("StatusCookie", serializedData, cookieOptions);
                                var claims = new List<Claim>
                                {
                                    new Claim(ClaimTypes.Name, login.Email),
                                    // Add additional claims if needed
                                };

                        var userIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                        ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);
                        HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal );
                    }
                    TempData["message"] = "Login Successfully";
                    return RedirectToAction("Admin", responseObject);
                }

                TempData["message"] = "Login information incorrect";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["message"] = "Login information incorrect";
                return RedirectToAction("Index");
            }

        }



        public async Task<IActionResult> Logout()
        {
            var serializedData = Request.Cookies["StatusCookie"];
            if (serializedData != null)
            {
                var statusData = JsonConvert.DeserializeObject<Status>(serializedData);
                // Use 'statusData' as needed
                var client = _httpClientFactory.CreateClient("YourApiClient");
                var res = await client.GetAsync($"Logout?Email={statusData.Email}");
                if (res.IsSuccessStatusCode)
                {
                    var response = await client.GetAsync($"CategoryGetByEmail?Email={statusData.Email}");
                    if (response.IsSuccessStatusCode)
                    {

                        // Read the response content as a string
                        var apiResponse = await response.Content.ReadAsStringAsync();

                        // Deserialize the JSON response to a model
                        var data = JsonConvert.DeserializeObject<Status>(apiResponse);

                        // Assuming 'data' is your Status object
                        var sd = JsonConvert.SerializeObject(data);

                        // Create a cookie with the serialized data
                        var cookieOptions = new CookieOptions
                        {
                            // You can set additional options like expiration, domain, path, etc.
                            Expires = DateTimeOffset.UtcNow.AddHours(1), // Example: Expires in 1 hour
                            HttpOnly = false,
                            Secure = true, // Set to true if using HTTPS
                            SameSite = SameSiteMode.Strict // Adjust as needed
                        };
                        
                        
                        // Save the cookie
                        Response.Cookies.Append("StatusCookie", sd, cookieOptions);
                        await HttpContext.SignOutAsync();

                    }
                    return RedirectToAction("Index");

                }

            }
            return RedirectToAction("Admin");
        }


    }
}

