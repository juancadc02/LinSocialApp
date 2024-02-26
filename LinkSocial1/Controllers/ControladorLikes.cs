using DB.Modelo;
using LinkSocial1.DTO;
using LinkSocial1.Servicios;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LinkSocial1.Controllers
{
    public class ControladorLikes : Controller
    {

        public IActionResult darLikePublicacion(int idPublicacion)
        {
            try
            {
                ServicioConsultas consultas = new ServicioConsultasImpl();
                consultas.log("Entrando en el metodo para dar me gusta a una publicacion");
                ServicioADto servicioADto = new ServicioADtoImpl();

                // Obtener el id del usuario que tiene la sesión iniciada.
                var claimsPrincipal = User;
                string idUsuario = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                // Verificar si el usuario ya dio me gusta a la publicación
                bool usuarioDioLike = consultas.usuarioDioLike(Convert.ToInt32(idUsuario), idPublicacion);

                // Si el usuario ya dio me gusta y vuelve a dar, elimina el me gusta
                if (usuarioDioLike)
                {
                    // Si ya dio me gusta, eliminar el like
                    consultas.eliminarLike(Convert.ToInt32(idUsuario), idPublicacion);
                    TempData["eliminarMeGusta"] = "Has quitado el me gusta a la publicación";
                }
                else // Si no dio me gusta, añadir el nuevo like
                {
                    DateTime fchLike = DateTime.Now.ToUniversalTime();
                    LikeUsuariosPublicaciones nuevaLike = new LikeUsuariosPublicaciones(Convert.ToInt32(idUsuario), idPublicacion, fchLike);
                    consultas.añadirLike(nuevaLike);
                    TempData["darMeGusta"] = "Has dado me gusta a la publicación";
                }

                // Recargar la información actualizada después de dar me gusta o eliminarlo
                List<Publicaciones> listaPublicaciones = consultas.mostrarPublicaciones();
                List<PublicacionesDTO> listaPublicacionesDto = servicioADto.ConvertirListaDAOaDTOPublicaciones(listaPublicaciones);

                // Cargar el usuario asociado a cada publicación
                foreach (var publicacionDto in listaPublicacionesDto)
                {
                    publicacionDto.usuarios = consultas.buscarUsuarioPorId(publicacionDto.idUsuario); // Ajusta el nombre del método según tu implementación
                }

                // Obtener los IDs de todas las publicaciones
                List<int> idsPublicaciones = listaPublicacionesDto.Select(p => p.idPublicacion).ToList();
                List<ComentarioConUsuarioViewModel> comentariosConUsuario = consultas.mostrarComentariosConUsuario();

                // Verificar si el usuario dio "me gusta" a cada publicación
                Dictionary<int, bool> likesPorPublicacion = new Dictionary<int, bool>();
                foreach (var idPublicaciones in idsPublicaciones)
                {
                    bool usuarioDioLikeActualizado = consultas.usuarioDioLike(Convert.ToInt32(idUsuario), idPublicaciones);
                    likesPorPublicacion.Add(idPublicaciones, usuarioDioLikeActualizado);
                }

                // Mostramos los datos actualizados en la vista
                ViewData["listaPublicaciones"] = listaPublicacionesDto;
                ViewData["likesPorPublicacion"] = likesPorPublicacion;
                ViewData["comentariosConUsuario"] = comentariosConUsuario;

                return View("~/Views/Home/PaginaInicio.cshtml");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Se ha producido un error: {0}", ex);
                return View("~/Views/Errores/paginaError.cshtml");
            }
        }



    }
}
