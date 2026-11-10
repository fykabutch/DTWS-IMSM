using CatalogClientApp.Data;
using CatalogClientApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;

namespace CatalogClientApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            //          curl - X 'GET' \
            //'http://localhost:5024/Catalog' \
            //-H 'accept: text/plain'
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync("http://localhost:5024/Catalog");
            response.EnsureSuccessStatusCode();
            var responseText = await response.Content.ReadAsStringAsync();
            var jsonOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            var responseData = JsonSerializer.Deserialize<List<Specialty>>(responseText, jsonOptions);

            return View(responseData);
        }
        [HttpGet]
        public IActionResult Add()
        {
            return View(new SpecialtyAddDto());
        }

        [HttpPost]
        public async Task<IActionResult> Add(SpecialtyAddDto request)
        {
            var httpClient = new HttpClient();
            var response = await httpClient.PutAsJsonAsync("http://localhost:5024/Catalog", request);

            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
          //          curl - X 'DELETE' \
          //'http://localhost:5024/Catalog?specialtyId=5' \
          //-H 'accept: */*'
            var httpClient = new HttpClient();
            var response = await httpClient.DeleteAsync($"http://localhost:5024/Catalog?specialtyId={id}");

            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            //          curl - X 'GET' \
            //'http://localhost:5024/Catalog/1' \
            //-H 'accept: text/plain'
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync($"http://localhost:5024/Catalog/{id}");
            var responseText = await response.Content.ReadAsStringAsync();
            var jsonOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            var responseData = JsonSerializer.Deserialize<Specialty>(responseText, jsonOptions);

            return View(responseData);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(SpecialtyAddDto request)
        {
            //        curl -X 'PATCH' \
            //  'http://localhost:5024/Catalog' \
            //  -H 'accept: */*' \
            //  -H 'Content-Type: application/json' \
            //  -d '{
            //  "id": 110,
            //  "educationalProgram": "string",
            //  "fieldOfStudy": "string",
            //  "scientificField": "string",
            //  "educationLevel": "string"
            //}'
            var httpClient = new HttpClient();
            var response = await httpClient.PatchAsJsonAsync("http://localhost:5024/Catalog", request);
            return RedirectToAction(nameof("Index"))
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


