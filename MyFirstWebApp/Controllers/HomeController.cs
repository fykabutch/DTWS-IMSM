using Microsoft.AspNetCore.Mvc;
using MyFirstWebApp.Data;
using MyFirstWebApp.Models;
using System.Diagnostics;

namespace MyFirstWebApp.Controllers;

public class HomeController : Controller
{
    private readonly MyApplicationContext _context;

    public HomeController(MyApplicationContext context)
    {
        _context = context;
    }

    public IActionResult Index(string? search)
    {
        var specialties = _context.Specialties.OrderBy(x => x.Id).ToList();

        if (!string.IsNullOrWhiteSpace(search))
        {
            search = search.Trim();
            specialties = specialties.FindAll(x =>
                x.EducationalProgram.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                x.FieldOfStudy.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                x.ScientificField.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                x.EducationLevel.Contains(search, StringComparison.OrdinalIgnoreCase));
        }

        ViewBag.Search = search;
        return View(specialties);
    }

    public IActionResult Add()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Add(Specialty specialty)
    {
        if (!ModelState.IsValid)
        {
            return View(specialty);
        }

        specialty.Id = 0;
        _context.Specialties.Add(specialty);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }

    public IActionResult Edit(int id)
    {
        var specialty = _context.Specialties.Find(id);
        if (specialty == null)
        {
            return NotFound();
        }

        return View(specialty);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Specialty specialty)
    {
        var record = _context.Specialties.Find(specialty.Id);
        if (record == null)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return View(specialty);
        }

        record.EducationalProgram = specialty.EducationalProgram;
        record.FieldOfStudy = specialty.FieldOfStudy;
        record.ScientificField = specialty.ScientificField;
        record.EducationLevel = specialty.EducationLevel;
        _context.SaveChanges();
        return RedirectToAction("Index");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int id)
    {
        var specialty = _context.Specialties.Find(id);
        if (specialty != null)
        {
            _context.Specialties.Remove(specialty);
            _context.SaveChanges();
        }

        return RedirectToAction("Index");
    }

    public IActionResult Error()
    {
        return View(new ErrorViewModel
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
        });
    }
}
