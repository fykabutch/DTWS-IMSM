using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
namespace CatalogWebAPI.Data;

public class Direction
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Заполните поле «Название направления».")]
    [StringLength(200)]
    [Display(Name = "Название направления")]
    public string Name { get; set; } = "";

}

