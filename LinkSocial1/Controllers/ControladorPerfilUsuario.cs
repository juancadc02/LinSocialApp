using DB.Modelo;
using LinkSocial1.DTO;
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
                ServicioADto aDto = new ServicioADtoImpl();

                consultas.log("Entrando en el metodo que carga el perfil del usuario que tiene la sesion iniciada");
                //Obtebemos el id de la sesion actual y lo convertimos en int
                var claimsPrincipal = User;
                string idUsuarioString = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                int idUsuario = Convert.ToInt32(idUsuarioString);

                //Buscamos el usuario por el id que tiene la sesion iniciada
                Usuarios usuarioEncontrado = consultas.buscarUsuarioPorId(idUsuario);
                UsuariosDTO usuarioDto = aDto.ConvertirDAOaDTOUsuarios(usuarioEncontrado);
                //Cargamos todas las publicaciones de ese usuario en una lista
                List<Publicaciones> listaPublicaciones = consultas.buscarPublicacionesPorIdUsuario(idUsuario);
                //Contamos el numero de publicaciones
                List<PublicacionesDTO> listaPublicacionesDto = aDto.ConvertirListaDAOaDTOPublicaciones(listaPublicaciones);

                int numeroPublicacion = listaPublicaciones.Count;
                // Obtenemos el número de seguidores y seguidos
                int numeroSeguidores = consultas.ObtenerNumeroSeguidores(idUsuario);
                int numeroSeguidos = consultas.ObtenerNumeroSeguidos(idUsuario);

                //Mostramos todos los datos.
                ViewData["NumeroSeguidores"] = numeroSeguidores;
                ViewData["NumeroSeguidos"] = numeroSeguidos;
                ViewData["numeroPublicacion"] = numeroPublicacion;
                ViewData["listaPublicacionesDto"] = listaPublicacionesDto;

                return View("~/Views/PerfilUsuario/mostrarPerfilUsuario.cshtml", usuarioDto);

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
                ServicioADto aDto = new ServicioADtoImpl();
                consultas.log("Entrando en el metodo que elimina una publicacion del usuario");
                consultas.eliminarPublicacion(idPublicacion);
                //Mostramos los datos del usuario 
                //Obtebemos el id de la sesion actual y lo convertimos en int
                var claimsPrincipal = User;
                string idUsuarioString = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                int idUsuario = Convert.ToInt32(idUsuarioString);

                //Buscamos el usuario por el id que tiene la sesion iniciada
                Usuarios usuarioEncontrado = consultas.buscarUsuarioPorId(idUsuario);

                UsuariosDTO usuarioDto = aDto.ConvertirDAOaDTOUsuarios(usuarioEncontrado);
                //Cargamos todas las publicaciones de ese usuario en una lista
                List<Publicaciones> listaPublicaciones = consultas.buscarPublicacionesPorIdUsuario(idUsuario);

                List<PublicacionesDTO> listaPublicacionesDto =aDto.ConvertirListaDAOaDTOPublicaciones(listaPublicaciones);

                //Contamos el numero de publicaciones
                int numeroPublicacion = listaPublicaciones.Count;
                // Obtenemos el número de seguidores y seguidos
                int numeroSeguidores = consultas.ObtenerNumeroSeguidores(idUsuario);
                int numeroSeguidos = consultas.ObtenerNumeroSeguidos(idUsuario);

                //Mostramos todos los datos.
                ViewData["NumeroSeguidores"] = numeroSeguidores;
                ViewData["NumeroSeguidos"] = numeroSeguidos;
                ViewData["numeroPublicacion"] = numeroPublicacion;
                ViewData["listaPublicacionesDto"] = listaPublicacionesDto;

                TempData["publicacionEliminada"] = "Publicación eliminada con éxito.";

                return View("~/Views/PerfilUsuario/mostrarPerfilUsuario.cshtml", usuarioDto);
            }catch(Exception ex)
            {
                Console.WriteLine("Se ha producido un error: {0}", ex);
                return View("~/Views/Errores/paginaError.cshtml");
            }
        }
    }
}
