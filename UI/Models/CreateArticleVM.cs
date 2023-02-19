using ENTITIES.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UI.Models
{
    public class CreateArticleVM
    {
        [Display(Name = "Makale Başlığı")]
        public string Title { get; set; }
        [Display(Name = "Makale İçeriği")]
        public string Content { get; set; }
        [Display(Name = "Kategori")]
        public string Category { get; set; }
        [Display(Name = "Oluşturulma Tarihi")]
        public DateTime CreateDate { get; set; } = DateTime.Now;
        [Display(Name = "Fotoğraf")]
        public IFormFile PhotoFile { get; set; }
        public string PhotoPath { get; set; }
        [Display(Name = "Onay Durumu")]
        public ConfirmationStatus ConfirmationStatus { get; set; } = ConfirmationStatus.Pending;
        [Display(Name = "Durum")]
        public Status Status { get; set; } = Status.Active;
     
    }
}
