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
                List<Publicaciones> listaPublicaciones = consultas.mostrarPublicaciones();

                //Obtenemos el id del usuario que tiene la sesion iniciada.
                var claimsPrincipal = User;
                string idUsuario = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                // Verificar si el usuario ya dio me gusta a la publicación
                bool usuarioDioLike = consultas.usuarioDioLike(Convert.ToInt32(idUsuario), idPublicacion);

                //Si el usuario ya dio me gusta y vuelve a dar elimina el me gusta
                if (usuarioDioLike)
                {
                    // Si ya dio me gusta, eliminar el like
                    consultas.eliminarLike(Convert.ToInt32(idUsuario), idPublicacion);
                }
                else // Si no dio me gusta, añadir el nuevo like

                {
                    DateTime fchLike = DateTime.Now.ToUniversalTime();
                    LikeUsuariosPublicaciones nuevaLike = new LikeUsuariosPublicaciones(Convert.ToInt32(idUsuario), idPublicacion, fchLike);
                    consultas.añadirLike(nuevaLike);
                }

                // Obtener los IDs de todas las publicaciones para volver a mostrar la publicacion despues de dar mg o eliminarlo
                List<int> idsPublicaciones = listaPublicaciones.Select(p => p.idPublicacion).ToList();
                List<ComentarioConUsuarioViewModel> comentariosConUsuario = consultas.mostrarComentariosConUsuario();

                // Verificar si el usuario dio "me gusta" a cada publicación
                Dictionary<int, bool> likesPorPublicacion = new Dictionary<int, bool>();
                foreach (var idPublicaciones in idsPublicaciones)
                {
                    bool usuarioDioLikeActualizado = consultas.usuarioDioLike(Convert.ToInt32(idUsuario), idPublicaciones);
                    likesPorPublicacion.Add(idPublicaciones, usuarioDioLikeActualizado);
                }

                //Mostramos los datos
                ViewData["listaPublicaciones"] = listaPublicaciones;
                ViewData["likesPorPublicacion"] = likesPorPublicacion;
                ViewData["comentariosConUsuario"] = comentariosConUsuario;

                return View("~/Views/Home/PaginaInicio.cshtml");

            }catch(Exception ex)
            {
                Console.WriteLine("Se ha producido un error: {0}", ex);
                return View("~/Views/Errores/paginaError.cshtml");
            }
        }



    }
}
