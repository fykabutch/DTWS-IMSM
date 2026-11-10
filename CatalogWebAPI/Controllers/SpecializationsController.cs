using CatalogWebAPI.Data;
using CatalogWebAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CatalogWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SpecializationsController : ControllerBase
    {
        private readonly MyApplicationContext _context;

        public SpecializationsController(MyApplicationContext context)
        {
            _context = context;
        }

        [HttpGet]
        public List<SpecializationGet> Get()
        {
            return _context.Specializations
                .Select(x => new SpecializationGet
                {
                    Id = x.Id,
                    Name = x.Name,
                    SpecialtyId = x.SpecialtyId,
                    SpecialtyName = x.Specialty!.Name
                })
                .ToList();
        }

        [HttpGet("{id}")]
        public SpecializationGet? Get(int id)
        {
            var specialization = _context.Specializations
                .Select(x => new SpecializationGet
                {
                    Id = x.Id,
                    Name = x.Name,
                    SpecialtyId = x.SpecialtyId,
                    SpecialtyName = x.Specialty!.Name
                })
                .FirstOrDefault(x => x.Id == id);

            return specialization;
        }

        [HttpPost]
        public List<SpecializationGet> Post([FromQuery] string? search, int? specialtyId)
        {
            var items = Get();
            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();
                items = items.FindAll(x => x.Name.Contains(search, StringComparison.OrdinalIgnoreCase));
            }
            if (specialtyId != null)
            {
                items = items.FindAll(x => x.SpecialtyId == specialtyId);
            }
            return items;
        }

        [HttpPut]
        public ActionResult<Specialization> Put(SpecializationAdd request)
        {
            if (!_context.Specialties.Any(x => x.Id == request.SpecialtyId))
                return BadRequest("Выберите существующую запись из списка.");
            var specialization = new Specialization
            {
                Name = request.Name,
                SpecialtyId = request.SpecialtyId
            };
            _context.Specializations.Add(specialization);
            _context.SaveChanges();
            return specialization;
        }

        [HttpDelete]
        public IActionResult Delete(int specializationId)
        {
            var specialization = _context.Specializations.FirstOrDefault(x => x.Id == specializationId);
            if (specialization != null)
            {
                _context.Specializations.Remove(specialization);
                _context.SaveChanges();
                return Ok();
            }
            return NotFound();
        }

        [HttpPatch]
        public IActionResult Patch(SpecializationEdit model)
        {
            var specialization = _context.Specializations.FirstOrDefault(x => x.Id == model.Id);
            if (specialization != null)
            {
                if (!_context.Specialties.Any(x => x.Id == model.SpecialtyId))
                    return BadRequest("Выберите существующую запись из списка.");
                specialization.Name = model.Name;
                specialization.SpecialtyId = model.SpecialtyId;
                _context.SaveChanges();
                return Ok();
            }
            return NotFound();
        }
    }
}


