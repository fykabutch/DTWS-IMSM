using System.ComponentModel.DataAnnotations;

namespace CatalogWebAPI.Models
{
    public class SpecialtyEdit
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Заполните поле «Наименование».")]
        [StringLength(200)]
        public string Name { get; set; } = "";

        [Required(ErrorMessage = "Заполните поле «Код специальности».")]
        [StringLength(200)]
        public string Code { get; set; } = "";

        [Range(1, int.MaxValue, ErrorMessage = "Выберите запись из списка.")]
        public int DirectionId { get; set; }

    }
}

