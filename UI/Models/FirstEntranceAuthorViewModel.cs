using ENTITIES.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace UI.Models
{
    public class FirstEntranceAuthorViewModel// ilk defa giriş yapan user buradaki bilgileri doldurmak zorunda.
    {
        [Display(Name = "Adı")]
        public string FirstName { get; set; }
        [Display(Name = "İkinci Adı")]
        public string SecondName { get; set; }
        [Display(Name = "Soyadı")]
        public string LastName { get; set; }
        [Display(Name = "İkinci Soyadı")]
        public string SecondLastName { get; set; }
        [Display(Name = "Mail")]
        public string Email { get; set; }
        [Display(Name = "Cinsiyet")]
        public Gender Gender { get; set; }
        [Display(Name = "Doğum Tarihi")]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }
        [Display(Name = "Fotoğraf")]
        public IFormFile PhotoFile { get; set; }
        public string ProfilePhotoPath { get; set; }
        [Display(Name = "Telefon Numarası")]
        public  string PhoneNumber { get; set; }
        [Display(Name = "T.C. Kimlik Numarası")]
        public string CitizenId { get; set; }
        [Display(Name = "Uyruk")]
        public string Country { get; set; }
        [Display(Name = "Meslek")]
        public string Job { get; set; }
    }
}
