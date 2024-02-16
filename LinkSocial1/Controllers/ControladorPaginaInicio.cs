using DB.Modelo;
using LinkSocial1.DTO;
using LinkSocial1.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static LinkSocial1.Servicios.ServicioConsultasImpl;

namespace LinkSocial1.Controllers
{
    [Authorize]
    public class ControladorPaginaInicio : Controller
    {
        /// <summary>
        /// Metodo encargado de devolver la pagina de inicio.
        /// </summary>
        /// <returns></returns>
        public ActionResult cargarPaginaInicio()
        {
            try
            {
                ServicioConsultas consultas = new ServicioConsultasImpl();
                List<Publicaciones> listaPublicaciones = consultas.mostrarPublicaciones();

                // Obtener el ID del usuario actual
                var claimsPrincipal = User;
                string idUsuario = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                // Obtener los IDs de todas las publicaciones
                List<int> idsPublicaciones = listaPublicaciones.Select(p => p.idPublicacion).ToList();
                List<ComentarioConUsuarioViewModel> comentariosConUsuario = consultas.mostrarComentariosConUsuario();

                // Verificar si el usuario dio "me gusta" a cada publicación
                Dictionary<int, bool> likesPorPublicacion = new Dictionary<int, bool>();
                foreach (var idPublicacion in idsPublicaciones)
                {
                    bool usuarioDioLike = consultas.usuarioDioLike(Convert.ToInt32(idUsuario), idPublicacion);
                    likesPorPublicacion.Add(idPublicacion, usuarioDioLike);
                }

                ViewData["listaPublicaciones"] = listaPublicaciones;
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
