using LinkSocial1.Servicios;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LinkSocial1.Controllers
{
    public class ControladorIniciarSesion : Controller
    {
        public IActionResult irAIniciarSesion()
        {
            return View("~/Views/InicioSesion/IniciarSesion.cshtml");
        }

        [HttpPost]
        public IActionResult IniciarSesion(string correoElectronico, string contraseña)
        {
            ServicioConsultas consultas = new ServicioConsultasImpl();
            if (string.IsNullOrEmpty(correoElectronico) || string.IsNullOrEmpty(contraseña))
            {
                return RedirectToAction("Error", "Home");
            }
            if (consultas.IniciarSesion(correoElectronico, contraseña))
            {
                // Autenticar al usuario
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, correoElectronico),
                };

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


        }
        public IActionResult CerrarSesion()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("irAIniciarSesion", "ControladorIniciarSesion");
        }
    }
}
