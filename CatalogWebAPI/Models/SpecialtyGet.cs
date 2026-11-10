using System.ComponentModel.DataAnnotations;

namespace CatalogWebAPI.Models
{
    public class SpecialtyGet
    {
        public int Id { get; set; }

        public string Name { get; set; } = "";

        public string Code { get; set; } = "";

        public int DirectionId { get; set; }

        public string DirectionName { get; set; } = "";
    }
}

