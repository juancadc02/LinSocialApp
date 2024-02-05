using DB.Modelo;
using LinkSocial1.Servicios;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LinkSocial1.Controllers
{
    public class ControladorSubirPublicaciones : Controller
    {
        private readonly UserManager<Usuarios> _userManager;

        public ControladorSubirPublicaciones(UserManager<Usuarios> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult cargarPaginaSubirPublicaciones()
        {
            return View("~/Views/Publicaciones/subirPublicaciones.cshtml");
        }

        [HttpPost]
        public IActionResult guardarPublicacion (int idUsuario, IFormFile imagen)
        {
            ServicioConsultas consulta= new ServicioConsultasImpl();
            var idUsuarioSesion = _userManager.GetUserId(User);

            string nombreImagen;
            string rutaImagen = "";
            if (imagen != null && imagen.Length > 0)
            {
                nombreImagen = Guid.NewGuid().ToString() + Path.GetExtension(imagen.FileName);
                string rutaCompleta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "imagenes/publicaciones", nombreImagen);
                using (var stream = new FileStream(rutaCompleta, FileMode.Create))
                {
                    imagen.CopyTo(stream);
                }
                rutaImagen = "/imagenes/usuarios/" + nombreImagen;
            }
            DateTime fchPublicacion = DateTime.Now.ToUniversalTime();
            Publicaciones nuevaPublicacion = new Publicaciones(idUsuario,fchPublicacion,rutaImagen);

            consulta.subirPublicacion(nuevaPublicacion);
            return RedirectToAction("cargarPaginaInicio", "ControladorPaginaInicio");
        }
    }
}
