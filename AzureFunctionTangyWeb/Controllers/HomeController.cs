using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using AzureFunctionTangyWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Newtonsoft.Json;
using System.Diagnostics;

namespace AzureFunctionTangyWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly BlobServiceClient _blobService;

        public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory, BlobServiceClient blobService)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _blobService = blobService;
        }

        public IActionResult Index()
        {
            return View();
        }

        // http://localhost:7129/api/OnSalesUploadWriteToQueue

        [HttpPost]
        public async Task<IActionResult> Index(SalesRequest salesRequest, IFormFile file)
        {
            salesRequest.Id = Guid.NewGuid().ToString();
            using var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri("http://localhost:7129/api/");

            using (var content = new StringContent(JsonConvert.SerializeObject(salesRequest), System.Text.Encoding.UTF8, "application/json"))
            {
                HttpResponseMessage response = await client.PostAsync("OnSalesUploadWriteToQueue", content);
                string returnValue = await response.Content.ReadAsStringAsync();
            }

            if (file != null)
            {
                var fileName = salesRequest.Id + Path.GetExtension(file.FileName);
                BlobContainerClient containerClient = _blobService.GetBlobContainerClient("functionsalesrep");
                var blobClient = containerClient.GetBlobClient(fileName);

                var httpheaders = new BlobHttpHeaders()
                {
                    ContentType = file.ContentType
                };

                await blobClient.UploadAsync(file.OpenReadStream(), httpheaders);
            }


            return RedirectToAction(nameof(Index));
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
