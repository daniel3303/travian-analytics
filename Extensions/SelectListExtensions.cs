using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TravianAnalytics.Extensions {
    public static class SelectListExtensions {
        public static SelectList SetPlaceholder(this SelectList selectList, string text = "") {
            if (selectList.FirstOrDefault()?.Value == "") {
                return selectList;
            }

            var list = selectList.ToList();
            list.Insert(0, new SelectListItem() {
                Text = text,
                Value = ""
            });
            return new SelectList(list, "Value", "Text");
        }
    }
}