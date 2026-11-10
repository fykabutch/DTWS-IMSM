using CatalogClientApp.Data;
using CatalogClientApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Json;

namespace CatalogClientApp.Controllers
{
    public class SpecializationsController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index(string? search, int? parentId)
        {
            using var httpClient = new HttpClient();
            HttpResponseMessage response;
            if (string.IsNullOrWhiteSpace(search) && parentId == null)
            {
                response = await httpClient.GetAsync("http://localhost:5024/api/Specializations");
            }
            else
            {
                response = await httpClient.PostAsync(
                    "http://localhost:5024/api/Specializations?search=" + Uri.EscapeDataString(search ?? "") + "&specialtyId=" + parentId, null);
            }
            response.EnsureSuccessStatusCode();
            var responseText = await response.Content.ReadAsStringAsync();
            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var responseData = JsonSerializer.Deserialize<List<Specialization>>(responseText, jsonOptions);
            ViewBag.Search = search;
            ViewBag.ParentId = parentId;
            var parents = await httpClient.GetFromJsonAsync<List<Specialty>>("http://localhost:5024/api/Specialties");
            ViewBag.Parents = new SelectList(parents, "Id", "Name");
            return View(responseData ?? new List<Specialization>());
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            using var httpClient = new HttpClient();
            var parents = await httpClient.GetFromJsonAsync<List<Specialty>>("http://localhost:5024/api/Specialties");
            ViewBag.Parents = new SelectList(parents, "Id", "Name");
            return View(new SpecializationAddDto());
        }

        [HttpPost]
        public async Task<IActionResult> Add(SpecializationAddDto request)
        {
            using var httpClient = new HttpClient();
            var response = await httpClient.PutAsJsonAsync("http://localhost:5024/api/Specializations", request);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync("http://localhost:5024/api/Specializations/" + id);
            response.EnsureSuccessStatusCode();
            var responseText = await response.Content.ReadAsStringAsync();
            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var responseData = JsonSerializer.Deserialize<SpecializationEditDto>(responseText, jsonOptions);
            var parents = await httpClient.GetFromJsonAsync<List<Specialty>>("http://localhost:5024/api/Specialties");
            ViewBag.Parents = new SelectList(parents, "Id", "Name");
            return View(responseData);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SpecializationEditDto request)
        {
            using var httpClient = new HttpClient();
            var response = await httpClient.PatchAsJsonAsync("http://localhost:5024/api/Specializations", request);
            return RedirectToAction("Index");
        }

        [HttpPost]

        public async Task<IActionResult> Delete(int id)
        {
            using var httpClient = new HttpClient();
            var response = await httpClient.DeleteAsync("http://localhost:5024/api/Specializations?specializationId=" + id);
            return RedirectToAction("Index");
        }
    }
}


