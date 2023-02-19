using BUSINESS.Abstract;
using ENTITIES.Entities;
using ENTITIES.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using UI.Models;

namespace UI.Controllers
{


    public class GuestController : Controller
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly IGenericService<Article> _articleService;
        private readonly IEmailSender _emailSender;
        private readonly IWebHostEnvironment _env;

        public GuestController(UserManager<AppUser> userManager, IGenericService<Article> articleService, IEmailSender emailSender, IWebHostEnvironment env)
        {
            _userManager = userManager;
            _articleService = articleService;
            _emailSender = emailSender;
            _env = env;
        }
        [AllowAnonymous]
        public IActionResult HomePage()
        {
            var categories = _articleService.GetAll().GroupBy(p => p.Category).Select(g => new CategoryVM { Category = g.Key, CategoryCount = g.Count() }).OrderByDescending(c => c.CategoryCount).Take(5).ToList();
            return View(categories);
        }
        public IActionResult ArticleDetails(int id)
        {
            TempData["ArticleId"] = id;
            return View(_articleService.GetById(id));
        }
        [AllowAnonymous]
        public IActionResult SearchArticles(string search)
        {

            if (search != null)
            {
                List<Article> Articles = _articleService.GetDefault(x => x.Title.Contains(search) || x.Content.Contains(search) || x.Category.Contains(search) && x.Status == Status.Active).OrderByDescending(x => x.CreateDate).ToList();
                //var categories = _articleService.GetAll().GroupBy(p => p.Category).Select(g => new CategoryArticlesVM { Category = g.Key, CategoryCount = g.Count(), Articles=Articles}).OrderByDescending(c => c.CategoryCount).Take(5).ToList();
                var categoriesFive = _articleService.GetAll().GroupBy(p => p.Category).Select(g => new CategoryVM { Category = g.Key, CategoryCount = g.Count() }).OrderByDescending(c => c.CategoryCount).Take(5).ToList();
                ViewBag.categoriesFive = categoriesFive;
                ViewBag.articles = Articles;
                return View();
            }
            else
            {
                return RedirectToAction("HomePage");
            }
        }
        [HttpPost]
        public async Task<IActionResult> SendCommentMail(int id, string name, string subject, string message, string email)
        {
            AppUser author = await _userManager.FindByIdAsync(id.ToString());
            var articleId = TempData["ArticleId"];
            if (email == null)
            {
                TempData["Message2"] = "Bu alan boş geçilemez!";
                return RedirectToAction("ArticleDetails", new { id = articleId });
            }
            else
            {


                //Mail içeriği
                string content = "";

                //Çalışma ortamına göre mailin içeriği belirleniyor
                if (_env.EnvironmentName == "Development")
                {

                    content = $"Merhaba {author.GetFullName()} , <br />" +
                       $"Mesaj: { message}  <br />" +
                       $"Gönderen: {name} <br />" +
                       $"Email: {email} <br />" +
                       $"<b> İyi çalışmalar dileriz... <b/> <br /> <br />" +
                       $"DEVHELL";

                }
                else if (_env.EnvironmentName == "Production")
                {
                    content = $"Merhaba {author.GetFullName()} , <br />" +
                         $"Mesaj: { message}  <br />" +
                         $"Gönderen: {name} <br />" +
                         $"<b>Email:</b> {email} <br />" +
                         $"<b> İyi çalışmalar dileriz... </b> <br /> <br />" +
                         $"DEVHELL";
                }
                await _emailSender.SendEmailAsync(author.Email, subject, content); //Mail gönderiliyor
                TempData["Message"] = "Mesajınız Gönderildi";

                return RedirectToAction("ArticleDetails", new { id = articleId });
            }
        }
        public IActionResult DevhellTeam()
        {
            return View();
        }
        public IActionResult DevhellAbout()
        {
            return View();
        }
    }
}
