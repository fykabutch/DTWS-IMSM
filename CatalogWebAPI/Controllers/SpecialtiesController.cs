using CatalogWebAPI.Data;
using CatalogWebAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CatalogWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SpecialtiesController : ControllerBase
    {
        private readonly MyApplicationContext _context;

        public SpecialtiesController(MyApplicationContext context)
        {
            _context = context;
        }

        [HttpGet]
        public List<SpecialtyGet> Get()
        {
            return _context.Specialties
                .Select(x => new SpecialtyGet
                {
                    Id = x.Id,
                    Name = x.Name,
                    Code = x.Code,
                    DirectionId = x.DirectionId,
                    DirectionName = x.Direction!.Name
                })
                .ToList();
        }

        [HttpGet("{id}")]
        public SpecialtyGet? Get(int id)
        {
            var specialty = _context.Specialties
                .Select(x => new SpecialtyGet
                {
                    Id = x.Id,
                    Name = x.Name,
                    Code = x.Code,
                    DirectionId = x.DirectionId,
                    DirectionName = x.Direction!.Name
                })
                .FirstOrDefault(x => x.Id == id);

            return specialty;
        }

        [HttpPost]
        public List<SpecialtyGet> Post([FromQuery] string? search, int? directionId)
        {
            var items = Get();
            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();
                items = items.FindAll(x => x.Name.Contains(search, StringComparison.OrdinalIgnoreCase) || x.Code.Contains(search, StringComparison.OrdinalIgnoreCase));
            }
            if (directionId != null)
            {
                items = items.FindAll(x => x.DirectionId == directionId);
            }
            return items;
        }

        [HttpPut]
        public ActionResult<Specialty> Put(SpecialtyAdd request)
        {
            if (!_context.Directions.Any(x => x.Id == request.DirectionId))
                return BadRequest("Выберите существующую запись из списка.");
            var specialty = new Specialty
            {
                Name = request.Name,
                Code = request.Code,
                DirectionId = request.DirectionId
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
                if (_context.Specializations.Any(x => x.SpecialtyId == specialtyId))
                    return BadRequest("Сначала удалите или перенесите связанные записи.");
                _context.Specialties.Remove(specialty);
                _context.SaveChanges();
                return Ok();
            }
            return NotFound();
        }

        [HttpPatch]
        public IActionResult Patch(SpecialtyEdit model)
        {
            var specialty = _context.Specialties.FirstOrDefault(x => x.Id == model.Id);
            if (specialty != null)
            {
                if (!_context.Directions.Any(x => x.Id == model.DirectionId))
                    return BadRequest("Выберите существующую запись из списка.");
                specialty.Name = model.Name;
                specialty.Code = model.Code;
                specialty.DirectionId = model.DirectionId;
                _context.SaveChanges();
                return Ok();
            }
            return NotFound();
        }
    }
}


