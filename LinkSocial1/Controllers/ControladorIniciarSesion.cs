using LinkSocial1.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace LinkSocial1.Controllers
{
    public class ControladorIniciarSesion : Controller
    {
        public IActionResult irAIniciarSesion()
        {
            return View("~/Views/InicioSesion/IniciarSesion.cshtml");
        }

        [HttpPost]
        public IActionResult IniciarSesion (string correoElectronico, string contraseña)
        {
            ServicioConsultas consultas = new ServicioConsultasImpl();
            if(string.IsNullOrEmpty(correoElectronico) || string.IsNullOrEmpty(contraseña))
            {
                return RedirectToAction("Error","Home");
            }
            if (consultas.IniciarSesion(correoElectronico, contraseña))
            {
                return RedirectToAction("cargarPaginaInicio", "ControladorPaginaInicio");
            }
            else
            {
                return View("~/Views/InicioSesion/IniciarSesion.cshtml");
            }


        }
    }
}
