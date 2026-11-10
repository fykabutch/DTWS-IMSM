using System.ComponentModel.DataAnnotations;

namespace CatalogWebAPI.Models
{
    public class SpecializationGet
    {
        public int Id { get; set; }

        public string Name { get; set; } = "";

        public int SpecialtyId { get; set; }

        public string SpecialtyName { get; set; } = "";
    }
}

