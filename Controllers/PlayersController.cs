using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravianAnalytics.Controllers.Abstract;
using TravianAnalytics.Data;

namespace TravianAnalytics.Controllers {

    public class PlayersController : BaseController {
        public PlayersController(ApplicationDbContext dbContext) : base(dbContext) { }
        

        [HttpGet( "{id}")]
        public async Task<IActionResult> Show(int id) {
            var player = await _dbContext.Players.FirstOrDefaultAsync(a => a.Id == id);
            if (player == null) return NotFound();
            return View(player);
        }

    }
}
