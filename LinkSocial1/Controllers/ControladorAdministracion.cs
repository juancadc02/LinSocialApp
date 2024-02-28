using DB;
using DB.Modelo;
using LinkSocial1.DTO;
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
                ServicioConsultas consulta = new ServicioConsultasImpl();
                consulta.log("Entrado en metodo que carga la vista de pagina administracion");
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
                consultas.log("Entrado en metodo que carga el listado de usuarios");
                ServicioADto servicioADto = new ServicioADtoImpl();
                //Creamos una lista y llamamos al metodo lista usuarios.
                List<Usuarios> listaUsuario = consultas.mostrarUsuarios();
                List<UsuariosDTO> listaUsuarioDto =servicioADto.ConvertirListaDAOaDTOUsuarios(listaUsuario);
                ViewData["listaUsuario"] = listaUsuarioDto;

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
                ServicioConsultas consultas = new ServicioConsultasImpl();
                consultas.log("Entrado en metodo que carga el formulario para editar un usuario.");
                ServicioADto servicioADto=new ServicioADtoImpl();
                // Obtén el usuario de la base de datos usando el ID
                Usuarios usuario = _contexto.Usuarios.Find(id);

                UsuariosDTO usuarioDto = servicioADto.ConvertirDAOaDTOUsuarios(usuario);
                // Verifica si el usuario existe
                if (usuarioDto == null)
                {
                    return NotFound(); // Puedes manejar la situación en la que el usuario no se encuentra
                }
                return View("~/Views/Administracion/paginaEdicionUsuario.cshtml", usuarioDto);

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
                ServicioConsultas consultas = new ServicioConsultasImpl();
                consultas.log("Entrado en metodo que guarda un usuario editado.");
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
                usuarioEditado.correoConfirmado = true;
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
                ServicioConsultas consultas = new ServicioConsultasImpl();
                consultas.log("Entrado en metodo que elimina un usuario.");
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

        /// <summary>
        /// Metodo que carga la vista con el formulario para que los administradores puedan crear usuarios.
        /// </summary>
        /// <returns></returns>
        public IActionResult irAAñadirUsuario()
        {
            try
            {
                ServicioConsultas consultas = new ServicioConsultasImpl();
                consultas.log("Entrado en metodo abre el formulario para añadir un usuario desde admin.");
                return View("~/Views/Administracion/añadirUsuario.cshtml");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Se ha producido un error: {0}", ex);
                return View("~/Views/Errores/paginaError.cshtml");
            }
        }

        /// <summary>
        /// Metodo que guarda usuarios en la base de datos desde la administracion (en este caso no hace falta la confirmacion por correo electronico).
        /// </summary>
        /// <param name="nombreCompleto"></param>
        /// <param name="correoElectronico"></param>
        /// <param name="dniUsuario"></param>
        /// <param name="movilUsuario"></param>
        /// <param name="contraseña"></param>
        /// <param name="fchNacimiento"></param>
        /// <param name="imagen"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult añadirUsuario(string nombreCompleto, string correoElectronico, string dniUsuario, string movilUsuario, string contraseña, DateTime fchNacimiento, IFormFile imagen)
        {
            try
            {
                ServicioConsultas consultas = new ServicioConsultasImpl();
                consultas.log("Entrado en metodo que guarda un usuario desde admin.");
                ServicioConsultas consulta = new ServicioConsultasImpl();
                string nombreImagen;
                string rutaImagen = "";
                if (imagen != null && imagen.Length > 0)
                {
                    nombreImagen = Guid.NewGuid().ToString() + Path.GetExtension(imagen.FileName);
                    string rutaCompleta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "imagenes/usuarios", nombreImagen);
                    using (var stream = new FileStream(rutaCompleta, FileMode.Create))
                    {
                        imagen.CopyTo(stream);
                    }
                    rutaImagen = "/imagenes/usuarios/" + nombreImagen;
                }

                if (consulta.existeCorreoElectronico(correoElectronico) || consulta.existeDNI(dniUsuario))
                {
                    TempData["ErrorRegistro"] = "El correo electrónico o el DNI ya están registrados.";
                    return RedirectToAction("irAAñadirUsuario", "ControladorAdministracion"); // Puedes redirigir a una vista de error o a la misma página de registro
                }

                //Si el correo electronico no existe, pasamos al registro del usuario.
                DateTime fchRegistro = DateTime.Now.ToUniversalTime();
                string rolAcceso = "basico";
                bool confirmacion = true;
                string token = null;
                Usuarios nuevoUsuario = new Usuarios(nombreCompleto, correoElectronico, dniUsuario, movilUsuario, contraseña, fchRegistro.Date, fchNacimiento.ToUniversalTime(), rolAcceso, rutaImagen, confirmacion, token);
                consulta.registrarUsuario(nuevoUsuario);
                TempData["MensajeRegistroExitoso"] = "Usuario registrado con éxito.";
                return RedirectToAction("irUsuario", "ControladorAdministracion");

            }
            catch (Exception ex)
            {
                Console.WriteLine("Se ha producido un error: {0}", ex);
                return View("~/Views/Errores/paginaError.cshtml");

            }

        }
    }


}
