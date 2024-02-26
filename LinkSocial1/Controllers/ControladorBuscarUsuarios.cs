using DB.Modelo;
using LinkSocial1.DTO;
using LinkSocial1.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LinkSocial1.Controllers
{
    [Authorize] //Tiene que tener la sesion iniciada para navegar esta pagina.
    public class ControladorBuscarUsuarios : Controller
    {
        /// <summary>
        /// Cargamos la pagina para buscar usuarios y controlamos los errores
        /// </summary>
        /// <returns></returns>
        public IActionResult irABuscarUsuarios()
        {
            try
            {
                ServicioConsultas consultas = new ServicioConsultasImpl();
                consultas.log("Entrado en metodo que abre la vista para buscar un usuario");
                return View("~/Views/BuscarUsuarios/PaginaBuscarUsuarios.cshtml");
            }catch (Exception ex)
            {
                Console.WriteLine("Se ha producido un error: {0}", ex);
                return View("~/Views/Errores/paginaError.cshtml");
            }
        }

        /// <summary>
        /// Metodo que busca en la base de datos al usuario por el correo electronico
        /// Recibe el correo electronico del formulario.
        /// </summary>
        /// <param name="correoElectronico"></param>
        /// <returns></returns>
        public IActionResult buscarUsuario(string correoElectronico)
        {
            try
            {
                ServicioConsultas consultas = new ServicioConsultasImpl();
                consultas.log("Entrado en metodo que busca un usuario por el correo electronico.");

                ServicioADto servicioADto = new ServicioADtoImpl();

                //Buscamos el usuarios introduciendo el correo electronico en el formulario
                Usuarios usuarioEncontrado = consultas.buscarUsuario(correoElectronico);

               

                //Si encuentra al usuario cargamos la vista y le pasamos el usuario.
                if (usuarioEncontrado != null)
                {
                    //Pasamos le usuario a DTO
                    UsuariosDTO usuarioEncontradoDto = servicioADto.ConvertirDAOaDTOUsuarios(usuarioEncontrado);
                    string nombreUsuario = usuarioEncontradoDto.nombreCompleto;
                    return View("~/Views/BuscarUsuarios/PaginaBuscarUsuarios.cshtml", usuarioEncontradoDto);
                }
                else //Si no encuentra al usuario muestra el mensaje de errores 
                {
                    TempData["mensajeError"] = "Usuario no encontrado";
                    return View("~/Views/BuscarUsuarios/PaginaBuscarUsuarios.cshtml");
                }
            }catch (Exception ex)
            {
                Console.WriteLine("Se ha producido un error: {0}", ex);
                return View("~/Views/Errores/paginaError.cshtml");
            }

        }

        /// <summary>
        /// Cargamos el perfil del usuario que hemos buscado.
        /// Le pasamos el id del usuario cuando pulsamos en el para ir a su perfil.
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult perfilUsuarioBuscado(int idUsuario)
        {
            try
            {
                ServicioConsultas consultas = new ServicioConsultasImpl();
                consultas.log("Entrado en metodo que carga una vista con los datos del usuario que hemos buscado");
                ServicioADto servicioADto = new ServicioADtoImpl();
                //Obtenemos el id del usuario que tiene la sesion inciaca
                var claimsPrincipal = User;
                string idUsuarioString = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                //Cargamos los datos del usuario a traves del su id
                Usuarios usuarioEncontrado = consultas.buscarUsuarioPorId(idUsuario);
                //Pasamos el usuario a DTO
                UsuariosDTO usuarioEncontradoDTO=servicioADto.ConvertirDAOaDTOUsuarios(usuarioEncontrado);
                //Cargamos las publicaciones del usuario que hemos buscado a traves del id.
                List<Publicaciones> listaPublicaciones = consultas.buscarPublicacionesPorIdUsuario(idUsuario);
                //Pasamos la lista a DTO
                List<PublicacionesDTO> listaPublicacionesDto = servicioADto.ConvertirListaDAOaDTOPublicaciones(listaPublicaciones);
                //Cargamos el numero de imagenes que tiene el usuario
                int numeroPublicacion = listaPublicaciones.Count;
               
                // Obtebemos el número de seguidores y seguidos
                int numeroSeguidores = consultas.ObtenerNumeroSeguidores(idUsuario);
                int numeroSeguidos = consultas.ObtenerNumeroSeguidos(idUsuario);

                //Mostramos todos los datos en la vista.
                ViewData["NumeroSeguidores"] = numeroSeguidores;
                ViewData["NumeroSeguidos"] = numeroSeguidos;
                ViewData["numeroPublicacion"] = numeroPublicacion;
                ViewData["listaPublicaciones"] = listaPublicacionesDto;

                // Comprobamos si el usuario que tiene la sesion iniciada sigue al usuario para no mostrar el boton de seguir.
                bool? estaSiguiendo = consultas.estaSiguiendo(Convert.ToInt32(idUsuarioString), idUsuario);
                ViewData["EstaSiguiendo"] = estaSiguiendo;

                return View("~/Views/BuscarUsuarios/PerfilUsuarioBuscado.cshtml", usuarioEncontradoDTO);
            }catch(Exception ex)
            {
                Console.WriteLine("Se ha producido un error: {0}", ex);
                return View("~/Views/Errores/paginaError.cshtml");
            }
        }

    }
}
