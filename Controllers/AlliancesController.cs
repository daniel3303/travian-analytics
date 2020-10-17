using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravianAnalytics.Controllers.Abstract;
using TravianAnalytics.Data;

namespace TravianAnalytics.Controllers {

    public class AlliancesController : BaseController {
        public AlliancesController(ApplicationDbContext dbContext) : base(dbContext) { }
        
        public IActionResult Index() {
            return View(_dbContext.Alliances.OrderBy(a => a.Name));
        }

        [HttpGet( "{id}")]

        public async Task<IActionResult> Show(int id) {
            var alliance = await _dbContext.Alliances.FirstOrDefaultAsync(a => a.Id == id);
            if (alliance == null) return NotFound();
            return View(alliance);
        }

    }
}
