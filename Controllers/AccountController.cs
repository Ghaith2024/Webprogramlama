using BarberApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BarberApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        // Login sayfası


        // Kayıt sayfasını göster
        public IActionResult Register()
        {
            return View();
        }

        // Kullanıcı kaydı
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // ApplicationUser sınıfını kullanarak yeni bir kullanıcı oluşturuyoruz
                var user = new ApplicationUser
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    FullName = model.FullName
                };

                // Kullanıcıyı oluşturuyoruz
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    var roleResult = await _userManager.AddToRoleAsync(user, "User");
                    // Kayıt başarılıysa giriş yap
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("UserDashboard", "Dashboard"); // Kullanıcıyı yönlendir
                }

                // Hataları modele ekle
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }
        public IActionResult Login()
        {
            return View();
        }

        // Login işlemi
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = await _userManager.FindByNameAsync(model.UserName);
        //        if (user != null)
        //        {
        //            var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
        //            if (result.Succeeded)
        //            {
        //                return RedirectToAction("Index", "Home"); // Ana sayfaya yönlendir
        //            }
        //            ModelState.AddModelError(string.Empty, "Geçersiz giriş denemesi.");
        //        }
        //        else
        //        {
        //            ModelState.AddModelError(string.Empty, "Kullanıcı bulunamadı.");
        //        }
        //    }
        //    return View(model);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Login(string email, string password)
        //{
        //    var user = await _userManager.FindByEmailAsync(email);
        //    if (user != null)
        //    {
        //        var result = await _signInManager.PasswordSignInAsync(user, password, false, false);
        //        if (result.Succeeded)
        //        {
        //            var roles = await _userManager.GetRolesAsync(user);
        //            if (roles.Contains("Admin"))
        //            {
        //                return RedirectToAction("Index", "Admin"); // Admin paneline yönlendir
        //            }
        //            else
        //            {
        //                return RedirectToAction("Index", "Salon"); // Normal kullanıcı sayfasına yönlendir
        //            }
        //        }
        //    }
        //    ModelState.AddModelError(string.Empty, "Geçersiz giriş denemesi.");
        //    return View();
        //}


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.UserName);
                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
                    if (result.Succeeded)
                    {
                        var roles = await _userManager.GetRolesAsync(user);
                        if (roles.Contains("Admin"))
                        {
                            return RedirectToAction("AdminDashboard", "Dashboard"); // Admin paneline yönlendir
                        }
                        else
                        {
                            return RedirectToAction("UserDashboard", "Dashboard"); // Normal kullanıcı sayfasına yönlendir
                        }
                    }
                    ModelState.AddModelError(string.Empty, "Geçersiz giriş denemesi.");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Kullanıcı bulunamadı.");
                }
            }
            return View(model);
        }


        // Logout işlemi
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
    }
}
