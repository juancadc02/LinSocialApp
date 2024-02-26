using DB.Modelo;
using LinkSocial1.DTO;
using LinkSocial1.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static LinkSocial1.Servicios.ServicioConsultasImpl;

namespace LinkSocial1.Controllers
{
    [Authorize] //Tiene que estar autentificado para estar en esta pagina.
    public class ControladorPaginaInicio : Controller
    {
        /// <summary>
        /// Metodo encargado de devolver la pagina de inicio y cargar todo su contenido.
        /// </summary>
        /// <returns></returns>
        public ActionResult cargarPaginaInicio()
        {
            try
            {
                ServicioConsultas consultas = new ServicioConsultasImpl();
                ServicioADto servicioADto = new ServicioADtoImpl();
                consultas.log("Entrando en el metodo que carga la pagina de inicio");
                // Obtenemos el id del usuario que ha iniciado sesión.
                var claimsPrincipal = User;
                string idUsuario = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                // Cargamos en la lista todas las publicaciones de todos los usuarios.
                List<Publicaciones> listaPublicaciones = consultas.mostrarPublicaciones();

                // Obtener los IDs de todas las publicaciones
                List<int> idsPublicaciones = listaPublicaciones.Select(p => p.idPublicacion).ToList();

                // Verificar si el usuario dio me gusta a cada publicación
                Dictionary<int, bool> likesPorPublicacion = new Dictionary<int, bool>();
                foreach (var idPublicacion in idsPublicaciones)
                {
                    bool usuarioDioLike = consultas.usuarioDioLike(Convert.ToInt32(idUsuario), idPublicacion);
                    likesPorPublicacion.Add(idPublicacion, usuarioDioLike);
                }

                // Cargamos en una lista todos los comentarios de las publicaciones con el usuario que ha puesto el comentario.
                List<ComentarioConUsuarioViewModel> comentariosConUsuario = consultas.mostrarComentariosConUsuario();

                // Pasamos los datos a DTO antes de mostrarlos y cargamos el usuario asociado a cada publicación
                List<PublicacionesDTO> listaPublicacionesDTO = new List<PublicacionesDTO>();
                foreach (var publicacion in listaPublicaciones)
                {
                    PublicacionesDTO publicacionDTO = servicioADto.ConvertirDAOaDTOPublicaciones(publicacion);
                    publicacionDTO.usuarios = consultas.buscarUsuarioPorId(publicacionDTO.idUsuario); // Ajusta el nombre del método según tu implementación
                    listaPublicacionesDTO.Add(publicacionDTO);
                }

                // Pasamos toda la información a la vista.
                ViewData["listaPublicaciones"] = listaPublicacionesDTO;
                ViewData["likesPorPublicacion"] = likesPorPublicacion;
                ViewData["comentariosConUsuario"] = comentariosConUsuario;

                return View("~/Views/Home/PaginaInicio.cshtml");
            }
            // Si se produce algún error, muestra la vista de errores.
            catch (Exception ex)
            {
                Console.WriteLine("Se ha producido un error: {0}", ex);
                return View("~/Views/Errores/paginaError.cshtml");
            }
        }



    }
}
