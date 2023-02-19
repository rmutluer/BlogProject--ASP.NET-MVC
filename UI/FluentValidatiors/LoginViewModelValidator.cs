using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UI.Models;

namespace UI.FluentValidatiors
{
    public class LoginViewModelValidator : AbstractValidator<LoginViewModel>
    {
        public LoginViewModelValidator()
        {
            RuleFor(x => x.Email)
               .NotEmpty()
               .WithName("Email")
               .WithMessage("{PropertyName} alanı boş bırakılamaz.")
               .Matches(new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$"))
            .WithMessage("{PropertyName} formatında giriş yapınız.");
            RuleFor(x => x.Password)
              .NotEmpty()
              .WithName("Şifre")
              .WithMessage("{PropertyName} alanı boş bırakılamaz.")
              .Matches(new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$"))
              .WithMessage("{PropertyName} 8-15 karakter uzunluğunda, en az bir büyük harf bir rakam ve özel karekterden oluşmalıdır.");
        }
    }
}
