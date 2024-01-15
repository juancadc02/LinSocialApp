using Microsoft.AspNetCore.Mvc;

namespace LinkSocial1.Controllers
{
    public class ControladorPaginaInicio : Controller
    {
        public ActionResult cargarPaginaInicio()
        {
            return View("~/Views/Home/PaginaInicio.cshtml");
        }
    }
}
