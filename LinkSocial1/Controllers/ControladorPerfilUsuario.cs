using DB.Modelo;
using LinkSocial1.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LinkSocial1.Controllers
{
    [Authorize] //Tiene que tener la sesion iniciada para navegar esta pagina.
    public class ControladorPerfilUsuario : Controller
    {
       
        /// <summary>
        /// Metodo que devuelve el perfil del usuario que tiene la sesion iniciada con todos sus datos.
        /// </summary>
        /// <returns></returns>
        public IActionResult verPerfilUsuario()
        {
            try
            {
                ServicioConsultas consultas = new ServicioConsultasImpl();

                //Obtebemos el id de la sesion actual y lo convertimos en int
                var claimsPrincipal = User;
                string idUsuarioString = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                int idUsuario = Convert.ToInt32(idUsuarioString);

                //Buscamos el usuario por el id que tiene la sesion iniciada
                Usuarios usuarioEncontrado = consultas.buscarUsuarioPorId(idUsuario);

                //Cargamos todas las publicaciones de ese usuario en una lista
                List<Publicaciones> listaPublicaciones = consultas.buscarPublicacionesPorIdUsuario(idUsuario);
                //Contamos el numero de publicaciones
                int numeroPublicacion = listaPublicaciones.Count;
                // Obtenemos el número de seguidores y seguidos
                int numeroSeguidores = consultas.ObtenerNumeroSeguidores(idUsuario);
                int numeroSeguidos = consultas.ObtenerNumeroSeguidos(idUsuario);

                //Mostramos todos los datos.
                ViewData["NumeroSeguidores"] = numeroSeguidores;
                ViewData["NumeroSeguidos"] = numeroSeguidos;
                ViewData["numeroPublicacion"] = numeroPublicacion;
                ViewData["listaPublicaciones"] = listaPublicaciones;

                return View("~/Views/PerfilUsuario/mostrarPerfilUsuario.cshtml", usuarioEncontrado);

            }catch (Exception ex)
            {
                Console.WriteLine("Se ha producido un error: {0}", ex);
                return View("~/Views/Errores/paginaError.cshtml");
            }
        }

        [HttpPost]
        public IActionResult EliminarPublicacion(int idPublicacion)
        {
            try
            {
                ServicioConsultas consultas = new ServicioConsultasImpl();
                consultas.eliminarPublicacion(idPublicacion);
                //Mostramos los datos del usuario 
                //Obtebemos el id de la sesion actual y lo convertimos en int
                var claimsPrincipal = User;
                string idUsuarioString = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                int idUsuario = Convert.ToInt32(idUsuarioString);

                //Buscamos el usuario por el id que tiene la sesion iniciada
                Usuarios usuarioEncontrado = consultas.buscarUsuarioPorId(idUsuario);

                //Cargamos todas las publicaciones de ese usuario en una lista
                List<Publicaciones> listaPublicaciones = consultas.buscarPublicacionesPorIdUsuario(idUsuario);
                //Contamos el numero de publicaciones
                int numeroPublicacion = listaPublicaciones.Count;
                // Obtenemos el número de seguidores y seguidos
                int numeroSeguidores = consultas.ObtenerNumeroSeguidores(idUsuario);
                int numeroSeguidos = consultas.ObtenerNumeroSeguidos(idUsuario);

                //Mostramos todos los datos.
                ViewData["NumeroSeguidores"] = numeroSeguidores;
                ViewData["NumeroSeguidos"] = numeroSeguidos;
                ViewData["numeroPublicacion"] = numeroPublicacion;
                ViewData["listaPublicaciones"] = listaPublicaciones;

                TempData["publicacionEliminada"] = "Publicación eliminada con éxito.";

                return View("~/Views/PerfilUsuario/mostrarPerfilUsuario.cshtml", usuarioEncontrado);
            }catch(Exception ex)
            {
                Console.WriteLine("Se ha producido un error: {0}", ex);
                return View("~/Views/Errores/paginaError.cshtml");
            }
        }
    }
}
