using AutoMapper;
using BUSINESS.Abstract;
using BUSINESS.Concrete;
using ENTITIES.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UI.Models;

namespace UI.Controllers
{
    [Authorize(Roles = "author")]
    public class AuthorController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IGenericService<Article> _articleService;
        private readonly IMapper _mapper;

        public AuthorController(UserManager<AppUser> userManager, IGenericService<Article> articleService, IMapper mapper)
        {
            _userManager = userManager;
            _articleService = articleService;
            _mapper = mapper;
        }

        public async Task<IActionResult> HomePage()
        {
            AppUser currentUser = await _userManager.GetUserAsync(HttpContext.User);//login olan kullanıcı(author)
            return View();
        }
        public async Task<IActionResult> MyArticleListForEdit()//tüm makalelerimi görüntülediğim sayfa
        {
            AppUser currentUser = await _userManager.GetUserAsync(HttpContext.User);//login olan kullanıcı(author)

            IEnumerable<Article> articles = (_articleService.GetDefault(x => x.AuthorId == currentUser.Id)?.OrderByDescending(x => x.ConfirmationStatus).ThenBy(x => x.CreateDate).Where(x => x.Status != ENTITIES.Enums.Status.Passive));
            return View(articles);
        }
        public async Task<IActionResult> ArticleDetails(int id)//makale detaylarım
        {
            AppUser currentUser = await _userManager.GetUserAsync(HttpContext.User);
            Article article = _articleService.GetById(id);
            if (currentUser.Id == article.AuthorId)
            {
                return View(article);

            }
            return RedirectToAction("MyArticleListForEdit");
        }
        public async Task<IActionResult> ArticleUpdate(int id)//makalemi düzenle
        {
            return View(_articleService.GetById(id));
        }
        [HttpPost]
        public async Task<IActionResult> ArticleUpdate(Article article)//makalemi düzenle
        {
            if (ModelState.IsValid)
            {
                article.UpdateDate = DateTime.Now;
                string photoUrl = article.PhotoFile == null ? article.PhotoPath : await ImageSaver.SaveImage(article.PhotoFile);
                article.PhotoPath = photoUrl;
                _articleService.Update(article);

                TempData["Message"] = "Makale Güncellendi";
                return RedirectToAction("MyArticleListForEdit");
            }
            else
            {
                return View(article);
            }

        }
        [HttpGet]
        public IActionResult FirstEntranceAuthor(FirstEntranceAuthorViewModel fevm)//yaa-zarın ilk girdiğinde erişeceği sayfa burada bilgilerini dolduracak
        {
            return View(fevm);
        }
        [HttpPost]
        public async Task<IActionResult> FirstEntranceAuthor(FirstEntranceAuthorViewModel fevm, int id)
        {
            AppUser currentUser = await _userManager.GetUserAsync(HttpContext.User);
           
            if (ModelState.IsValid)
            {
                currentUser.FirstName = fevm.FirstName;
                currentUser.SecondName = fevm.SecondName;
                currentUser.LastName = fevm.LastName;
                currentUser.SecondLastName = fevm.SecondLastName;
                currentUser.PhoneNumber = fevm.PhoneNumber;
                currentUser.BirthDate = fevm.BirthDate;
                currentUser.Country = fevm.Country;
                currentUser.CitizenId = fevm.CitizenId;
                currentUser.Job = fevm.Job;
                currentUser.Gender = fevm.Gender;
                currentUser.UpdateDate = DateTime.Now;
                string photoUrl = fevm.PhotoFile == null ? fevm.ProfilePhotoPath : await ImageSaver.SaveImage(fevm.PhotoFile);
                currentUser.ProfilePhotoPath = photoUrl;                
                await _userManager.UpdateAsync(currentUser);
                TempData["Message"] = "Bilgileriz Kaydedildi";
                return RedirectToAction("HomePage","Guest");
            }
            else
            {
                
                return View(fevm);
            }

        }

        [HttpGet]
        public IActionResult CreateArticle()
        {

            CreateArticleVM vm = new CreateArticleVM();

            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> CreateArticle(CreateArticleVM vm)
        {
            AppUser currentUser = await _userManager.GetUserAsync(HttpContext.User);//login olan kullanıcı(author)
            if (ModelState.IsValid)
            {
                Article newArticle = new Article()
                {
                    Title = vm.Title,
                    Category=vm.Category,
                    Content = vm.Content,
                    PhotoFile = vm.PhotoFile,
                    AuthorId = currentUser.Id,
                    ConfirmationStatus = ENTITIES.Enums.ConfirmationStatus.Pending

                };
                string photoUrl = vm.PhotoFile == null ? vm.PhotoPath : await ImageSaver.SaveImage(vm.PhotoFile);
                newArticle.PhotoPath = photoUrl;
                _articleService.Add(newArticle);
                TempData["Message"] = "Makale Oluşturuldu! Onay Bekleniyor...";
                return RedirectToAction("HomePage", "Guest");
            }
            else
            {
                return View(vm);
            }

        }
        [HttpGet]
        public async Task<IActionResult> AuthorProfile()
        {
            AppUser currentUser = await _userManager.GetUserAsync(HttpContext.User);//login olan kullanıcı(author)
            return View(currentUser);
        }
        [HttpGet]
        public async Task<IActionResult> AuthorUpdate()
        {
            AppUser currentUser = await _userManager.GetUserAsync(HttpContext.User);//login olan kullanıcı(author)
            return View(currentUser);
        }
        [HttpPost]
        public async Task<IActionResult> AuthorUpdate(AppUser user)
        {
            AppUser currentUser = await _userManager.GetUserAsync(HttpContext.User);//login olan kullanıcı(author)
            if (ModelState.IsValid)
            {
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
