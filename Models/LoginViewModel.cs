using System.ComponentModel.DataAnnotations;

namespace BarberApp.Models
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "E-posta")]
        [EmailAddress]
        public string? UserName { get; set; }  // Burada "UserName" yerine "Email" kullanabilirsiniz

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Şifre")]
        public string? Password { get; set; }

        [Display(Name = "Beni Hatırla")]
        public bool RememberMe { get; set; }
    }
}
