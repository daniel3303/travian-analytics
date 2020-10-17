using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TravianAnalytics.Extensions {
    public static class TagBuilderExtensions {
        public static string GetHtml(this TagBuilder tagBuilder) {
            using var writer = new System.IO.StringWriter();
            tagBuilder.WriteTo(writer, HtmlEncoder.Default);
            return writer.ToString();
        }

    }
}