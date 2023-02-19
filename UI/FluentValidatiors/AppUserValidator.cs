using ENTITIES.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace UI.FluentValidatiors
{
    public class AppUserValidator:AbstractValidator<AppUser>
    {
        public AppUserValidator()
        {

            RuleFor(x => x.FirstName)
               .NotEmpty()
               .WithName("Adı")
               .WithMessage("{PropertyName} alanı boş bırakılamaz.")
               .Length(2, 15)
               .WithName("Adı")
               .WithMessage("{PropertyName}  {MinLength} - {MaxLength} aralığında olmak zorundadır.")
               .Matches(new Regex(@"^[a-zA-ZıüöçşğİÜÖÇŞĞ]+$"))
               .WithMessage("lütfen Sadece harf girişi yapınız");

            RuleFor(x => x.SecondName)
                .Length(2, 15)
                .WithName("İkinci Adı")
                .WithMessage("{PropertyName}  {MinLength} - {MaxLength} aralığında olmak zorundadır.")
                .Matches(new Regex(@"^[a-zA-ZıüöçşğİÜÖÇŞĞ]+$"))
               .WithMessage("lütfen Sadece harf girişi yapınız");

            RuleFor(x => x.LastName)
                .NotEmpty()
                .WithName("Soyadı")
                .WithMessage("{PropertyName} alanı boş bırakılamaz.")
                .Length(2, 15)
                .WithName("Soyadı")
                .WithMessage("{PropertyName}  {MinLength} - {MaxLength} aralığında olmak zorundadır.")
                .Matches(new Regex(@"^[a-zA-ZıüöçşğİÜÖÇŞĞ]+$"))
                .WithMessage("lütfen Sadece harf girişi yapınız");

            RuleFor(x => x.SecondLastName)
                .Length(2, 15)
                .WithName("İkinci Soyadı")
                .WithMessage("{PropertyName}  {MinLength} - {MaxLength} aralığında olmak zorundadır.")
                .Matches(new Regex(@"^[a-zA-ZıüöçşğİÜÖÇŞĞ]+$"))
               .WithMessage("lütfen Sadece harf girişi yapınız");

            RuleFor(x => x.BirthDate)
                .NotEmpty()
                .WithName("Doğum Tarihi")
                .WithMessage("{PropertyName} alanı boş bırakılamaz.")
                .Custom((date, context) =>
                {
                    DateTime from = new DateTime(1940, 1, 1);
                    DateTime to = DateTime.Now.AddYears(-12);
                    if (date < from)
                        context.AddFailure($"Doğum Tarihi {from.Date} tarihinden sonra olmalı");
                    else if (date > to)
                        context.AddFailure($"Doğum Tarihi {to.Day}.{to.Month}.{to.Year} tarihinden önce olmalı");
                });

            RuleFor(x => x.PhoneNumber)
                .NotEmpty()
                 .WithName("Telefon Numarası")
                .WithMessage("{PropertyName} boş bırakılamaz.")
                .Matches(new Regex(@"^(\+\d{1,2}\s)?\(?\d{3}\)?[\s.-]?\d{3}[\s.-]?\d{4}$"))
                .WithMessage("Lütfen geçerli bir telefon numarası giriniz.");

            RuleFor(x => x.CitizenId)
                .NotEmpty()
                .WithName("T.C. Kimlik Numarası")
                .WithMessage("{PropertyName} boş bırakılamaz.")
                .Length(11)
                .WithName("T.C. Kimlik Numarası")
                .WithMessage("{PropertyName}'nı 11 haneden oluşmalıdır. Lütfen tekrar giriniz.")
                .Matches(new Regex(@"^[1-9]{1}[0-9]{9}[02468]{1}$"))
                .WithMessage("Hatalı bir {PropertyName} girdiniz. Lütfen tekrar giriniz.");


            RuleFor(x => x.Job)
                .NotEmpty()
                .WithName("Meslek")
                .WithMessage("{PropertyName} alanı boş bırakılamaz.")
                  .Matches(new Regex(@"^[a-zA-ZıüöçşğİÜÖÇŞĞ]+$"))
                .WithMessage("lütfen Sadece harf girişi yapınız");

            RuleFor(x => x.Gender)
                .IsInEnum()
                .WithName("Cinsiyet")
                .WithMessage("{PropertyName} alanı boş bırakılamaz.");
                

            RuleFor(x => x.PhotoFile)
                .NotEmpty()
                .WithName("Fotoğraf")
                .WithMessage("{PropertyName} alanı boş bırakılamaz.");
            RuleFor(x => x.Country)
             .NotEmpty()
             .WithName("Uyruk")
             .WithMessage("{PropertyName} alanı boş bırakılamaz.")
               .Matches(new Regex(@"^[a-zA-ZıüöçşğİÜÖÇŞĞ]+$"))
               .WithMessage("lütfen Sadece harf girişi yapınız");

        }
    }
}
