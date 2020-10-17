using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;

namespace TravianAnalytics.Services {
    public class RedirectService {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public RedirectService(IHttpContextAccessor httpContextAccessor) {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetGoBackUrl() {
            var GoBackUrl = "";
            if (!string.IsNullOrEmpty(_httpContextAccessor.HttpContext.Request.Query["b"])) {
                GoBackUrl = WebUtility.UrlDecode(_httpContextAccessor.HttpContext.Request.Query["b"]);
                if (!GoBackUrl.StartsWith("/") && !GoBackUrl.StartsWith("http")) {
                    GoBackUrl = "/" + GoBackUrl;
                }
            }
            if (GoBackUrl?.Length == 0) {
                GoBackUrl = "/";
            }
            return GoBackUrl;
        }


        public string SetGoBackUrl() {
            return WebUtility.UrlEncode(_httpContextAccessor.HttpContext.Request.GetEncodedUrl());
        }

    }
}
