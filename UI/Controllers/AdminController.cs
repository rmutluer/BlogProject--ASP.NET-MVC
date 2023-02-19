using BUSINESS.Abstract;
using BUSINESS.Concrete;
using ENTITIES.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using UI.Models;

namespace UI.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IGenericService<Article> _articleService;

        public AdminController(UserManager<AppUser> userManager, IGenericService<Article> articleService)
        {
            _userManager = userManager;
            _articleService = articleService;

        }

        public async Task<IActionResult> HomePage()
        {
            ArticlesUsersVM vm = new ArticlesUsersVM();
            var authorUsers = (IEnumerable<AppUser>)await _userManager.GetUsersInRoleAsync("author");
            vm.AppUsers = (List<AppUser>)authorUsers;
            vm.Articles = _articleService.GetAll().Where(x => x.Status != ENTITIES.Enums.Status.Passive).ToList();
            return View(vm);
        }

        public async Task<ActionResult> ArticleList(string articleName)
        {
            ArticlesUsersVM vm = new ArticlesUsersVM();
            var authorUsers = (IEnumerable<AppUser>)await _userManager.GetUsersInRoleAsync("author");
            if (articleName != null)
            {
                vm.Articles = _articleService.GetDefault(x => x.Title.Contains(articleName)); ;
                vm.AppUsers = (List<AppUser>)authorUsers;
                return View(vm);
            }
            else
            {
                vm.AppUsers = (List<AppUser>)authorUsers;
                vm.Articles = (_articleService.GetAll()?.OrderByDescending(x => x.ConfirmationStatus).ThenBy(x => x.CreateDate).Where(x => x.Status != ENTITIES.Enums.Status.Passive)).ToList();
                return View(vm);
            }


        }
        public ActionResult ArticleList2()
        {
            var getArticles = _articleService.GetAll()?.OrderByDescending(x => x.ConfirmationStatus).ThenBy(x => x.CreateDate).Where(x => x.Status != ENTITIES.Enums.Status.Passive);
            return View(getArticles);
        }
        public async Task<ActionResult> AuthorList()
        {

            var authorUsers = (IEnumerable<AppUser>)await _userManager.GetUsersInRoleAsync("author");

            return View(authorUsers);
        }
        public async Task<ActionResult> ArticleDetails(int id)
        {

            var getArticle = _articleService.GetById(id);

            return View(getArticle);
        }
        public async Task<ActionResult> AuthorDetails(int id)
        {

            var getAuthor = await _userManager.FindByIdAsync(id.ToString());

            return View(getAuthor);
        }
        public async Task<ActionResult> ArticleUpdate(int id)
        {

            var updateAuthor = _articleService.GetById(id);

            return View(updateAuthor);
        }
        [HttpPost]
        public async Task<ActionResult> ArticleUpdate(Article article)
        {

            if (ModelState.IsValid)
            {
                Article updateArticle = _articleService.GetById(article.ArticleID);
                updateArticle.Title = article.Title;
                updateArticle.Content = article.Content;
                updateArticle.Status = article.Status;
                updateArticle.ConfirmationStatus = article.ConfirmationStatus;
                updateArticle.UpdateDate = DateTime.Now;
                //Eğer güncelleme sırasında yeni fotoğrag yüklenmediyse eski fotoğraf korunur, yüklendiyse URL güncellenir
                //Static Class olarak oluşturulan resim kaydetme metodu kullanılıyor (Business/Concrete)
                string photoUrl = article.PhotoFile == null ? article.PhotoPath : await ImageSaver.SaveImage(article.PhotoFile);
                updateArticle.PhotoPath = photoUrl;
                _articleService.Update(updateArticle);
                return RedirectToAction("ArticleList2");
            }
            else
            {
                return View(article);//burası updateArticle olabilir bütün form eski haline gelmesin diye!
            }


        }
        public async Task<ActionResult> AuthorUpdate(int id)
        {

            var updateAuthor = await _userManager.FindByIdAsync(id.ToString());

            return View(updateAuthor);
        }
        [HttpPost]
        public async Task<ActionResult> AuthorUpdate(AppUser author)//loginden sonra burası düzenlenecek
        {
            if (ModelState.IsValid)
            {
                AppUser updateAuthor = await _userManager.FindByIdAsync(author.Id.ToString());//loginden sonra burası düzenlenecek!!!
                updateAuthor.PhoneNumber = author.PhoneNumber;
                updateAuthor.Email = author.Email;
                updateAuthor.UpdateDate = DateTime.Now;
                updateAuthor.FirstName = author.FirstName;
                updateAuthor.LastName = author.LastName;
                updateAuthor.Status = author.Status;
                updateAuthor.SecurityStamp = "asdasdasd"; //loginden sonra burası düzenlecek

                //Eğer güncelleme sırasında yeni fotoğrag yüklenmediyse eski fotoğraf korunur, yüklendiyse URL güncellenir
                //Static Class olarak oluşturulan resim kaydetme metodu kullanılıyor (Business/Concrete)
                string photoUrl = author.PhotoFile == null ? author.ProfilePhotoPath : await ImageSaver.SaveImage(author.PhotoFile);
                updateAuthor.ProfilePhotoPath = photoUrl;
                await _userManager.UpdateAsync(updateAuthor);

                return RedirectToAction("AuthorList");
            }
            else
            {
                return View(author);
            }


        }


        public IActionResult ArticleSetStatusRefuse(int id)//talep reddetme
        {
            Article article = _articleService.GetById(id);
            article.ConfirmationStatus = ENTITIES.Enums.ConfirmationStatus.No;
            article.DateOfReply = DateTime.Now;
            _articleService.Update(article);
            TempData["Message"] = "Makale talebi reddedildi.";
            return RedirectToAction("ArticleList");
        }

        public IActionResult ArticleSetStatusConfirm(int id)//talep onaylama 
        {
            Article article = _articleService.GetById(id);
            article.ConfirmationStatus = ENTITIES.Enums.ConfirmationStatus.Yes;
            article.DateOfReply = DateTime.Now;
            _articleService.Update(article);
            TempData["Message"] = "Makale talebi onaylandı.";
            return RedirectToAction("ArticleList");
        }


        public IActionResult ArticleSetStatusPassive(int id)//makaleyi veritabanından siler//BURAYA BAKIN
        {
            Article article = _articleService.GetById(id);
            article.Status = ENTITIES.Enums.Status.Passive;
            _articleService.Update(article);
            TempData["Message"] = "Makale silindi.";
            return RedirectToAction("ArticleList");
        }
        [HttpGet]
        public async Task<IActionResult> AdminProfile()
        {
            AppUser currentUser = await _userManager.GetUserAsync(HttpContext.User);//login olan kullanıcı(author)
            return View(currentUser);
        }
        [HttpGet]
        public async Task<IActionResult> AdminProfileUpdate()
        {
            AppUser currentUser = await _userManager.GetUserAsync(HttpContext.User);//login olan kullanıcı(author)
            return View(currentUser);
        }
        [HttpPost]
        public async Task<IActionResult> AdminProfileUpdate(AppUser user)
        {
            if (ModelState.IsValid)
            {
                AppUser currentUser = await _userManager.GetUserAsync(HttpContext.User);//login olan kullanıcı(author)
                currentUser.FirstName = user.FirstName;
                currentUser.LastName = user.LastName;
                currentUser.Job = user.Job;
                currentUser.Country = user.Country;
                currentUser.Email = user.Email;
                currentUser.BirthDate = user.BirthDate;
                currentUser.CitizenId = user.CitizenId;
                currentUser.PhoneNumber = user.PhoneNumber;
                string photoUrl = user.PhotoFile == null ? user.ProfilePhotoPath : await ImageSaver.SaveImage(user.PhotoFile);
                currentUser.ProfilePhotoPath = photoUrl;
                currentUser.UpdateDate = DateTime.Now;
                await _userManager.UpdateAsync(currentUser);
                return RedirectToAction("AuthorProfile");
            }
            else
            {
                return View(user);
            }

        }

    }
}
