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
            var claimsPrincipal = User;
            string idSeguidorSolicitud = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            DateTime fchSeguimiento = DateTime.Now.ToUniversalTime();

            Seguidores nuevoSeguidor = new Seguidores(Convert.ToInt32(idSeguidorSolicitud), Convert.ToInt32(idSeguidorSeguido), fchSeguimiento);
            consultas.iniciarSeguimiento(nuevoSeguidor);

            // También puedes cargar los datos directamente desde la base de datos
            Usuarios usuarioEncontrado = consultas.buscarUsuarioPorId(idSeguidorSeguido);
            List<Publicaciones> listaPublicaciones = consultas.buscarPublicacionesPorIdUsuario(idSeguidorSeguido);
            int numeroPublicacion = listaPublicaciones.Count;
            ViewData["numeroPublicacion"] = numeroPublicacion;
            ViewData["listaPublicaciones"] = listaPublicaciones;

            // Verificar si el usuario está siguiendo al otro
            bool siguiendo = consultas.estaSiguiendo(Convert.ToInt32(idSeguidorSolicitud), idSeguidorSeguido);
            ViewData["EstaSiguiendo"] = siguiendo;

            return View("~/Views/BuscarUsuarios/PerfilUsuarioBuscado.cshtml", usuarioEncontrado);
        }




    }
}
