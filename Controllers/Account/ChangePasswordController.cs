using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TravianAnalytics.Controllers.Abstract;
using TravianAnalytics.Data;
using TravianAnalytics.Dtos.Identity;
using TravianAnalytics.Models.Identity.Abstract;
using TravianAnalytics.Services.Contracts.FlashMessage;

namespace TravianAnalytics.Controllers.Account {
    public class ChangePasswordController : BaseController {
        private readonly IFlashMessage _flashMessage;
        private readonly UserManager<User> _userManager;

        public ChangePasswordController(ApplicationDbContext dbContext, UserManager<User> userManager, IFlashMessage flashMessage) : base(dbContext) {
            _flashMessage = flashMessage;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index() {
            var user = await GetAuthenticatedUserAsync();
            if (user == null) {
                return NotFound();
            }

            return View("../Account/ChangePassword/Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(ChangePasswordDto passwordDto) {
            var user = await GetAuthenticatedUserAsync();
            if (user == null) {
                return NotFound();
            }

            if (ModelState.IsValid) {
                if (await _userManager.CheckPasswordAsync(user, passwordDto.CurrentPassword)) {
                    await ChangeUserPassword(user, passwordDto.NewPassword);
                    _flashMessage.Success($"Password alterada com sucesso.");
                    await _dbContext.SaveChangesAsync();
                } else {
                    _flashMessage.Danger($"Password errada.");
                }

                return RedirectToAction("Index");
            }

            return View("../Account/ChangePassword/Index");
        }

        private async Task ChangeUserPassword(User user, string newPassword) {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            await _userManager.ResetPasswordAsync(user, token, newPassword);
        }


    }
}