using System.ComponentModel.DataAnnotations;

namespace CatalogWebAPI.Models
{
    public class SpecializationEdit
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Заполните поле «Название профиля».")]
        [StringLength(200)]
        public string Name { get; set; } = "";

        [Range(1, int.MaxValue, ErrorMessage = "Выберите запись из списка.")]
        public int SpecialtyId { get; set; }

    }
}

