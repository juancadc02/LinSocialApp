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
            ServicioConsultas consulta = new ServicioConsultasImpl();
            ServicioConsultas consultas = new ServicioConsultasImpl();
            List<Publicaciones> listaPublicaciones = consultas.mostrarPublicaciones();

            // Usar el nuevo método para obtener comentarios con usuarios
            List<ComentarioConUsuarioViewModel> comentariosConUsuario = consultas.mostrarComentariosConUsuario();

        
            var claimsPrincipal = User;
            string idUsuario = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            DateTime fchLike = DateTime.Now.ToUniversalTime();
            LikeUsuariosPublicaciones nuevaLike = new LikeUsuariosPublicaciones(Convert.ToInt32(idUsuario), idPublicacion, fchLike);
            consulta.añadirLike(nuevaLike);
            bool usuarioDioLike = consulta.usuarioDioLike(Convert.ToInt32(idUsuario), idPublicacion);
            ViewData["listaPublicaciones"] = listaPublicaciones;
            ViewData["comentariosConUsuario"] = comentariosConUsuario;
            ViewData["usuarioDioLike"] = usuarioDioLike;

            return View("~/Views/Home/PaginaInicio.cshtml");

        }


    }
}
