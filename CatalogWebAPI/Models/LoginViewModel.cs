using System.ComponentModel.DataAnnotations;

namespace CatalogWebAPI.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Введите логин.")]
        [StringLength(50)]
        public string Username { get; set; } = "";

        [Required(ErrorMessage = "Введите пароль.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = "";
    }
}

