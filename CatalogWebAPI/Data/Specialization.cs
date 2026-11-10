using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
namespace CatalogWebAPI.Data;

public class Specialization
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Заполните поле «Название профиля».")]
    [StringLength(200)]
    [Display(Name = "Название профиля")]
    public string Name { get; set; } = "";

    [Range(1, int.MaxValue, ErrorMessage = "Выберите значение из списка.")]
    [Display(Name = "Специальность")]
    public int SpecialtyId { get; set; }
    [JsonIgnore]
    public Specialty? Specialty { get; set; }
}

