using System.ComponentModel.DataAnnotations;

namespace CatalogWebAPI.Models
{
    public class DirectionAdd
    {

        [Required(ErrorMessage = "Заполните поле «Название направления».")]
        [StringLength(200)]
        public string Name { get; set; } = "";

    }
}

