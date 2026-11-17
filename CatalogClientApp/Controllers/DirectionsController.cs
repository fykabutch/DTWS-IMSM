using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using System.Net.Http.Headers;
using CatalogClientApp.Data;
using CatalogClientApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CatalogClientApp.Controllers
{
    [Authorize]
    public class DirectionsController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index(string? search)
        {
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await HttpContext.GetTokenAsync("access_token"));
            HttpResponseMessage response;
            if (string.IsNullOrWhiteSpace(search))
            {
                response = await httpClient.GetAsync("http://localhost:5024/api/Directions");
            }
            else
            {
                response = await httpClient.PostAsync(
                    "http://localhost:5024/api/Directions?search=" + Uri.EscapeDataString(search ?? ""), null);
            }
            response.EnsureSuccessStatusCode();
            var responseText = await response.Content.ReadAsStringAsync();
            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var responseData = JsonSerializer.Deserialize<List<Direction>>(responseText, jsonOptions);
            ViewBag.Search = search;
            return View(responseData ?? new List<Direction>());
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Add()
        {
            return View(new DirectionAddDto());
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Add(DirectionAddDto request)
        {
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await HttpContext.GetTokenAsync("access_token"));
            var response = await httpClient.PutAsJsonAsync("http://localhost:5024/api/Directions", request);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await HttpContext.GetTokenAsync("access_token"));
            var response = await httpClient.GetAsync("http://localhost:5024/api/Directions/" + id);
            response.EnsureSuccessStatusCode();
            var responseText = await response.Content.ReadAsStringAsync();
            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var responseData = JsonSerializer.Deserialize<DirectionEditDto>(responseText, jsonOptions);
            return View(responseData);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(DirectionEditDto request)
        {
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await HttpContext.GetTokenAsync("access_token"));
            var response = await httpClient.PatchAsJsonAsync("http://localhost:5024/api/Directions", request);
            return RedirectToAction("Index");
        }

        [HttpPost]

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await HttpContext.GetTokenAsync("access_token"));
            var response = await httpClient.DeleteAsync("http://localhost:5024/api/Directions?directionId=" + id);
            return RedirectToAction("Index");
        }
    }
}



