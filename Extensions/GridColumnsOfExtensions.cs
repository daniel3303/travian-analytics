using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using NonFactors.Mvc.Grid;
using TravianAnalytics.Models.Contracts;

namespace TravianAnalytics.Extensions {
    public static class GridColumnsOfExtensions {
        public static void AddId<T>(this IGridColumnsOf<T> gridColumn) {
            gridColumn.Add()
                .Titled("Id")
                .RenderedAs(m => m.GetType().GetProperty("Id")?.GetValue(m))
                .CssClasses = "w-1 p-2";
        }


        public static void AddActivating<T>(this IGridColumnsOf<T> gridColumn) where T : IActivable {
            gridColumn.Add()
                .RenderedAs(m => {
                    var type = (m.GetType().Namespace ?? "").StartsWith("Castle.Proxies")
                        ? m.GetType().BaseType
                        : m.GetType();
                    return string.Format(
                        "<a data-key=\"" + m.Id + "\" data-model=\"" + type?.FullName +
                        "\" class=\"activable {0}\" href=\"\"><i class=\"fas fa-lg fa-toggle-on {1}\"></i></a>",
                        m.Active ? "activated" : "deactivated", !m.Active ? "fa-rotate-180" : "");
                })
                .Encoded(false).CssClasses = "w-1 p-2";
        }

        public static void AddSorting<T>(this IGridColumnsOf<T> gridColumn) where T : ISortable {
            gridColumn.Add(m => m.Order)
                .RenderedAs(m => {
                    var type = (m.GetType().Namespace ?? "").StartsWith("Castle.Proxies")
                        ? m.GetType().BaseType
                        : m.GetType();
                    return "<button data-key=\"" + m.Id + "\" data-model=\"" + type?.FullName +
                           "\" class=\"btn-link p-0 m-0 bg-transparent border-0 sort-handle\" type=\"button\"><i class=\"fas fa-sort\"></i></button>";
                })
                .Titled("").Encoded(false).CssClasses = "p-2";
        }

        public static void AddActions<T>(this IGridColumnsOf<T> gridColumn, Action<IActionBuilder<T>> action) {
            var viewContext = gridColumn.Grid.ViewContext;
            var httpContext = viewContext?.HttpContext;
            gridColumn.Add().RenderedAs(m => {
                var builder = new ActionBuilder<T>(m, httpContext,
                    httpContext?.RequestServices.GetService<LinkGenerator>(),
                    viewContext?.RouteData.Values["controller"]?.ToString());
                action(builder);
                return builder.Html;
            }).Encoded(false).CssClasses = "p-2 w-1 white-space-nowrap align-middle";
        }
    }

    public interface IActionBuilder<TEntity> {
        public string Html { get; set; }
        public LinkGenerator LinkGenerator { get; set; }
        public HttpContext HttpContext { get; set; }
        public TEntity Model { get; set; }
        public string Controller { get; set; }

        public IActionBuilder<TEntity> AddShow(string action = null, string controller = null,
            object routeValues = null, string text = "Abrir", Func<TEntity, bool> when = null);

        public IActionBuilder<TEntity> AddEdit(string action = null, string controller = null,
            object routeValues = null, string text = "Editar", Func<TEntity, bool> when = null);

        public IActionBuilder<TEntity> AddInfo(string action = null, string controller = null,
            object routeValues = null, string text = "Info", Func<TEntity, bool> when = null);

        public IActionBuilder<TEntity> AddDelete(string action = default, string controller = default,
            object routeValues = null, string text = "Eliminar", bool goBack = true, Func<TEntity, bool> when = null);

        public IActionBuilder<TEntity> AddCustomHtml(Func<TEntity, string> action, Func<TEntity, bool> when = null);

        public IActionBuilder<TEntity> AddCustom(string action = null, string controller = null,
            object routeValues = null, string text = "Editar", string btnClass = "btn-primary",
            string icon = "fa fa-info", Func<TEntity, bool> when = null);
    }

    public class ActionBuilder<TEntity> : IActionBuilder<TEntity> {
        public string Html { get; set; }
        public LinkGenerator LinkGenerator { get; set; }
        public HttpContext HttpContext { get; set; }
        public TEntity Model { get; set; }
        public string Controller { get; set; }

        public ActionBuilder(TEntity model, HttpContext context, LinkGenerator linkGenerator, string controller) {
            Model = model;
            HttpContext = context;
            LinkGenerator = linkGenerator;
            Controller = controller;
            Html = "";
        }

        public IActionBuilder<TEntity> AddShow(string action = null, string controller = null,
            object routeValues = null, string text = "Editar", Func<TEntity, bool> when = null) {
            if (when != null && when(Model) == false) return this;
            
            var id = Model.GetType().GetProperty("Id")?.GetValue(Model) ?? "";
            action ??= "Show";
            controller ??= Controller;
            routeValues ??= new {id = id};
            var link = LinkGenerator.GetPathByAction(HttpContext, action, controller, routeValues);
            Html +=
                $"<a href=\"{link}\" class=\"btn btn-secondary btn-xs d-inline-block mx-1\"><i class=\"fas fa-eye\"></i><span class=\"d-none d-md-inline-block ml-md-2\">{text}</span></a>";
            return this;
        }

        public IActionBuilder<TEntity> AddEdit(string action = null, string controller = null,
            object routeValues = null, string text = "Editar", Func<TEntity, bool> when = null) {
            if (when != null && when(Model) == false) return this;
            
            var id = Model.GetType().GetProperty("Id")?.GetValue(Model) ?? "";
            action ??= "Edit";
            controller ??= Controller;
            routeValues ??= new {id = id};
            var link = LinkGenerator.GetPathByAction(HttpContext, action, controller, routeValues);
            var span = string.IsNullOrEmpty(text)
                ? ""
                : $@"<span class=""d-none d-md-inline-block ml-md-2"">{text}</span>";
            Html +=
                $"<a href=\"{link}\" class=\"btn btn-primary btn-xs d-inline-block mx-1\"><i class=\"fas fa-pencil-alt\"></i>{span}</a>";
            return this;
        }

        public IActionBuilder<TEntity> AddInfo(string action = null, string controller = null,
            object routeValues = null, string text = "Info", Func<TEntity, bool> when = null) {
            if (when != null && when(Model) == false) return this;
            
            var id = Model.GetType().GetProperty("Id")?.GetValue(Model) ?? "";
            action ??= "Edit";
            controller ??= Controller;
            routeValues ??= new {id = id};
            var link = LinkGenerator.GetPathByAction(HttpContext, action, controller, routeValues);
            Html +=
                $"<a href=\"{link}\" class=\"btn btn-primary btn-xs d-inline-block mx-1\"><i class=\"fas fa-eye\"></i><span class=\"d-none d-md-inline-block ml-md-2\">{text}</span></a>";
            return this;
        }

        public IActionBuilder<TEntity> AddDelete(string action = default, string controller = default,
            object routeValues = null, string text = "Eliminar", bool goBack = true, Func<TEntity, bool> when = null) {
            if (when != null && when(Model) == false) return this;
            
            var id = Model.GetType().GetProperty("Id")?.GetValue(Model) ?? "";
            action ??= "Delete";
            controller ??= Controller;
            routeValues ??= new {id = id};
            var name = goBack ? "GoBack" : "";
            var link = LinkGenerator.GetPathByAction(HttpContext, action, controller, routeValues);
            var span = string.IsNullOrEmpty(text)
                ? ""
                : $@"<span class=""d-none d-md-inline-block ml-md-2"">{text}</span>";
            Html +=
                $@"<form method=""post"" class=""form-confirm d-inline-block mx-1"" data-message=""Tem a certeza que deseja eliminar este registo?"" action=""{link}"">
                         <button type=""submit"" name=""{name}"" class=""btn btn-outline-primary btn-xs""><i class=""far fa-trash-alt""></i>{span}</button>
                    </form>";
            return this;
        }

        public IActionBuilder<TEntity> AddCustomHtml(Func<TEntity, string> action, Func<TEntity, bool> when = null) {
            if (when != null && when(Model) == false) return this;
            
            Html += action(Model);
            return this;
        }

        public IActionBuilder<TEntity> AddCustom(string action = null, string controller = null,
            object routeValues = null, string text = "Editar", string btnClass = "btn-primary",
            string icon = "fa fa-info", Func<TEntity, bool> when = null) {
            var id = Model.GetType().GetProperty("Id")?.GetValue(Model) ?? "";
            action ??= "Delete";
            controller ??= Controller;
            routeValues ??= new {id = id};
            var link = LinkGenerator.GetPathByAction(HttpContext, action, controller, routeValues);
            var span = string.IsNullOrEmpty(text)
                ? ""
                : $@"<span class=""d-none d-md-inline-block ml-md-2"">{text}</span>";
            Html +=
                $"<a href=\"{link}\" class=\"btn {btnClass} btn-xs\"><i class=\"{icon}\"></i>{span}</a>";
            return this;
        }
    }
}