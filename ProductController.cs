using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Test.Models;
using TestApi.Model;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Test.Controllers
{
    public class ProductController : Controller
    {

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(IHttpClientFactory httpClientFactory,IWebHostEnvironment webHostEnvironment)
        {

            _httpClientFactory = httpClientFactory;
            _webHostEnvironment = webHostEnvironment;
        }


        public async Task<IActionResult> Product()
        {


            var client = _httpClientFactory.CreateClient("YourApiClient");

            // Make a GET request to the API endpoint
            var response = await client.GetAsync("GetProducts");

            if (response.IsSuccessStatusCode)
            {
                // Read the response content as a string
                var apiResponse = await response.Content.ReadAsStringAsync();

                // Deserialize the JSON response to a model
                var data = JsonConvert.DeserializeObject<IEnumerable<ProductData>>(apiResponse);

                // Convert ProductData to ViewModelProduct
                var viewModelProducts = new List<ViewModelProduct>();
                foreach (var productData in data)
                {
                    var viewModelProduct = new ViewModelProduct
                    {
                        Id = productData.ProductId,

                        Categoryy = productData.Category,
                        CategoryId = productData.CategoryId,
                        SubCategories = null, // You may need to set this based on your application logic
                                              // SubCategoriess = productData.Subcategories,
                        SubCategoriessId = productData.SubcategoriesId,
                        SubCategoriessName = string.Join(", ", productData.Subcategories), // You may need to set this based on your application logic
                        Name = productData.ProductName,
                        ShortDescription = productData.ShortDescription,
                        Status = productData.Status,
                        FullDesciption = string.Join(" ", productData.Titles.Concat(productData.Headings).Concat(productData.Descriptions))
                    };

                    viewModelProducts.Add(viewModelProduct);
                }

                // Pass the deserialized data to the view
                return View(viewModelProducts);
            }
            else
            {
                ViewBag.ApiResponse = "Error fetching data from API.";
                return View();
            }
        }






        [HttpPost]
        public async Task<IActionResult> ProductAsync(ViewModelProduct model)
        {


            string name = model.Name;
            bool status = model.Status;
            string shortdec = model.ShortDescription;
            // Access lists
            List<string> titles = model.Title;
            List<string> headings = model.Heading;
            List<string> descriptions = model.Description;
            List<string> pdfHeadings = model.PdfHeading;
            List<ImageClass> formFiless = model.FormFiless;


            model.PdfName = new List<string>();
            if (model.Picture != null)
            {
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(model.Picture.FileName);

                // Combine the path to wwwroot/pdfuploads with the unique file name
                string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "imagesuploads", uniqueFileName);

                // Save the file to the specified path
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.Picture.CopyToAsync(stream);
                }

                // Add the unique file name to the PdfName list
                model.PictureName=uniqueFileName;
            }

            if (pdfHeadings != null && formFiless != null && pdfHeadings.Count == formFiless.Count)
            {
                // Iterate through each PDF file
                for (int i = 0; i < pdfHeadings.Count; i++)
                {
                    // Access PdfHeading and FormFile for the current index
                    string pdfHeading = pdfHeadings[i];
                    IFormFile pdfFile = formFiless[i].formFile;

                    // Check if the PDF file is not null
                    if (pdfFile != null)
                    {
                        // Generate a unique file name (you can customize this logic)
                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(pdfFile.FileName);

                        // Combine the path to wwwroot/pdfuploads with the unique file name
                        string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "pdfuploads", uniqueFileName);

                        // Save the file to the specified path
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await pdfFile.CopyToAsync(stream);
                        }

                        // Add the unique file name to the PdfName list
                        model.PdfName.Add(uniqueFileName);
                    }
                }
            }
            // Access Category and Categoryy

            string categoryy = model.Categoryy;


            List<string> subCategoriess = model.SubCategoriess;

            CompleteModelProduct obj = new CompleteModelProduct()
            {
                Name = name,
                Statuss = status,
                Shortdecription = shortdec,
                Titles = titles,
                Headings = headings,
                Descriptions = descriptions,
                Category = categoryy,
                SubCategoriess = subCategoriess,
                PdfHeadings = pdfHeadings,
                PdfName = model.PdfName,
                Content=model.Content,
                PictureName=model.PictureName

            };






            var client = _httpClientFactory.CreateClient("YourApiClient");
            if (model.Id == 0 || ModelState.IsValid)
            {

                var jsonContent = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");

                var response = await client.PostAsync("PostProduct", jsonContent);
                if (response.IsSuccessStatusCode)
                {
                    TempData["message"] = "<script>alert('Product Added Successfully')</script>";
                    return RedirectToAction("Product");
                }
            }
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> GetProductAsync(int Id)
        {
            var client = _httpClientFactory.CreateClient("YourApiClient");

            var response = await client.GetAsync($"GetProductDetailByProductId?Id={Id}");

            if (response.IsSuccessStatusCode)
            {
                // Read the response content as a string
                var apiResponse = await response.Content.ReadAsStringAsync();

                // Deserialize the JSON response to a model
                var data = JsonConvert.DeserializeObject<ProductData>(apiResponse);
                ViewModelProduct viewModelProduct = new ViewModelProduct();
                viewModelProduct.Id = data.ProductId;
                viewModelProduct.Name = data.ProductName;
                viewModelProduct.ShortDescription = data.ShortDescription;
                viewModelProduct.Status = data.Status;
                viewModelProduct.Categoryy = data.Category;
                viewModelProduct.Title = data.Titles;
                viewModelProduct.Heading = data.Headings;
                viewModelProduct.Description = data.Descriptions;
                viewModelProduct.CategoryId = data.CategoryId;
                viewModelProduct.SubCategoriess = data.Subcategories;
                viewModelProduct.SubCategoriessId = data.SubcategoriesId;
                viewModelProduct.Content = data.Content;
                viewModelProduct.PictureName = data.PictureName;
                int i = 0;
                foreach (var item in data.PDFDetails)
                {
                    viewModelProduct.PdfHeading.Add(item.PDFHeading);
                    viewModelProduct.PdfName.Add(item.PDFName);
                    viewModelProduct.PdfId.Add(item.Id);
                   
                        var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "pdfuploads", item.PDFName);

                    // Check if the file exists
                    if (System.IO.File.Exists(filePath))
                    {
                        // Read the file into a byte array
                        byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);

                        // Create a MemoryStream from the byte array
                        using (MemoryStream memoryStream = new MemoryStream(fileBytes))
                        {
                            // Create a FormFile from the MemoryStream
                            var formFile = new FormFile(memoryStream, 0, memoryStream.Length, null, item.PDFName)
                            {
                                Headers = new HeaderDictionary(),
                                ContentType = "application/pdf", // Set the content type as needed
                            };

                            // Add the FormFile to the PdfFiles list
                            viewModelProduct.FormFiless.Add(new ImageClass
                            {
                                Id = viewModelProduct.PdfId[i], // Set the Id as needed
                                PdfHeading = viewModelProduct.PdfHeading[i],
                                formFile = formFile // You may need to handle this based on your requirements

                            });
                            i++;
                        }
                    }

                    else
                    {
                        // Handle the case where the file does not exist
                    }

                   
                }

                return View(viewModelProduct);
            }
            return RedirectToAction("Product");
        }



        [HttpPost]
        public async Task<IActionResult> GetProduct([FromForm] ViewModelProduct model, [FromForm] IFormCollection formCollection)
        {

            var client = _httpClientFactory.CreateClient("YourApiClient");
            if(formCollection.Files.Count == 1)
            {
                if (formCollection.Files[0].ContentType == "image/jpeg")
                {

                }
                if (formCollection.Files[0].ContentType == "application/pdf")
                {
                    foreach (var file in formCollection.Files)
                    {
                        if (file.ContentType == "application/pdf")
                        {
                            // Generate a unique filename (you can use your own logic for this)
                            var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;

                            // Combine the unique filename with the wwwroot path
                            var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "pdfuploads", uniqueFileName);

                            // Save the file to the wwwroot folder
                            using (var fileStream = new FileStream(filePath, FileMode.Create))
                            {
                                file.CopyTo(fileStream);
                            }

                            model.PdfName.Add(uniqueFileName);
                        }

                    }
                }
            }

            if (formCollection.Files.Count > 1)
            {

                if (formCollection.Files[0].ContentType == "application/pdf" || formCollection.Files[1].ContentType == "application/pdf"||formCollection.Files.First().ContentType== "application/pdf")
                {
                    foreach (var file in formCollection.Files)
                    {
                        if (file.ContentType == "application/pdf")
                        {
                            // Generate a unique filename (you can use your own logic for this)
                            var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;

                            // Combine the unique filename with the wwwroot path
                            var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "pdfuploads", uniqueFileName);

                            // Save the file to the wwwroot folder
                            using (var fileStream = new FileStream(filePath, FileMode.Create))
                            {
                                file.CopyTo(fileStream);
                            }

                            model.PdfName.Add(uniqueFileName);
                        }

                    }
                }
                else
                {

                }
            }
            


            var response1 = await client.GetAsync($"PdfDetailsByProduct?Id={model.Id}");

            if (response1.IsSuccessStatusCode)
            {
                // Read the response content as a string
                var apiResponse = await response1.Content.ReadAsStringAsync();

                // Deserialize the JSON response to a model
                var data = JsonConvert.DeserializeObject<List<PdfDetails>>(apiResponse);
                bool flag = false;
                foreach (var item in data)
                {

                    // Check if model.PdfId is not null and not empty
                    if (model.PdfId.Count != 0)
                    {

                    
                   
                        foreach (var pdfid in model.PdfId)
                        {
                            if (item.Id != pdfid)
                            {
                                flag = true;
                                continue;
                            }
                            else
                            {
                                flag = false;
                                break;
                            }
                        }
                    }
                    else
                    {
                        model.deletePdfName.Add(item.PDFName);
                        // Add all item.Id to deletePdfId when model.PdfId is null or empty
                        model.deletePdfId.Add(item.Id);
                    }

                    if (flag)
                    {
                        model.deletePdfName.Add(item.PDFName);
                        // Add all item.Id to deletePdfId when model.PdfId is null or empty
                        model.deletePdfId.Add(item.Id);
                    }
                        
                   
                }

            }



            if (model.Id != 0 || ModelState.IsValid)
            {
                ProductData productData = new ProductData();
                int headingCount = model.PdfHeading.Count();
                int newNameCount = model.PdfName.Count();
                int HeadingNewIndex = headingCount - newNameCount;
                for (int i = 0; i < model.PdfName.Count; i++)
                {


                    // Create a new PdfDetails object
                    var pdfDetail = new PdfDetails
                    {

                        PDFHeading = model.PdfHeading[HeadingNewIndex],
                        PDFName = model.PdfName[i]
                    };
                    HeadingNewIndex++;
                    // Add the PdfDetails object to the PDFDetails list
                    productData.PDFDetails.Add(pdfDetail);
                }


                if (model.deletePdfName != null)
                {

                    foreach (var pdfName in model.deletePdfName)
                    {
                        // Construct the full file path
                        var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "pdfuploads", pdfName);

                        // Check if the file exists before attempting to delete
                        if (System.IO.File.Exists(filePath))
                        {
                            // Delete the file
                            System.IO.File.Delete(filePath);
                        }
                    }
                }




                if (model.Picture != null)
                {
                    if (!string.IsNullOrEmpty(model.PictureName))
                    {
                        // Delete the previous picture
                        string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "imagesuploads", model.PictureName);
                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }
                    }
                }
                // Save the new picture
                if (model.Picture != null)
                {
                    // Generate a unique file name for the new picture
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(model.Picture.FileName);

                    // Combine the path to wwwroot/images with the unique file name
                    string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "imagesuploads", uniqueFileName);

                    // Save the file to the specified path
                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        await model.Picture.CopyToAsync(stream);
                    }

                    // Update the model with the new picture name
                    model.PictureName = uniqueFileName;
                }
                productData.deletePdfId = model.deletePdfId;
                productData.ProductName = model.Name;
                productData.ProductId = model.Id;
                productData.ShortDescription = model.ShortDescription;
                productData.Status = model.Status;
                productData.Titles = model.Title;
                productData.Headings = model.Heading;
                productData.CategoryId = Convert.ToInt16(model.Categoryy);
                productData.Descriptions = model.Description;
                productData.Subcategories = model.SubCategoriess;
                productData.PictureName = model.PictureName;
                productData.Content = model.Content;

                var jsonContent = new StringContent(JsonConvert.SerializeObject(productData), Encoding.UTF8, "application/json");
                var response = await client.PostAsync("PostEditProduct1", jsonContent);
                if (response.IsSuccessStatusCode)
                {
                    TempData["message"] = "<script>alert('Product Updated Successfully')</script>";
                    return RedirectToAction("Product");
                }
            }
            return View();
        }

    }
}

