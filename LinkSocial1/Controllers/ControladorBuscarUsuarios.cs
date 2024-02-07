using DB.Modelo;
using LinkSocial1.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace LinkSocial1.Controllers
{
    public class ControladorBuscarUsuarios : Controller
    {
        public IActionResult irABuscarUsuarios()
        {
            return View("~/Views/BuscarUsuarios/PaginaBuscarUsuarios.cshtml");
        }

        public IActionResult buscarUsuario(string correoElectronico)
        {
            ServicioConsultas consultas = new ServicioConsultasImpl();

            Usuarios usuarioEncontrado = consultas.buscarUsuario(correoElectronico);

            if (usuarioEncontrado != null)
            {
                string nombreUsuario = usuarioEncontrado.nombreCompleto;
                TempData["mensajeExito"] = "Usuario encontrado";
                return View("~/Views/BuscarUsuarios/PaginaBuscarUsuarios.cshtml", usuarioEncontrado);

            }
            else
            {
                TempData["mensajeError"] = "Usuario no encontrado";
                return View("~/Views/BuscarUsuarios/PaginaBuscarUsuarios.cshtml");
            }

        }

        [HttpGet]
        public IActionResult perfilUsuarioBuscado(int idUsuario)
        {
            ServicioConsultas consultas = new ServicioConsultasImpl();

            Usuarios usuarioEncontrado = consultas.buscarUsuarioPorId(idUsuario);

            List<Publicaciones> listaPublicaciones = consultas.buscarPublicacionesPorIdUsuario(idUsuario);
            int numeroPublicacion = listaPublicaciones.Count;
            ViewData["numeroPublicacion"] = numeroPublicacion;
            ViewData["listaPublicaciones"] = listaPublicaciones;
            TempData["mensajeExito"] = "Usuario encontrado";
                
            return View("~/Views/BuscarUsuarios/PerfilUsuarioBuscado.cshtml", usuarioEncontrado);
            
          
        }
    }
}
