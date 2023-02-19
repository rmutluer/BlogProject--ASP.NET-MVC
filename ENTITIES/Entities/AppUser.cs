using ENTITIES.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES.Entities
{
    
    public class AppUser : IdentityUser<int>, IBaseEntity
    {
        //BaseEntityden gelenler
        [Display(Name = "Oluşturulma Tarihi")]
        public DateTime CreateDate { get; set; }
        [Display(Name = "Güncellenme Tarihi")]
        public DateTime? UpdateDate { get; set; }
        [Display(Name = "Silinme Tarihi")]
        public DateTime? DeleteDate { get; set; }
        [Display(Name = "Durum")]
        public Status Status { get; set; } = Status.Active;
        // ekstra eklenen property'ler
        [Display(Name ="Adı")]
        public string FirstName { get; set; }
        [Display(Name = "İkinci Adı")]
        public string SecondName { get; set; }
        [Display(Name = "Soyadı")]
        public string LastName { get; set; }
        [Display(Name = "İkinci Soyadı")]
        public string SecondLastName { get; set; }
        [Display(Name = "Cinsiyet")]
        public Gender Gender { get; set; }
        [Display(Name = "Doğum Tarihi")]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }
        [NotMapped]//veri tabanına kaydetmez
        [Display(Name = "Fotoğraf")]
        public IFormFile PhotoFile { get; set; }
        public string ProfilePhotoPath { get; set; }
        [Display(Name = "Telefon Numarası")]
        public override string PhoneNumber { get; set; }
        [Display(Name = "Takipçi Sayısı")]
        public int? Followers { get; set; }
        [Display(Name = "T.C. Kimlik Numarası")]
        public string CitizenId { get; set; }
        [Display(Name = "Uyruk")]
        public string Country { get; set; }
        [Display(Name = "Meslek")]
        public string Job { get; set; }
        public List<Article> Articles { get; set; }

        public string GetFullNameShort()
        {
           
            string lastNameShort = string.Empty;
            string nameShort = string.Empty;
            if (!string.IsNullOrEmpty(SecondName))
            {
                nameShort = SecondName[0].ToString() + ".";
            }
            if (!string.IsNullOrEmpty(SecondLastName))
            {
                lastNameShort = SecondLastName[0].ToString() + ".";
            }
            return string.Join(" ", FirstName, nameShort, LastName, lastNameShort);
        }
        public string GetFullName()
        {
            return string.Join(" ", FirstName, SecondName, LastName, SecondLastName);
        }


    }
}
