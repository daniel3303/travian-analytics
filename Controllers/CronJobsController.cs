using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using TravianAnalytics.Controllers.Abstract;
using TravianAnalytics.Data;
using TravianAnalytics.Services.Contracts.CronJobs;

namespace TravianAnalytics.Controllers {
    [Authorize(Policy = "MASTER")]
    public class CronJobsController : BaseController {
        public CronJobsController(ApplicationDbContext dbContext) : base(dbContext) {
        }

        [HttpGet]
        public IActionResult Index() {
            ViewData["CronJobSubscribers"] = HttpContext.RequestServices.GetServices<ICronJobSubscriber>().ToList();
            return View(_dbContext.CronJobs.OrderByDescending(c => c.ExecutionTime));
        }
        
        [AllowAnonymous]
        public async Task<IActionResult> Execute() {
            var cronJobManager = HttpContext.RequestServices.GetService<ICronJobManager>();
            if (cronJobManager == null) return BadRequest(false);
            await cronJobManager.ExecuteAsync();
            return Ok(true);
        }
    }
}