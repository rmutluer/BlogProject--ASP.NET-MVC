using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UI.Models
{
    public class SignUpViewModel
    {
        [Display(Name = "Mail")]
        [Required(ErrorMessage = "E-posta alanı zorunlu")]
        public string Email { get; set; }
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$", ErrorMessage = "Şifre kriterlere uymuyor. ( ℹ️ )")]
        [Required(ErrorMessage = "Şifre alanı zorunlu")]
        [Display(Name = "Şifre")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Bu alan zorunlu!")]
        [Compare("Password", ErrorMessage = "Şifreler eşleşmiyor!")]// ilk şifreyle karşılaştırır!
        [Display(Name = "Şifre(Tekrar)")]
        public string SecondPassword { get; set; }
    }
}
