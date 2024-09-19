using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Test.Models;



namespace Test.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;


        public CategoryController(IHttpClientFactory httpClientFactory)
        {

            _httpClientFactory = httpClientFactory;
        }


        [HttpGet]
        public async Task<IActionResult> Category()
        {
            var client = _httpClientFactory.CreateClient("YourApiClient");

            // Make a GET request to the API endpoint
            var response = await client.GetAsync("Category");

            if (response.IsSuccessStatusCode)
            {
                // Read the response content as a string
                var apiResponse = await response.Content.ReadAsStringAsync();

                // Deserialize the JSON response to a model
                var data = JsonConvert.DeserializeObject<IEnumerable<ViewModelCategories>>(apiResponse);

                // Pass the deserialized data to the view
                return View(data);
            }
            else
            {
                ViewBag.ApiResponse = "Error fetching data from API.";
                return View();
            }

        }
        public IActionResult GetCategory()
        {
            return View();
        }


        public async Task<IActionResult> AddCategoryAsync(ViewModelCategories model)
        {
            var client = _httpClientFactory.CreateClient("YourApiClient");
            if (model.Id == 0 || ModelState.IsValid)
            {



                var jsonContent = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

                var response = await client.PostAsync("CategoryPost", jsonContent);



            }
            return RedirectToAction("Category");

        }


        [HttpGet]
        public async Task<IActionResult> GetCategory(int Id)
        {
            var client = _httpClientFactory.CreateClient("YourApiClient");

            // Make a GET request to the API endpoint
            var response = await client.GetAsync($"CategoryGet?Id={Id}");

            if (response.IsSuccessStatusCode)
            {
                ViewModelCategories obj = new();
                // Read the response content as a string
                var apiResponse = await response.Content.ReadAsStringAsync();

                // Deserialize the JSON response to a model
                var data = JsonConvert.DeserializeObject<List<ViewModelCategories>>(apiResponse);
                foreach (var category in data)
                {
                    obj = new ViewModelCategories
                    {
                        Id = category.Id,
                        Category = category.Category,
                        Status = category.Status
                    };
                }
                // Pass the deserialized data to the view
                return View(obj);
            }
            else
            {
                ViewBag.ApiResponse = "Error fetching data from API.";
                return View();
            }
        }



        public async Task<IActionResult> EditAsync(ViewModelCategories model)
        {
            var client = _httpClientFactory.CreateClient("YourApiClient");
            if (model.Id != 0 || ModelState.IsValid)
            {



                var jsonContent = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

                var response = await client.PostAsync("EditCategory", jsonContent);



            }
            return RedirectToAction("Category");

        }



        [HttpPost]

        public async Task<IActionResult> Delete(int Id)
        {
            var client = _httpClientFactory.CreateClient("YourApiClient");
            var response = await client.GetAsync($"Delete?id={Id}");
            if (response.IsSuccessStatusCode)
            {
                // Read the response content as a string
                return RedirectToAction("Category");

            }
            return Ok();
        }
    }
}

