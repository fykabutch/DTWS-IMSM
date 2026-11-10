using System.ComponentModel.DataAnnotations;

namespace CatalogClientApp.Models;

public class SpecialtyEditDto
{

    public int Id { get; set; }

    [Required(ErrorMessage = "Заполните поле «Наименование».")]
    [StringLength(200)]
    [Display(Name = "Наименование")]
    public string Name { get; set; } = "";

    [Required(ErrorMessage = "Заполните поле «Код специальности».")]
    [StringLength(200)]
    [Display(Name = "Код специальности")]
    public string Code { get; set; } = "";

    [Range(1, int.MaxValue, ErrorMessage = "Выберите значение из списка.")]
    [Display(Name = "Направление подготовки")]
    public int DirectionId { get; set; }

}


