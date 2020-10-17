using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravianAnalytics.Controllers.Abstract;
using TravianAnalytics.Data;
using TravianAnalytics.Dtos.Identity;
using TravianAnalytics.Models.Identity.Abstract;
using TravianAnalytics.Services.Contracts.FlashMessage;

namespace TravianAnalytics.Controllers.Users {

    [Authorize(Policy = "Users")]
    public class UsersController : BaseController {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IFlashMessage _flashMessage;

        public UsersController(ApplicationDbContext dbContext, UserManager<User> userManager, IMapper mapper,
            IFlashMessage flashMessage) :
            base(dbContext) {
            _userManager = userManager;
            _mapper = mapper;
            _flashMessage = flashMessage;
        }

        [HttpGet]
        public IActionResult Index() {
            return View(_dbContext.Users.OrderByDescending(u => u.Active).ThenBy(u => u.Name));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Edit(int id) {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null) {
                return NotFound();
            }
            ViewData["userModel"] = user;
            var userDto = _mapper.Map<UserForEditDto>(user);
            userDto.SetEnabledClaims(user.Claims);
            return View(userDto);
        }

        [HttpPost("{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UserForEditDto userDto) {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null) {
                return NotFound();
            }
            ViewData["userModel"] = user;
            if (ModelState.IsValid) {
                _mapper.Map(userDto, user);

                // Password change
                if (userDto.Password != null && await ValidatePassword(user, userDto.Password)) {
                    await ChangeUserPassword(user, userDto.Password);
                }

                // Claims
                user.Claims.Clear();

                foreach (var claimDto in userDto.UserClaims.Where(c => c.Enabled)) {
                    user.Claims.Add(new UserClaim(user, claimDto.Type, claimDto.Name));
                }
                await _dbContext.SaveChangesAsync();

                _flashMessage.Success($"Registo atualizado com sucesso.");

                userDto.SetEnabledClaims(user.Claims);
                return RedirectToAction(nameof(Edit), new { user.Id });
            }
            return View(userDto);
        }


        private async Task<bool> ValidatePassword(User user, string password) {
            var valid = true;
            foreach (var validator in _userManager.PasswordValidators) {
                var validation = await validator.ValidateAsync(_userManager, user, password);
                foreach (var error in validation.Errors) {
                    valid = false;
                    ModelState.AddModelError("Password", error.Description);
                }
            }

            return valid;
        }

        private async Task ChangeUserPassword(User user, string newPassword) {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            await _userManager.ResetPasswordAsync(user, token, newPassword);
        }
    }
}