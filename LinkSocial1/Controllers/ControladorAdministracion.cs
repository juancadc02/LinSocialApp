using DB.Modelo;
using LinkSocial1.Servicios;
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
        public IActionResult irUsuario()
        {
            ServicioConsultas consultas = new ServicioConsultasImpl();
            //Creamos una lista y llamamos al metodo lista usuarios.
            List<Usuarios> listaUsuario = consultas.mostrarUsuarios();
            ViewData["listaUsuario"] = listaUsuario;
            return View("~/Views/Administracion/listaUsuario.cshtml");
        }

    }

    
}
