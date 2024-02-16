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
            ServicioConsultas consultas = new ServicioConsultasImpl();
            List<Publicaciones> listaPublicaciones = consultas.mostrarPublicaciones();

         

        
            var claimsPrincipal = User;
            string idUsuario = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            DateTime fchLike = DateTime.Now.ToUniversalTime();
            LikeUsuariosPublicaciones nuevaLike = new LikeUsuariosPublicaciones(Convert.ToInt32(idUsuario), idPublicacion, fchLike);
            consultas.añadirLike(nuevaLike);

            // Obtener los IDs de todas las publicaciones
            List<int> idsPublicaciones = listaPublicaciones.Select(p => p.idPublicacion).ToList();
            List<ComentarioConUsuarioViewModel> comentariosConUsuario = consultas.mostrarComentariosConUsuario();

            // Verificar si el usuario dio "me gusta" a cada publicación
            Dictionary<int, bool> likesPorPublicacion = new Dictionary<int, bool>();
            foreach (var idPublicaciones in idsPublicaciones)
            {
                bool usuarioDioLike = consultas.usuarioDioLike(Convert.ToInt32(idUsuario), idPublicacion);
                likesPorPublicacion.Add(idPublicacion, usuarioDioLike);
            }

            ViewData["listaPublicaciones"] = listaPublicaciones;
            ViewData["likesPorPublicacion"] = likesPorPublicacion;
            ViewData["comentariosConUsuario"] = comentariosConUsuario;




            return View("~/Views/Home/PaginaInicio.cshtml");

        }


    }
}
