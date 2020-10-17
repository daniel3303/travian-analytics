using Microsoft.AspNetCore.Mvc;
using TravianAnalytics.Data;

namespace TravianAnalytics.Controllers.Abstract {

    [Route("api/{controller=Home}/{action=Index}")]
    public abstract class ApiController : BaseController {
        protected ApiController(ApplicationDbContext dbContext) : base(dbContext) { }
    }
}