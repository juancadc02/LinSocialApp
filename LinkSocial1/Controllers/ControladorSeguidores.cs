using DB.Modelo;
using LinkSocial1.DTO;
using LinkSocial1.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LinkSocial1.Controllers
{
    [Authorize] //Tiene que tener la sesion iniciada para navegar esta pagina.

    //Controlador para seguir al usuario que hemos buscado.
    public class ControladorSeguidores : Controller
    {

        /// <summary>
        /// Metodo que sigue al usuario que hemos buscado.
        /// Recibe el id del seguidor que queremos seguir.
        /// </summary>
        /// <param name="idSeguidorSeguido"></param>
        /// <returns></returns>
        public IActionResult solicitudSeguimiento(int idSeguidorSeguido)
        {
            try
            {
                ServicioConsultas consultas = new ServicioConsultasImpl();
                ServicioADto servicioADto = new ServicioADtoImpl();
                //Obtenemos el id del usuario que tiene la sesion iniciada.
                var claimsPrincipal = User;
                string idSeguidorSolicitud = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                //Cargamos la fecha actual en el momento de seguimiento.
                DateTime fchSeguimiento = DateTime.Now.ToUniversalTime();

                // Verificamos si se siguen o no.
                bool siguiendo = consultas.estaSiguiendo(Convert.ToInt32(idSeguidorSolicitud), idSeguidorSeguido);

                // Si ya está siguiendo, no realizar ninguna acción solo muestra los datos
                if (siguiendo)
                {
                    // Cargamos los datos del usuario.
                    Usuarios usuarioEncontrado = consultas.buscarUsuarioPorId(idSeguidorSeguido);
                    UsuariosDTO usuarioEncontradoDto = servicioADto.ConvertirDAOaDTOUsuarios(usuarioEncontrado);
                    List<Publicaciones> listaPublicaciones = consultas.buscarPublicacionesPorIdUsuario(idSeguidorSeguido);
                    List<PublicacionesDTO> listaPublicacionesDto = servicioADto.ConvertirListaDAOaDTOPublicaciones(listaPublicaciones);

                    int numeroPublicacion = listaPublicaciones.Count;
                    ViewData["numeroPublicacion"] = numeroPublicacion;
                    ViewData["listaPublicaciones"] = listaPublicacionesDto;
                    ViewData["EstaSiguiendo"] = siguiendo;
                    return View("~/Views/BuscarUsuarios/PerfilUsuarioBuscado.cshtml", usuarioEncontradoDto);
                }

                // Si no está siguiendo, realizar la acción de seguimiento
                Seguidores nuevoSeguidor = new Seguidores(Convert.ToInt32(idSeguidorSolicitud), Convert.ToInt32(idSeguidorSeguido), fchSeguimiento, true);
                consultas.iniciarSeguimiento(nuevoSeguidor);

                // Cargamos los datos del usuario.
                Usuarios usuarioEncontradoNuevo = consultas.buscarUsuarioPorId(idSeguidorSeguido);
                UsuariosDTO usuarioEncontradoNuevoDto = servicioADto.ConvertirDAOaDTOUsuarios(usuarioEncontradoNuevo);
                List<Publicaciones> listaPublicacionesNuevo = consultas.buscarPublicacionesPorIdUsuario(idSeguidorSeguido);
                List<PublicacionesDTO> listaPublicacionesNuevoDto = servicioADto.ConvertirListaDAOaDTOPublicaciones(listaPublicacionesNuevo);

                //Cargamos el numero de publicaciones del usuario
                int numeroPublicacionNuevo = listaPublicacionesNuevo.Count;
                // Obtenemos el número de seguidores y seguidos
                int numeroSeguidores = consultas.ObtenerNumeroSeguidores(idSeguidorSeguido);
                int numeroSeguidos = consultas.ObtenerNumeroSeguidos(idSeguidorSeguido);

                //Mostramos los datos en la vista
                ViewData["NumeroSeguidores"] = numeroSeguidores;
                ViewData["NumeroSeguidos"] = numeroSeguidos;
                ViewData["numeroPublicacion"] = numeroPublicacionNuevo;
                ViewData["listaPublicaciones"] = listaPublicacionesNuevoDto;
                ViewData["EstaSiguiendo"] = true;

                return View("~/Views/BuscarUsuarios/PerfilUsuarioBuscado.cshtml", usuarioEncontradoNuevoDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Se ha producido un error: {0}", ex);
                return View("~/Views/Errores/paginaError.cshtml");
            }
        }

        public IActionResult dejarDeSeguir(int idSeguidorSeguido)
        {
            try
            {
                ServicioConsultas consultas = new ServicioConsultasImpl();
                ServicioADto servicioADto = new ServicioADtoImpl();

                // Obtenemos el id del usuario que tiene la sesión iniciada.
                var claimsPrincipal = User;
                string idSeguidorSolicitud = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                // Verificamos si se siguen o no.
                bool siguiendo = consultas.estaSiguiendo(Convert.ToInt32(idSeguidorSolicitud), idSeguidorSeguido);

                // Si no está siguiendo, no realizar ninguna acción y mostrar los datos
                if (!siguiendo)
                {
                    // Cargamos los datos del usuario.
                    Usuarios usuarioEncontrado = consultas.buscarUsuarioPorId(idSeguidorSeguido);
                    UsuariosDTO usuarioEncontradoDto = servicioADto.ConvertirDAOaDTOUsuarios(usuarioEncontrado);
                    List<Publicaciones> listaPublicaciones = consultas.buscarPublicacionesPorIdUsuario(idSeguidorSeguido);
                    List<PublicacionesDTO> listaPublicacionesDto = servicioADto.ConvertirListaDAOaDTOPublicaciones(listaPublicaciones);

                    int numeroPublicacion = listaPublicaciones.Count;
                    ViewData["numeroPublicacion"] = numeroPublicacion;
                    ViewData["listaPublicaciones"] = listaPublicacionesDto;
                    ViewData["EstaSiguiendo"] = siguiendo;
                    return View("~/Views/BuscarUsuarios/PerfilUsuarioBuscado.cshtml", usuarioEncontradoDto);
                }

                // Si está siguiendo, realizar la acción de dejar de seguir
                consultas.dejarDeSeguir(Convert.ToInt32(idSeguidorSolicitud), idSeguidorSeguido);

                // Cargamos los datos del usuario.
                Usuarios usuarioEncontradoNuevo = consultas.buscarUsuarioPorId(idSeguidorSeguido);
                UsuariosDTO usuarioEncontradoNuevoDto = servicioADto.ConvertirDAOaDTOUsuarios(usuarioEncontradoNuevo);
                List<Publicaciones> listaPublicacionesNuevo = consultas.buscarPublicacionesPorIdUsuario(idSeguidorSeguido);
                List<PublicacionesDTO> listaPublicacionesNuevoDto = servicioADto.ConvertirListaDAOaDTOPublicaciones(listaPublicacionesNuevo);

                // Cargamos el número de publicaciones del usuario
                int numeroPublicacionNuevo = listaPublicacionesNuevo.Count;
                // Obtenemos el número de seguidores y seguidos
                int numeroSeguidores = consultas.ObtenerNumeroSeguidores(idSeguidorSeguido);
                int numeroSeguidos = consultas.ObtenerNumeroSeguidos(idSeguidorSeguido);

                // Mostramos los datos en la vista
                ViewData["NumeroSeguidores"] = numeroSeguidores;
                ViewData["NumeroSeguidos"] = numeroSeguidos;
                ViewData["numeroPublicacion"] = numeroPublicacionNuevo;
                ViewData["listaPublicaciones"] = listaPublicacionesNuevoDto;
                ViewData["EstaSiguiendo"] = false;

                return View("~/Views/BuscarUsuarios/PerfilUsuarioBuscado.cshtml", usuarioEncontradoNuevoDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Se ha producido un error: {0}", ex);
                return View("~/Views/Errores/paginaError.cshtml");
            }
        }

    }
}
