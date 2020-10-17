using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using TravianAnalytics.Extensions;

namespace TravianAnalytics.Infrastructure {
    public sealed class RedirectHandler {
        private readonly RequestDelegate _next;

        public RedirectHandler(RequestDelegate next) {
            _next = next;
        }

        public async Task Invoke(HttpContext context) {
            var session = context.Session;

            var currController = context.Request.RouteValues["controller"]?.ToString() ?? "";
            if (currController != "") {
                var currAction = context.Request.RouteValues["action"].ToString();
                if (currAction == "Index") {
                    var state = session.GetObject<IDictionary<string, string>>("IndexState") ?? new Dictionary<string, string>();
                    state[currController] = context.Request.Path + context.Request.QueryString.ToString();
                    session.SetObject("IndexState", state);
                }
            }
            await _next(context);

            try {
                if (context.Request.Method == HttpMethods.Post && context.Request.Form.ContainsKey("GoBack") && !context.Response.HasStarted) {
                    var stateDict = session.GetObject<IDictionary<string, string>>("IndexState");
                    if (stateDict != null) {
                        var target = session.GetObject<IDictionary<string, string>>("IndexState").FirstOrDefault(i => i.Key == currController).Value;
                        if (!string.IsNullOrEmpty(target)) {
                            context.Response.Redirect(target);
                        } else {
                            context.Response.Redirect(string.Concat("/" + currController));
                        }
                    } else {
                        context.Response.Redirect(string.Concat("/" + currController));
                    }
                }
            } catch {

            }

        }

    }
}