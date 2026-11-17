using System.ComponentModel.DataAnnotations;

namespace CatalogClientApp.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Введите логин.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Логин: от 3 до 50 символов.")]
        public string Username { get; set; } = "";

        [Required(ErrorMessage = "Введите пароль.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Пароль: от 6 до 100 символов.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = "";

        [Required(ErrorMessage = "Повторите пароль.")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; } = "";
    }
}

