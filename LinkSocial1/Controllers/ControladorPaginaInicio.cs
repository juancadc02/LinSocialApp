using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LinkSocial1.Controllers
{
    [Authorize]
    public class ControladorPaginaInicio : Controller
    {
        public ActionResult cargarPaginaInicio()
        {
            return View("~/Views/Home/PaginaInicio.cshtml");
        }
    }
}
