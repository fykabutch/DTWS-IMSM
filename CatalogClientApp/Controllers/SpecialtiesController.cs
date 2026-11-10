using CatalogClientApp.Data;
using CatalogClientApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Json;

namespace CatalogClientApp.Controllers
{
    public class SpecialtiesController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index(string? search, int? parentId)
        {
            using var httpClient = new HttpClient();
            HttpResponseMessage response;
            if (string.IsNullOrWhiteSpace(search) && parentId == null)
            {
                response = await httpClient.GetAsync("http://localhost:5024/api/Specialties");
            }
            else
            {
                response = await httpClient.PostAsync(
                    "http://localhost:5024/api/Specialties?search=" + Uri.EscapeDataString(search ?? "") + "&directionId=" + parentId, null);
            }
            response.EnsureSuccessStatusCode();
            var responseText = await response.Content.ReadAsStringAsync();
            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var responseData = JsonSerializer.Deserialize<List<Specialty>>(responseText, jsonOptions);
            ViewBag.Search = search;
            ViewBag.ParentId = parentId;
            var parents = await httpClient.GetFromJsonAsync<List<Direction>>("http://localhost:5024/api/Directions");
            ViewBag.Parents = new SelectList(parents, "Id", "Name");
            return View(responseData ?? new List<Specialty>());
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            using var httpClient = new HttpClient();
            var parents = await httpClient.GetFromJsonAsync<List<Direction>>("http://localhost:5024/api/Directions");
            ViewBag.Parents = new SelectList(parents, "Id", "Name");
            return View(new SpecialtyAddDto());
        }

        [HttpPost]
        public async Task<IActionResult> Add(SpecialtyAddDto request)
        {
            using var httpClient = new HttpClient();
            var response = await httpClient.PutAsJsonAsync("http://localhost:5024/api/Specialties", request);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync("http://localhost:5024/api/Specialties/" + id);
            response.EnsureSuccessStatusCode();
            var responseText = await response.Content.ReadAsStringAsync();
            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var responseData = JsonSerializer.Deserialize<SpecialtyEditDto>(responseText, jsonOptions);
            var parents = await httpClient.GetFromJsonAsync<List<Direction>>("http://localhost:5024/api/Directions");
            ViewBag.Parents = new SelectList(parents, "Id", "Name");
            return View(responseData);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SpecialtyEditDto request)
        {
            using var httpClient = new HttpClient();
            var response = await httpClient.PatchAsJsonAsync("http://localhost:5024/api/Specialties", request);
            return RedirectToAction("Index");
        }

        [HttpPost]

        public async Task<IActionResult> Delete(int id)
        {
            using var httpClient = new HttpClient();
            var response = await httpClient.DeleteAsync("http://localhost:5024/api/Specialties?specialtyId=" + id);
            return RedirectToAction("Index");
        }
    }
}


