using DB.Modelo;
using LinkSocial1.Servicios;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LinkSocial1.Controllers
{
    public class ControladorSeguidores : Controller
    {

        public IActionResult solicitudSeguimiento(int idSeguidorSeguido)
        {
            ServicioConsultas consultas = new ServicioConsultasImpl();
            
            //Usuario que tiene la sesion iniciada.
            var claimsPrincipal = User;
            string idSeguidorSolicitud = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            //Fecha actual del momento en el que se siguen.
            DateTime fchSeguimiento = DateTime.Now.ToUniversalTime();

            // Verificar si el usuario ya está siguiendo al otro
            bool siguiendo = consultas.estaSiguiendo(Convert.ToInt32(idSeguidorSolicitud), idSeguidorSeguido);

            // Si ya está siguiendo, no realizar ninguna acción
            if (siguiendo)
            {
                // También puedes cargar los datos directamente desde la base de datos
                Usuarios usuarioEncontrado = consultas.buscarUsuarioPorId(idSeguidorSeguido);
                List<Publicaciones> listaPublicaciones = consultas.buscarPublicacionesPorIdUsuario(idSeguidorSeguido);
                int numeroPublicacion = listaPublicaciones.Count;
                ViewData["numeroPublicacion"] = numeroPublicacion;
                ViewData["listaPublicaciones"] = listaPublicaciones;
                ViewData["EstaSiguiendo"] = siguiendo;
                return View("~/Views/BuscarUsuarios/PerfilUsuarioBuscado.cshtml", usuarioEncontrado);
            }

            // Si no está siguiendo, realizar la acción de seguimiento
            Seguidores nuevoSeguidor = new Seguidores(Convert.ToInt32(idSeguidorSolicitud), Convert.ToInt32(idSeguidorSeguido), fchSeguimiento, true);
            consultas.iniciarSeguimiento(nuevoSeguidor);

            // También puedes cargar los datos directamente desde la base de datos
            Usuarios usuarioEncontradoNuevo = consultas.buscarUsuarioPorId(idSeguidorSeguido);
            List<Publicaciones> listaPublicacionesNuevo = consultas.buscarPublicacionesPorIdUsuario(idSeguidorSeguido);
            int numeroPublicacionNuevo = listaPublicacionesNuevo.Count;
            ViewData["numeroPublicacion"] = numeroPublicacionNuevo;
            ViewData["listaPublicaciones"] = listaPublicacionesNuevo;

            ViewData["EstaSiguiendo"] = true;

            return View("~/Views/BuscarUsuarios/PerfilUsuarioBuscado.cshtml", usuarioEncontradoNuevo);
        }





    }
}
