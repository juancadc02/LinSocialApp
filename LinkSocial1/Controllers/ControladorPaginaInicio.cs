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

                // Usar el nuevo método para obtener comentarios con usuarios
                List<ComentarioConUsuarioViewModel> comentariosConUsuario = consultas.mostrarComentariosConUsuario();

                // Obtener el ID del usuario actual
                var claimsPrincipal = User;
                string idUsuario = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                // Verificar si el usuario dio "me gusta" a cada publicación
                foreach (var publicacion in listaPublicaciones)
                {
                    publicacion.UsuarioDioLike = consultas.usuarioDioLike(Convert.ToInt32(idUsuario), publicacion.Id);
                }

                ViewData["listaPublicaciones"] = listaPublicaciones;
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
