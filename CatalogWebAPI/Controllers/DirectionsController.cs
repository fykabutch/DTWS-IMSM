using CatalogWebAPI.Data;
using CatalogWebAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CatalogWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DirectionsController : ControllerBase
    {
        private readonly MyApplicationContext _context;

        public DirectionsController(MyApplicationContext context)
        {
            _context = context;
        }

        [HttpGet]
        public List<DirectionGet> Get()
        {
            return _context.Directions
                .Select(x => new DirectionGet
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .ToList();
        }

        [HttpGet("{id}")]
        public DirectionGet? Get(int id)
        {
            var direction = _context.Directions
                .Select(x => new DirectionGet
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .FirstOrDefault(x => x.Id == id);

            return direction;
        }

        [HttpPost]
        public List<DirectionGet> Post([FromQuery] string? search)
        {
            var items = Get();
            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();
                items = items.FindAll(x => x.Name.Contains(search, StringComparison.OrdinalIgnoreCase));
            }
            return items;
        }

        [HttpPut]
        public Direction Put(DirectionAdd request)
        {
            var direction = new Direction
            {
                Name = request.Name
            };
            _context.Directions.Add(direction);
            _context.SaveChanges();
            return direction;
        }

        [HttpDelete]
        public IActionResult Delete(int directionId)
        {
            var direction = _context.Directions.FirstOrDefault(x => x.Id == directionId);
            if (direction != null)
            {
                if (_context.Specialties.Any(x => x.DirectionId == directionId))
                    return BadRequest("Сначала удалите или перенесите связанные записи.");
                _context.Directions.Remove(direction);
                _context.SaveChanges();
                return Ok();
            }
            return NotFound();
        }

        [HttpPatch]
        public IActionResult Patch(DirectionEdit model)
        {
            var direction = _context.Directions.FirstOrDefault(x => x.Id == model.Id);
            if (direction != null)
            {
                direction.Name = model.Name;
                _context.SaveChanges();
                return Ok();
            }
            return NotFound();
        }
    }
}


