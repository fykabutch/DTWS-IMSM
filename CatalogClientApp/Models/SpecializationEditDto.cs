using System.ComponentModel.DataAnnotations;

namespace CatalogClientApp.Models;

public class SpecializationEditDto
{

    public int Id { get; set; }

    [Required(ErrorMessage = "Заполните поле «Название профиля».")]
    [StringLength(200)]
    [Display(Name = "Название профиля")]
    public string Name { get; set; } = "";

    [Range(1, int.MaxValue, ErrorMessage = "Выберите значение из списка.")]
    [Display(Name = "Специальность")]
    public int SpecialtyId { get; set; }

}


