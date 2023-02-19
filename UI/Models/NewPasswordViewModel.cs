using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UI.Models
{
    public class NewPasswordViewModel
    {
        [Display(Name = "Yeni Şifre")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$")]
        [Required]
        public string Password { get; set; }
        [Required]
        [Compare("Password")]// ilk şifreyle karşılaştırır!
        [Display(Name = "Yeni Şifre(tekrar)")]
        public string SecondPassword { get; set; }
        [Display(Name = "Şifre")]
        public string CurrentPassword { get; set; }
        public string Token { get; set; }
        [Display(Name = "Mail")]
        public string Email { get; set; }
    }
}
