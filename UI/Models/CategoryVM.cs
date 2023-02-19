using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UI.Models
{
    public class CategoryVM
    {
        [Display(Name = "Kategori")]
        public string Category { get; set; }
        [Display(Name = "Kategori Sayısı")]
        public int CategoryCount{ get; set; }
       
    }
}
