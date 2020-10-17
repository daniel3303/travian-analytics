using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravianAnalytics.Controllers.Abstract;
using TravianAnalytics.Data;

namespace TravianAnalytics.Controllers {

    [Authorize(Policy = "Players")]
    public class PlayersController : BaseController {
        public PlayersController(ApplicationDbContext dbContext) : base(dbContext) { }
        
        public IActionResult Index() {
            return View(_dbContext.Players.OrderBy(p => p.Name));
        }

        [HttpGet( "{id}")]
        public async Task<IActionResult> Show(int id) {
            var player = await _dbContext.Players.FirstOrDefaultAsync(a => a.Id == id);
            if (player == null) return NotFound();
            return View(player);
        }

    }
}
