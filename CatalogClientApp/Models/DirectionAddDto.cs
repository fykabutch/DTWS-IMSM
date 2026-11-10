using System.ComponentModel.DataAnnotations;

namespace CatalogClientApp.Models;

public class DirectionAddDto
{



    [Required(ErrorMessage = "Заполните поле «Название направления».")]
    [StringLength(200)]
    [Display(Name = "Название направления")]
    public string Name { get; set; } = "";

}


