using CatalogWebAPI.Data;
using CatalogWebAPI.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CatalogWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CatalogController : ControllerBase
    {
        private readonly ILogger<CatalogController> _logger;
        private readonly MyApplicationContext _context;

        public CatalogController(ILogger<CatalogController> logger, MyApplicationContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public List<SpecialtyGet> Get()
        {
            return _context.Specialties
                .Select(x => new SpecialtyGet
                { 
                    Id = x.Id,
                    EducationalProgram = x.EducationalProgram,
                    FieldOfStudy = x.FieldOfStudy,
                    ScientificField = x.ScientificField
                })
                .ToList();

        }
        [HttpPost]
        public IEnumerable<Specialty> Post([FromQuery] string? search)
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
            return specialties;
        }
        [HttpPut]
        public Specialty Put(SpecialtyAdd request)
        {
            var specialty = new Specialty
            {
                EducationalProgram = request.EducationalProgram,
                FieldOfStudy = request.FieldOfStudy,
                ScientificField = request.ScientificField,
                EducationLevel = request.EducationLevel
            };
            _context.Specialties.Add(specialty);
            _context.SaveChanges();
            return specialty;
        }
        [HttpDelete]
        public IActionResult Delete(int specialtyId)
        {
            var specialty = _context.Specialties.FirstOrDefault(x => x.Id == specialtyId);
            if (specialty != null)
            {
                _context.Remove(specialty);
                _context.SaveChanges();
                return Ok();
            }
            return NotFound();
        }
        [HttpPatch]
        public IActionResult Patch(Specialty model)
        {
            var specialty = _context.Specialties.FirstOrDefault(x => x.Id == model.Id);
            if (specialty != null)
            {
                specialty.EducationalProgram = model.EducationalProgram;
                specialty.FieldOfStudy = model.FieldOfStudy;
                specialty.ScientificField = model.ScientificField;
                specialty.EducationLevel = model.EducationLevel;
                _context.Update(specialty);
                _context.SaveChanges();
                return Ok();
            }
            return NotFound();
        }
    }
}

