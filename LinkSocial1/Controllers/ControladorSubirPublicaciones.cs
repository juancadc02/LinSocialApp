using DB.Modelo;
using LinkSocial1.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LinkSocial1.Controllers
{
    [Authorize]
    public class ControladorSubirPublicaciones : Controller
    {

        public IActionResult cargarPaginaSubirPublicaciones()
        {
            
            return View("~/Views/Publicaciones/subirPublicaciones.cshtml");
        }

        [HttpPost]
        public IActionResult guardarPublicacion(IFormFile imagen)
        {
            ServicioConsultas consulta = new ServicioConsultasImpl();
            string nombreImagen;
            string rutaImagen = "";

            var claimsPrincipal = User;

            string idUsuarioString = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Intenta convertir la cadena a un entero utilizando int.TryParse
            if (int.TryParse(idUsuarioString, out int idUsuario))
            {
                if (imagen != null && imagen.Length > 0)
                {
                    nombreImagen = Guid.NewGuid().ToString() + Path.GetExtension(imagen.FileName);
                    string rutaCompleta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "imagenes/publicaciones", nombreImagen);
                    using (var stream = new FileStream(rutaCompleta, FileMode.Create))
                    {
                        imagen.CopyTo(stream);
                    }
                    rutaImagen = "/imagenes/publicaciones/" + nombreImagen;
                }

                DateTime fchPublicacion = DateTime.Now.ToUniversalTime();
                Publicaciones nuevaPublicacion = new Publicaciones(idUsuario, fchPublicacion, rutaImagen);

                consulta.subirPublicacion(nuevaPublicacion);
                return RedirectToAction("cargarPaginaInicio", "ControladorPaginaInicio");
            }
            else
            {
                // La conversión falló, maneja el escenario donde la cadena no es un entero válido
                // Puedes redirigir a una página de error o realizar alguna otra acción
                return RedirectToAction("Error");
            }
        }

        [HttpPost]
        public IActionResult añadirComentario(string idPublicacion, string contenidoComentario)
        {
            ServicioConsultas consultas = new ServicioConsultasImpl();
            var claimsPrincipal = User;
            string idUsuarioString = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
           
            DateTime fchComentario = DateTime.Now.ToUniversalTime();

            Comentarios nuevoComentario = new Comentarios(Convert.ToInt32(idUsuarioString),Convert.ToInt32(idPublicacion),contenidoComentario, fchComentario);
            consultas.añadirComentario(nuevoComentario);
            return RedirectToAction("cargarPaginaInicio", "ControladorPaginaInicio");

        }




    }
}
