using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UI.Models
{
    public class LoginViewModel
    {
        [Display(Name = "Mail")] 
        public string Email { get; set; }
              
        [Display(Name = "Şifre")]
        public string Password { get; set; }
    }
}
