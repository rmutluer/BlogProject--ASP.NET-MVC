using ENTITIES.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES.Entities
{
    public class Article : IBaseEntity
    {
        [Key]
        public int ArticleID { get; set; }
        [Display(Name = "Makale Başlığı")]
        public string Title { get; set; }
        [Display(Name = "Makale İçeriği")]
        public string Content { get; set; }
        [Display(Name = "Kategori")]
        public string Category { get; set; } // tekrar düzenlenebilir!!!
        [Display(Name = "Cevaplanma Tarihi")]
        public DateTime? DateOfReply { get; set; }
        [Display(Name = "Oluşturulma Tarihi")]
        public DateTime CreateDate { get; set; } = DateTime.Now;
        [Display(Name = "Güncellenme Tarihi")]
        public DateTime? UpdateDate { get; set; }
        [Display(Name = "Silinme Tarihi")]
        public DateTime? DeleteDate { get; set; }
        [NotMapped]//veri tabanına kaydetmez
        [Display(Name = "Fotoğraf")]
        public IFormFile PhotoFile { get; set; }
        public string PhotoPath { get; set; }
        [Display(Name = "Onay Durumu")]
        public ConfirmationStatus ConfirmationStatus { get; set; }
        [Display(Name = "Durum")]
        public Status Status { get; set; } = Status.Active;
        [ForeignKey("AppUser")]
        public int AuthorId { get; set; }
        public AppUser AppUser { get; set; }

    }
}
