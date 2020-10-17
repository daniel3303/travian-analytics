using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TravianAnalytics.Controllers {
    [Route("error")]
    [AllowAnonymous]
    public class ErrorController : Controller {

        [Route("{errorCode}")]
        public IActionResult Index(string errorCode) {
            switch (errorCode) {
                case "404":
                    Response.StatusCode = 404;
                    ViewData["Title"] = "404";
                    ViewData["BreadCrumb"] = new string[,] { { "Dashboard", Url.RouteUrl("Dashboard") }, { "Página não encontrada", "" } };
                    ViewData["Text"] = "A página solicitada não foi encontrada.";
                    break;
                case "403":
                case "405":
                    ViewData["Title"] = "403";
                    ViewData["BreadCrumb"] = new string[,] { { "Dashboard", Url.RouteUrl("Dashboard") }, { "Sem permissões", "" } };
                    ViewData["Text"] = "Não tem permissões para o pedido efetuado";
                    break;
                case "500":
                    ViewData["Title"] = "500";
                    ViewData["BreadCrumb"] = new string[,] { { "Dashboard", Url.RouteUrl("Dashboard") }, { "Página de erro", "" } };
                    ViewData["Text"] = "Ocorreu um erro no pedido efetuado";
                    break;
                default:
                    throw new Exception(string.Concat("Status code não processado: ", errorCode));
            }
            return View();
        }

    }

}