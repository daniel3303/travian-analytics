using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravianAnalytics.Controllers.Abstract;
using TravianAnalytics.Data;
using TravianAnalytics.Models.Contracts;

namespace TravianAnalytics.Controllers.Api {
    [Authorize]
    public class SortableController : ApiController {
        public SortableController(ApplicationDbContext dbContext) : base(dbContext) { }

        [HttpPost]
        public async Task<IActionResult> Index(string modelName, List<int> items) {
            if (modelName == null) return BadRequest("The ModelName is a required parameter.");
            var modelType = Type.GetType(modelName);
            if (modelType == null) return BadRequest("Model type not found.");

            var dbType = _dbContext.GetType();
            var dbsetProp = dbType.GetProperties()
                .FirstOrDefault(p => p.PropertyType.GenericTypeArguments.Any(t => t == modelType));
            if (dbsetProp == null) return BadRequest("DbSet not found.");


            if (!typeof(ISortable).IsAssignableFrom(modelType))
                return BadRequest("The model must implement the ISortable interface.");

            var dbSet = (dynamic)dbsetProp.GetValue(_dbContext);

            foreach (ISortable entity in dbSet) {
                if (items.Contains(entity.Id))
                    entity.Order = items.Count() - items.IndexOf(entity.Id);
            }

            await _dbContext.SaveChangesAsync();
            return Ok(true);
        }
    }
}