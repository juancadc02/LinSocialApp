using DB.Modelo;
using LinkSocial1.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LinkSocial1.Controllers
{
    [Authorize] //Tiene que tener la sesion iniciada para navegar esta pagina.
    public class ControladorSubirPublicaciones : Controller
    {

        /// <summary>
        /// Cargamos la pagina para subir publicaciones y controlamos los posibles errores.
        /// </summary>
        /// <returns></returns>
        public IActionResult cargarPaginaSubirPublicaciones()
        {
            try
            {
                return View("~/Views/Publicaciones/subirPublicaciones.cshtml");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Se ha producido un error: {0}", ex);
                return View("~/Views/Errores/paginaError.cshtml");
            }
        }

        /// <summary>
        /// Metodo que guarda en la base de datos una publicacion con todos los datos recibidos a traves del formulario.
        /// //Recibe la foto a traves del formulario 
        /// </summary>
        /// <param name="imagen"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult guardarPublicacion(IFormFile imagen,string pieDeFoto)
        {
            try
            {
                ServicioConsultas consulta = new ServicioConsultasImpl();
                consulta.log("Entrando en el metodo que guarda una publicacion en la base de datos. ");

                string nombreImagen;
                string rutaImagen = "";

                //Obtenemos el id del usuario que tiene la sesion iniciada (el que va a subir la publicacion).
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

                    //Fecha actual.
                    DateTime fchPublicacion = DateTime.Now.ToUniversalTime();
                    //Creamos la publicacion
                    Publicaciones nuevaPublicacion = new Publicaciones(idUsuario, fchPublicacion, rutaImagen,pieDeFoto);

                    consulta.subirPublicacion(nuevaPublicacion);

                    TempData["ExitoMensaje"] = "Publicacion subida con exito.";
                    return RedirectToAction("cargarPaginaInicio", "ControladorPaginaInicio");
                }
                else
                {
                    // La conversión falló, maneja el escenario donde la cadena no es un entero válido
                    return View("~/Views/Errores/paginaError.cshtml");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Se ha producido un error: {0}", ex);
                return View("~/Views/Errores/paginaError.cshtml");
            }
        }

        /// <summary>
        /// Metodo para poder añadir comentarios en la publicacion.
        /// Recibe el id de la publicacion y el comentario.
        /// </summary>
        /// <param name="idPublicacion"></param>
        /// <param name="contenidoComentario"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult añadirComentario(string idPublicacion, string contenidoComentario)
        {
            try
            {
                ServicioConsultas consultas = new ServicioConsultasImpl();
                consultas.log("Entrando en el metodo que añade una comentario en una publicacion y guarda el registro en la base de datos. ");

                //Obtenemos el id del usuario que tiene la sesion iniciada.
                var claimsPrincipal = User;
                string idUsuarioString = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                //Cargamos la fecha actual
                DateTime fchComentario = DateTime.Now.ToUniversalTime();

                //Creamos el nuevo comentario.
                Comentarios nuevoComentario = new Comentarios(Convert.ToInt32(idUsuarioString), Convert.ToInt32(idPublicacion), contenidoComentario, fchComentario);
                consultas.añadirComentario(nuevoComentario);
                return RedirectToAction("cargarPaginaInicio", "ControladorPaginaInicio");

            }catch (Exception ex)
            {
                Console.WriteLine("Se ha producido un error: {0}", ex);
                return View("~/Views/Errores/paginaError.cshtml");
            }
        }




    }
}
