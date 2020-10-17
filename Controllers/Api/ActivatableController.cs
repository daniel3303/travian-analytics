using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravianAnalytics.Controllers.Abstract;
using TravianAnalytics.Data;
using TravianAnalytics.Models.Contracts;

namespace TravianAnalytics.Controllers.Api {
    [Authorize]
    public class ActivableController : ApiController {
        public ActivableController(ApplicationDbContext dbContext) : base(dbContext) { }

        [HttpPost]
        public async Task<IActionResult> Index(string modelName, int key) {
            var modelType = Type.GetType(modelName);
            if (modelType == null) return BadRequest("Model type not found.");

            var dbType = _dbContext.GetType();
            var dbsetProp = dbType.GetProperties()
                .FirstOrDefault(p => p.PropertyType.GenericTypeArguments.Any(t => t == modelType));
            if (dbsetProp == null) return BadRequest("DbSet not found.");


            if (!typeof(IActivable).IsAssignableFrom(modelType))
                return BadRequest("The model must implement the IActivable interface.");

            var dbSet = (dynamic)dbsetProp.GetValue(_dbContext);
            var model = (IActivable)dbSet.Find(key);
            if (model == null) return NotFound($"{modelType.Name} with primary key {key} was not found.");

            model.Active = !model.Active;
            await _dbContext.SaveChangesAsync();
            return Ok(model.Active);
        }
    }
}