using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravianAnalytics.Data;
using TravianAnalytics.Models.Identity.Abstract;

namespace TravianAnalytics.Controllers.Abstract {
    [Authorize]
    [Route("{controller=Home}/{action=Index}")]
    public abstract class BaseController : Controller {
        protected readonly ApplicationDbContext _dbContext;

        protected BaseController(ApplicationDbContext dbContext) {
            _dbContext = dbContext;
        }
        
        protected async Task<User> GetAuthenticatedUserAsync() {
            var idClaim = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == idClaim);
        }
        
        protected User GetAuthenticatedUser() {
            return GetAuthenticatedUserAsync().Result;
        }
    }
}