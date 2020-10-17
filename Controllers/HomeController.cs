using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TravianAnalytics.Controllers.Abstract;
using TravianAnalytics.Data;

namespace TravianAnalytics.Controllers {

    [Route("/", Name = "Dashboard")]
    public class HomeController : BaseController {
        public HomeController(ApplicationDbContext dbContext) : base(dbContext) { }
        public IActionResult Index() {
            
            return View(_dbContext.Notifications.OrderByDescending(n => n.Time));

        }

    }
}
