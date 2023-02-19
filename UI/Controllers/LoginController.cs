using ENTITIES.Entities;
using ENTITIES.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using REPOSITORIES.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using UI.Models;

namespace UI.Controllers
{
    public class LoginController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly IWebHostEnvironment _env;
        private readonly IGenericRepository<Article> _articleService;

        public LoginController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IEmailSender emailSender, IWebHostEnvironment env, IGenericRepository<Article> articleService)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _env = env; // Çalışma ortamına ait bilgileri tutar
            _articleService = articleService;
            _signInManager = signInManager; // Identity üzerinden kullanıcı girişi, şifre yenileme ve değiştirme gibi işlemleri yapabilmemizi sağlar
        }

        [HttpGet]
        public IActionResult Login() // Giriş ekranı LOGİN - SİGNUP İÇİN GET BURADA.
        {
            return View();
        }
        [AllowAnonymous]
        public IActionResult AccessDeniedPage()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel vm) //Giriş işlemi
        {
            if (!ModelState.IsValid)
            {
                if (vm.Email == null || vm.Password == null)
                {
                   TempData["Message2"] = "Email veya Şifre boş bırakılamaz";
                }
                else
                {
                   TempData["Message2"] = "Şifre veya Email doğru formatta değil";
                }
                return View(vm);
            }
            var user = await _userManager.FindByEmailAsync(vm.Email);
            if (user == null)
            {
                TempData["Message2"] = "Böyle bir kullanıcı bulunamadı!";
                return View(vm);
            }


            var result = await _signInManager.PasswordSignInAsync(user, vm.Password, false, false); // Giriş yapma işlemi denenir

            if (result.Succeeded)
            {

                if (user.FirstName == null) // Sisteme yeni eklenen bir kulllanıcı ilk girişinde bu sayfaya yönlendirilecek.
                {
                    FirstEntranceAuthorViewModel fevm = new FirstEntranceAuthorViewModel()
                    {
                        Email = vm.Email,
                    };
                    return RedirectToAction("FirstEntranceAuthor", "Author", fevm); // ilk girişi ise bu view da bilgilerini dolduracak
                }


                var role = (await _userManager.GetRolesAsync(user)).FirstOrDefault(); // Giriş yapan kullanıcının rolü alınır

                if (role == "admin")
                {
                    return RedirectToAction("HomePage", $"{role}"); // Rolüne göre uygun adrese yönlendirilir.Furkan burada id gönderilmesi gerektiğini söylüyor!!

                }


                return RedirectToAction("HomePage", "Guest"); // Rolüne göre uygun adrese yönlendirilir.Furkan burada id gönderilmesi gerektiğini söylüyor!!


            }
            else
            {
                /*ModelState.AddModelError(string.Empty, "Hatalı şifre ya da Email adı!");*/ //=> view'larda asp-for validation/summary yazılmazsa bu mesaj görüntülenemez ya da bir validation mesajı yapılmazsa buradan da gönderilebilir.View da bu mesajları kapattığımız için görülmeyecek!!!
                TempData["Message2"] = "Hatalı şifre !";
                return View(vm);
            }

        }




        [HttpPost]
        public async Task<IActionResult> SignUp(LoginViewModel vm, string SecondPassword)//üye kayıt ekranı
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(vm.Email);
                if (user == null)
                {
                    AppUser newUser = new AppUser()
                    {
                        CreateDate = DateTime.Now,
                        Email = vm.Email,
                        UserName = vm.Email


                    };
                    if (vm.Password == SecondPassword)
                    {
                        var result = await _userManager.CreateAsync(newUser, vm.Password); // Identity ie yeni bir kullanıcı oluşturuluyor.
                        await _userManager.AddToRoleAsync(newUser, "author");
                        TempData["Message"] = "Kayıt Başarılı";
                        return RedirectToAction("Login");//author controller 'ın update ine yönlendirilecek-değişecek!!!
                    }
                    else
                    {
                        TempData["Message2"] = "Şifre Eşleşmiyor";
                        return RedirectToAction("Login");
                    }
                }
                else
                {
                    TempData["Message2"] = "Böyle bir hesap zaten var!";

                    return RedirectToAction("Login");
                }
            }
            else
            {
                TempData["Message2"] = "Girdiğiniz Bilgiler Eksik ya da Hatalı!";
                //ModelState.AddModelError(string.Empty, "Girdiğiniz bilgiler eksik ya da hatalı!");
                return RedirectToAction("Login");
            }



        }


        public IActionResult ForgotPassword() // Şifremi unuttum ekranı
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            if (ModelState.IsValid)
            {
                if (email == null)
                {
                    ViewBag.message2 = "Bu alan boş geçilemez!";
                    return View();
                }
                AppUser user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    ViewBag.message2 = "Böyle bir kullanıcı bulunamadı!";
                    return View();
                }
                else
                {
                    string token = await _userManager.GeneratePasswordResetTokenAsync(user); // Token üretiliyor
                    string encodedToken = HttpUtility.UrlEncode(token); // URL ile gönderileceği için bazı karakterlerin bozulmaması için encode ediliyor
                    string generatedUrlExtension = Url.Action("NewPassword", "Login", new { email = email, token = encodedToken });

                    ViewBag.GeneratedUrlExtension = generatedUrlExtension;


                    //Mail içeriği
                    string content = "";

                    //Çalışma ortamına göre mailin içeriği belirleniyor
                    if (_env.EnvironmentName == "Development")
                    {
                        content = $"Merhaba {user.GetFullName()} , <br />" +
                            $"Şifreni yenilemek için linke tıklayabilirsin: " +
                            $"<a href='https://localhost:44304{Url.Action("NewPassword", "Login", new { email = email, token = encodedToken })}'>Şifre Yenile</a> <br />" +
                            $"İyi çalışmalar dileriz.. <br /> <br />" +
                            $"DEVHELL";
                    }
                    else if (_env.EnvironmentName == "Production")
                    {
                        content = $"Merhaba {user.GetFullName()} , <br />" +
                            $"Şifreni yenilemek için linke tıklayabilirsin: " +
                            $"<a href='https://devhell.com.tr{Url.Action("NewPassword", "Login", new { email = email, token = encodedToken })}'>Şifre Yenile</a>" +
                             $"İyi çalışmalar dileriz.. <br /> <br />" +
                            $"DEVHELL";
                    }
                    await _emailSender.SendEmailAsync(email, "Şifre Yenile", content); //Mail gönderiliyor
                    TempData["Message"] = "Email Gönderildi";

                    return RedirectToAction("Login");
                }
            }
            else
            {
                ViewBag.message2 = "Mail Formatı Yanlış!";
                return View();
            }


        }
        public IActionResult NewPassword(string email, string token) // Şifre sıfırlama ekranı
        {
            NewPasswordViewModel vm = new NewPasswordViewModel();
            vm.Email = email;
            vm.Token = token;
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> NewPassword(NewPasswordViewModel vm) // Şifre sıfırlama işlemi
        {
            if (!ModelState.IsValid)
            {
                if (vm.Password==null||vm.SecondPassword==null)
                {
                TempData["Message2"] = "Şifre alanı boş geçilemez!";

                }
                else if (vm.Password!=vm.SecondPassword)
                {
                TempData["Message2"] = "Şifreler eşleşmiyor!";

                }
                else
                {

                 TempData["Message2"] = "Şifre formatı yanlış!";
                }
                return View(vm);
            }

            AppUser user = await _userManager.FindByEmailAsync(vm.Email);
            user.UpdateDate = DateTime.Now;
            string decodedtoken = HttpUtility.UrlDecode(vm.Token); // Encode edilen token decode ediliyor
            IdentityResult passwordChangeResult = await _userManager.ResetPasswordAsync(user, decodedtoken, vm.Password);
            if (passwordChangeResult.Succeeded)
            {
                await _userManager.UpdateSecurityStampAsync(user);
                TempData["Message"] = "Şifreniz Değiştirildi";
            }
            else
            {
                TempData["Message2"] = "Şifreniz Değiştirilemedi";
            }
            return RedirectToAction("Login");

        }

        [HttpGet]
        public async Task<IActionResult> Logout() // Çıkış işlemi
        {
            await _signInManager.SignOutAsync();
            TempData["Message"] = "Çıkış Başarılı.";
            return RedirectToAction("Index", "HomePage");//bu sayfa guest controller da düzenlenip çıkış yapan yazarı oraya yönlendirecek!

        }





    }
}
