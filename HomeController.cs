using System.Diagnostics;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Test.Models;
using TestApi.Model;

namespace Test.Controllers;
[Authorize]
public class HomeController : Controller
{
    
    private readonly IHttpClientFactory _httpClientFactory;


    public HomeController( IHttpClientFactory httpClientFactory)
    {
        
        _httpClientFactory = httpClientFactory;
    }


    public async Task<IActionResult> ChangePw()
    {

        return View();
    }


    [HttpPost]
    public async Task<IActionResult> ChangePw(ChangePwModel changePwModel)
    {
        if (changePwModel.NewPassword == changePwModel.ConfirmPassword) { 
        var client = _httpClientFactory.CreateClient("YourApiClient");
        var serializedData = Request.Cookies["StatusCookie"];
        if (serializedData != null)
        {
            var statusData = JsonConvert.DeserializeObject<Status>(serializedData);
            changePwModel.Email = statusData.Email;
            var jsonContent = new StringContent(JsonConvert.SerializeObject(changePwModel), Encoding.UTF8, "application/json");

            var response = await client.PostAsync("ChangePw", jsonContent);
                if (response.IsSuccessStatusCode)
                {
                    TempData["message"] = "<script>alert('Password Updated Successfully')</script>";
                    return RedirectToAction("Admin","MyLoginSignUp");
                }
                
            }
        }
        TempData["message"] = "<script>alert('Password not Updated wrong input')</script>";
        return View();
    }

    


    
}

