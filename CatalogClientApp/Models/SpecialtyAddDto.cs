using System.ComponentModel.DataAnnotations;

namespace CatalogClientApp.Models
{
    public class SpecialtyAddDto
    {
        [Required(ErrorMessage = "Введите образовательную программу.")]
        [StringLength(200)]
        [Display(Name = "Образовательная программа")]
        public string EducationalProgram { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введите направление подготовки.")]
        [StringLength(200)]
        [Display(Name = "Направление подготовки")]
        public string FieldOfStudy { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введите научную отрасль.")]
        [StringLength(200)]
        [Display(Name = "Научная отрасль")]
        public string ScientificField { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введите уровень образования.")]
        [StringLength(100)]
        [Display(Name = "Уровень образования")]
        public string EducationLevel { get; set; } = string.Empty;
    }
}
