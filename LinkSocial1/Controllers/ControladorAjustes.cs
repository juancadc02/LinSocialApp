using Microsoft.AspNetCore.Mvc;

namespace LinkSocial1.Controllers
{
    public class ControladorAjustes : Controller
    {
        public IActionResult irAAjustes()
        {
            try
            {
                return View("~/Views/Ajustes/PaginaAjuste.cshtml");

            }
            catch (Exception ex)
            {
                Console.WriteLine("Se ha producido un error: {0}", ex);
                return View("~/Views/Errores/paginaError.cshtml");
            }
        }
        public IActionResult irAModificarContraseña()
        {
            try
            {
                return View("~/Views/Ajustes/ModificarContraseña.cshtml");

            }
            catch (Exception ex)
            {
                Console.WriteLine("Se ha producido un error: {0}", ex);
                return View("~/Views/Errores/paginaError.cshtml");
            }
        }
        
    }
}
