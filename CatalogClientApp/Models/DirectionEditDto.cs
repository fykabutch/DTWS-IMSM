using System.ComponentModel.DataAnnotations;

namespace CatalogClientApp.Models;

public class DirectionEditDto
{

    public int Id { get; set; }

    [Required(ErrorMessage = "Заполните поле «Название направления».")]
    [StringLength(200)]
    [Display(Name = "Название направления")]
    public string Name { get; set; } = "";

}


