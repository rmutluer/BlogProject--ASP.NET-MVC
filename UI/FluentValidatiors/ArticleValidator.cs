using ENTITIES.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UI.FluentValidatiors
{
    public class ArticleValidator : AbstractValidator<Article>
    {
        public ArticleValidator()
        {
            RuleFor(x => x.Title)
              .NotEmpty()
              .WithName("Makale Başlığı")
              .WithMessage("{PropertyName}  boş bırakılamaz.")
              .Length(2,50)
              .WithName("Makale Başlığı")
              .WithMessage("{PropertyName}  {MinLength} - {MaxLength} aralığında olmak zorundadır.");

            RuleFor(x => x.Category)
                .NotEmpty()
                .WithName("Kategori")
              .WithMessage("{PropertyName}  boş bırakılamaz.")
                .Length(2, 15)
                .WithName("Kategori")
                .WithMessage("{PropertyName}  {MinLength} - {MaxLength} aralığında olmak zorundadır.");

            RuleFor(x => x.Content)
                .NotEmpty()
                .WithName("Makale İçeriği")
                .WithMessage("{PropertyName}  boş bırakılamaz.");
                
           


           
        }
    }
}
