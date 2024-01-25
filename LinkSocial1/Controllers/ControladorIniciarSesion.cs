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
        /// Metodo encargardo de abrir la pagina con el formulario de iniciar sesion.
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

                if (string.IsNullOrEmpty(correoElectronico) || string.IsNullOrEmpty(contraseña))
                {
                    TempData["MensajeCampoVacios"] = "Introduzca el correo y contraseña";
                    return View("~/Views/InicioSesion/IniciarSesion.cshtml");

                }

                if (consultas.IniciarSesion(correoElectronico, contraseña, out string rolUsuario))
                {
                    // Autenticar al usuario
                    var claims = new List<Claim>
                {
                     new Claim(ClaimTypes.Name, correoElectronico),
                 };

                    // Verificar si el usuario tiene el rol "admin"
                    if (rolUsuario == "admin")
                    {
                        claims.Add(new Claim(ClaimTypes.Role, "admin"));
                        // Puedes agregar otros roles aquí según la lógica de tu aplicación
                    }

                    ClaimsIdentity claimsIdentity = new ClaimsIdentity(
                        claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    AuthenticationProperties authProperties = new AuthenticationProperties
                    {
                        AllowRefresh = true
                    };

                    HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);

                    return RedirectToAction("cargarPaginaInicio", "ControladorPaginaInicio");
                }
                else
                {
                    TempData["Error"] = "Usuario o contraseña incorrectos. Inténtelo de nuevo.";
                    return View("~/Views/InicioSesion/IniciarSesion.cshtml");
                }

            }catch (Exception ex)
            {
                Console.WriteLine("Se ha producido un error: {0}", ex);
                return View("~/Views/Errores/paginaError.cshtml");
            }
        }

        /// <summary>
        /// Metodo que borra las cookies (cierra sesion) y redirige al login.
        /// </summary>
        /// <returns></returns>
        public IActionResult CerrarSesion()
        {

            try
            {
                HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return RedirectToAction("irAIniciarSesion", "ControladorIniciarSesion");

            }catch (Exception ex)
            {
                Console.WriteLine("Se ha producido un error: {0}", ex);
                return View("~/Views/Errores/paginaError.cshtml");
            }
        }
    }
}
