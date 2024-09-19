using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Test.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Test.Controllers
{
    [Authorize]
    public class SubCategoryController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;


        public SubCategoryController(IHttpClientFactory httpClientFactory)
        {

            _httpClientFactory = httpClientFactory;
        }


        public async Task<IActionResult> AddSubCategory(ViewModelSubCategories model)
        {
            var client = _httpClientFactory.CreateClient("YourApiClient");
            if (model.Id == 0 || ModelState.IsValid)
            {



                var jsonContent = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

                var response = await client.PostAsync("SubCategoryPost", jsonContent);



            }
            return RedirectToAction("SubCategory");

        }



        [HttpGet]
        public async Task<IActionResult> GetSubCategory(int Id)
        {
            var client = _httpClientFactory.CreateClient("YourApiClient");

            // Make a GET request to the API endpoint
            var response = await client.GetAsync($"SubCategoryGet?Id={Id}");

            if (response.IsSuccessStatusCode)
            {
                ViewModelSubCategories obj = new();
                // Read the response content as a string
                var apiResponse = await response.Content.ReadAsStringAsync();

                // Deserialize the JSON response to a model
                var data = JsonConvert.DeserializeObject<ViewModelSubCategories>(apiResponse);

                data.Categoryy = data.Category.Category;
                data.CategoryId = data.Category.Id;
                return View(data);
            }
            else
            {
                ViewBag.ApiResponse = "Error fetching data from API.";
                return View();
            }
        }
        public async Task<IActionResult> SubCategoryAsync()
        {
            var client = _httpClientFactory.CreateClient("YourApiClient");

            // Make a GET request to the API endpoint
            var response = await client.GetAsync("SubCategory");
            List<ViewModelSubCategories> obj1 = new List<ViewModelSubCategories>();
            ViewModelSubCategories obj = new();
            if (response.IsSuccessStatusCode)
            {
                // Read the response content as a string
                var apiResponse = await response.Content.ReadAsStringAsync();

                // Deserialize the JSON response to a model
                var data = JsonConvert.DeserializeObject<IEnumerable<ViewModelSubCategories>>(apiResponse);
                foreach (var category in data)
                {

                    obj = new ViewModelSubCategories
                    {
                        Id = category.Id,

                        Categoryy = category.Category.Category,

                        Status = category.Status,
                        Name = category.Name
                    };
                    obj1.Add(obj);

                }
                // Pass the deserialized data to the view
                return View(obj1);
            }
            else
            {
                ViewBag.ApiResponse = "Error fetching data from API.";
                return View();
            }

        }



        [HttpPost]
        public async Task<IActionResult> EditSub(ViewModelSubCategories model)
        {
            var client = _httpClientFactory.CreateClient("YourApiClient");
            if (model.Id != 0 || ModelState.IsValid)
            {
                var jsonContent = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

                var response = await client.PostAsync("EditSubCategory", jsonContent);
            }
            return RedirectToAction("SubCategory");

        }


        [HttpPost]

        public async Task<IActionResult> DeleteSub(int Id)
        {
            var client = _httpClientFactory.CreateClient("YourApiClient");
            var response = await client.GetAsync($"DeleteSub?id={Id}");
            if (response.IsSuccessStatusCode)
            {
                // Read the response content as a string
                return RedirectToAction("Category","Category");

            }
            return Ok();
        }
    }
}

