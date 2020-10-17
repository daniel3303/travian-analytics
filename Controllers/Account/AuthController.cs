using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TravianAnalytics.Controllers.Abstract;
using TravianAnalytics.Data;
using TravianAnalytics.Dtos.Auth;
using TravianAnalytics.Models.Identity.Abstract;
using TravianAnalytics.Services.Contracts.FlashMessage;

namespace TravianAnalytics.Controllers.Account {

    [AllowAnonymous]
    public class AuthController : BaseController {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IFlashMessage _flashMessage;

        public AuthController(ApplicationDbContext dbContext, UserManager<User> userManager,
            SignInManager<User> signInManager, IFlashMessage flashMessage) : base(
            dbContext) {
            _userManager = userManager;
            _signInManager = signInManager;
            _flashMessage = flashMessage;
        }

        [HttpGet]
        public IActionResult Login() {
            var userManager = HttpContext.RequestServices.GetService<UserManager<User>>();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto loginDto) {
            if (ModelState.IsValid) {
                var user =  await _dbContext.Users.FirstOrDefaultAsync(u => u.UserName == loginDto.Username);
                if (user?.Active ?? false) {
                    var result = await _signInManager.PasswordSignInAsync(loginDto.Username, loginDto.Password, false, false);
                    if (result.Succeeded) {
                        return Redirect(Url.RouteUrl("Dashboard"));
                    }
                }

                _flashMessage.Danger("E-mail ou Password errados!");
            }

            return View(loginDto);
        }

        public async Task<IActionResult> Logout() {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        public IActionResult AccessDenied() {
            ViewData["Title"] = "Acesso proibído";
            ViewData["BreadCrumb"] = new string[,]
                {{"Dashboard", Url.RouteUrl("Dashboard")}, {"Não tem acesso à pagina pedida", ""}};
            ViewData["Text"] = "Não tem acesso à pagina pedida.";
            return View();
        }

        [HttpGet]
        public IActionResult LockedOut() {
            ViewData["Title"] = "Conta bloqueada";
            ViewData["BreadCrumb"] = new string[,]
                {{"Dashboard", Url.RouteUrl("Dashboard")}, {"Não tem acesso à pagina pedida", ""}};
            ViewData["Text"] = "A sua conta está bloqueada";
            return View();
        }

    }

}