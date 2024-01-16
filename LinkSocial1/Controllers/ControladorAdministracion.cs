using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LinkSocial1.Controllers
{
    [Authorize(Roles = "admin")]
    public class ControladorAdministracion : Controller
    {
        public IActionResult irPaginaAdmin()
        {
           
            return View("~/Views/Administracion/PaginaAdministracion.cshtml");
        }
    }
}
