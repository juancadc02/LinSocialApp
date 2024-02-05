using DB;
using DB.Modelo;
using LinkSocial1.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LinkSocial1.Controllers
{
    [Authorize(Roles = "admin")]
    public class ControladorAdministracion : Controller
    {
        private readonly GestorLinkSocialDbContext _contexto;

        /// <summary>
        /// Contructor con el contexto.
        /// </summary>
        /// <param name="contexto"></param>
        public ControladorAdministracion(GestorLinkSocialDbContext contexto)
        {
            _contexto = contexto;
        }

        /// <summary>
        /// Metod encargado de cargar la pagina de administracion.
        /// </summary>
        /// <returns></returns>
        public IActionResult irPaginaAdmin()
        {
            try
            {
                return View("~/Views/Administracion/PaginaAdministracion.cshtml");
            }catch (Exception ex)
            {
                Console.WriteLine("Se ha producido un error: {0}", ex);
                return View("~/Views/Errores/paginaError.cshtml");
            }
        }

        /// <summary>
        /// Metod encargado de añadir a una lista de usuario a todos los usuario de la base de datos, cargar una vista de listaUsuarios y mostrar en ella todos los usuarios.
        /// </summary>
        /// <returns></returns>
        public IActionResult irUsuario()
        {
            try
            {
                ServicioConsultas consultas = new ServicioConsultasImpl();
                //Creamos una lista y llamamos al metodo lista usuarios.
                List<Usuarios> listaUsuario = consultas.mostrarUsuarios();
                ViewData["listaUsuario"] = listaUsuario;
                return View("~/Views/Administracion/listaUsuario.cshtml");
            }catch(Exception ex)
            {
                Console.WriteLine("Se ha producido un error: {0}", ex);
                return View("~/Views/Errores/paginaError.cshtml");
            }
        }

        /// <summary>
        /// Metodo encargado de mandar el id del usuario seleccionado a traves de la url y cargar una vista con los datos de ese usuario.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult EditarUsuario(int id)
        {
            try
            {
                // Obtén el usuario de la base de datos usando el ID
                var usuario = _contexto.Usuarios.Find(id);

                // Verifica si el usuario existe
                if (usuario == null)
                {
                    return NotFound(); // Puedes manejar la situación en la que el usuario no se encuentra
                }
                return View("~/Views/Administracion/paginaEdicionUsuario.cshtml", usuario);

            }catch(Exception ex)
            {
                Console.WriteLine("Se ha producido un error: {0}", ex);
                return View("~/Views/Errores/paginaError.cshtml");
            }
        }

        /// <summary>
        /// Metodo encargado de guardar las modificaciones que se le han echo al usuario en la base de datos.
        /// </summary>
        /// <param name="usuarioEditado"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult GuardarEdicion(Usuarios usuarioEditado,IFormFile imagen)
        {
            try
            {
                if (imagen != null && imagen.Length > 0)
                {
                    string nombreImagen = Guid.NewGuid().ToString() + Path.GetExtension(imagen.FileName);
                    string rutaCompleta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "imagenes/usuarios", nombreImagen);
                    using (var stream = new FileStream(rutaCompleta, FileMode.Create))
                    {
                        imagen.CopyTo(stream);
                    }
                    usuarioEditado.rutaImagen = "/imagenes/usuarios/" + nombreImagen;
                }
                // Actualiza el estado del usuario en la base de datos
                _contexto.Update(usuarioEditado);
                _contexto.SaveChanges();

                TempData["SuccessMessage"] = "Usuario editado exitosamente";
                return RedirectToAction("irUsuario"); // Redirige a la lista de usuarios u otra vista apropiada
            }catch (Exception ex)
            {
                Console.WriteLine("Se ha producido un error: {0}", ex);
                return View("~/Views/Errores/paginaError.cshtml");
            }
        }

        /// <summary>
        /// Metodo encargado de eliminar a un usuario de la base de datos a traves del id.
        /// Comprobamos si el usuario es admin y en el caso de que sea admin si es el ultimo usuario admin no me dejara eliminarlos, mostrado un mensaje en pantalla.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult EliminarUsuario(int id)
        {
            try
            {
                var usuarioExistente = _contexto.Usuarios.Find(id);

                if (usuarioExistente == null)
                {
                    return NotFound();
                }

                try
                {
                    // Verificamos si el usuario a eliminar es el último admin
                    if (usuarioExistente.rolAcceso == "admin" && _contexto.Usuarios.Count(u => u.rolAcceso == "admin") == 1)
                    {
                        // Si es el último admin, mostramos un mensaje de error
                        TempData["ErrorMessage"] = "No puedes eliminar al último usuario con rol de administrador.";
                        return RedirectToAction("irUsuario");
                    }

                    // Borramos el usuario y guardamos los cambios 
                    _contexto.Usuarios.Remove(usuarioExistente);
                    _contexto.SaveChanges();

                    // Mostramos mensaje de éxito y redirigimos al listado
                    TempData["SuccessMessage"] = "El usuario se ha eliminado correctamente.";
                    return RedirectToAction("irUsuario");
                }
                catch (Exception ex)
                {
                    // Mostramos error en la consola y redirigimos a la administración.
                    Console.WriteLine(ex.Message);
                    return RedirectToAction("irPaginaAdmin");
                }
            }catch (Exception ex)
            {
                Console.WriteLine("Se ha producido un error: {0}", ex);
                return View("~/Views/Errores/paginaError.cshtml");
            }
        }



       



    }


}
