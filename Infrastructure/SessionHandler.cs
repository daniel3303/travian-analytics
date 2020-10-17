using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using TravianAnalytics.Helpers;

namespace TravianAnalytics.Infrastructure {
    public class SessionHandler {
        private readonly RequestDelegate _next;

        public SessionHandler(RequestDelegate next) {
            _next = next;
        }

        public async Task Invoke(HttpContext context) {
            context.RequestServices.GetService<EntityChangeTracker>();
            
            var controller = context.Request.RouteValues["controller"]?.ToString() ?? "";
            if (controller != "") {
                var linkGenerator = context.RequestServices.GetService<LinkGenerator>();
                var indexPath = linkGenerator.GetPathByAction(context, "Index", controller);
                context.Session.SetString("Controller", controller);
                context.Session.SetString("IndexPath", indexPath ?? "/");
            }
            await _next(context);
        }

    }
}