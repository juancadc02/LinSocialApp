using LinkSocial1.Servicios;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LinkSocial1.Controllers
{
    public class ControladorIniciarSesion : Controller
    {
        /// <summary>
        /// Metodo encargardo de abrir la pagina con el formulario de iniciar sesion (es lo primero que se ejecuta al arrancar la aplicacion).
        /// </summary>
        /// <returns></returns>
        public IActionResult irAIniciarSesion()
        {
            try
            {
                return View("~/Views/InicioSesion/IniciarSesion.cshtml");

            }catch (Exception ex)
            {
                Console.WriteLine("Se ha producido un error: {0}", ex);
                return View("~/Views/Errores/paginaError.cshtml");
            }
        }

        /// <summary>
        /// Metodo encargado de comprobar con la base de datos si el usuario y la contraseña introducidos son correctos o no,
        /// Caso 1: Si los campos estan vacios redirige a la pagina error.
        /// Caso 2: Si los campos no estan vacios, los comprueba con la base de datos, comprueba si es admin y no y entra en la pagina de inicio.
        /// Caso 3: La contraseña o el usuario son incorrectas, resetea el formulario y muestra el mensaje de error.
        /// </summary>
        /// <param name="correoElectronico"></param>
        /// <param name="contraseña"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult IniciarSesion(string correoElectronico, string contraseña)
        {
            try
            {
                ServicioConsultas consultas = new ServicioConsultasImpl();

                //Comprobamos si los campos de el correo y la contraseña estan vacios, si lo estan muestra el mensaje
                if (string.IsNullOrEmpty(correoElectronico) || string.IsNullOrEmpty(contraseña))
                {
                    TempData["MensajeCampoVacios"] = "Introduzca el correo y contraseña";
                    return View("~/Views/InicioSesion/IniciarSesion.cshtml");

                }
                //Comprobamos si el correo y la contraseña introducida son correctos o no.
                if (consultas.IniciarSesion(correoElectronico, contraseña, out int idUsuario, out string rolUsuario))
                {
                    //Autentificamos al usuario 
                    var claims = new List<Claim>
                {
                     new Claim(ClaimTypes.Name, correoElectronico),
                      new Claim(ClaimTypes.NameIdentifier, idUsuario.ToString()), // Añadir el ID del usuario como reclamación
                 };

                    // Verificamos si el usuario es admin o no.
                    if (rolUsuario == "admin")
                    {
                        claims.Add(new Claim(ClaimTypes.Role, "admin"));
                    }

                    ClaimsIdentity claimsIdentity = new ClaimsIdentity(
                        claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    AuthenticationProperties authProperties = new AuthenticationProperties
                    {
                        AllowRefresh = true
                    };

                    //Creamos las cokies ha ese usuario para cuando se cierre la sesion poder borrarlas.
                    HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);

                    return RedirectToAction("cargarPaginaInicio", "ControladorPaginaInicio");
                }
                //Mostramos mensaje de que el usuario o la contraseña son incorrectos.
                else
                {
                    TempData["Error"] = "Usuario o contraseña incorrectos. Inténtelo de nuevo.";
                    return View("~/Views/InicioSesion/IniciarSesion.cshtml");
                }

                //Controlamos todos los posibles errores y redirigimos a una pagina de error.
            }catch (Exception ex)
            {
                Console.WriteLine("Se ha producido un error: {0}", ex);
                return View("~/Views/Errores/paginaError.cshtml");
            }
        }

        /// <summary>
        /// Metodo que borra las cookies (cierra sesion) y redirige al login mostrando mensaje de que la sesion ha sido cerrada correctamente..
        /// </summary>
        /// <returns></returns>
        public IActionResult CerrarSesion()
        {
            try
            {
                HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                TempData["mensajeSesionCerrada"] = "Usuario encontrado";
                return RedirectToAction("irAIniciarSesion", "ControladorIniciarSesion");
            }catch (Exception ex)
            {
                Console.WriteLine("Se ha producido un error: {0}", ex);
                return View("~/Views/Errores/paginaError.cshtml");
            }
        }
    }
}
