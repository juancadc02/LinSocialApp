using DB.Modelo;
using LinkSocial1.Servicios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LinkSocial1.Controllers
{
    public class ControladorRegistrarUsuarios : Controller
    {
        public IActionResult irARegistro()
        {
            return View("~/Views/Registro/RegistrarUsuario.cshtml");
        }
        
        [HttpPost]
        public IActionResult RegistrarUsuario(string nombreCompleto,string correoElectronico,string dniUsuario,string movilUsuario,string contraseña)
        {
            ServicioConsultas consulta = new ServicioConsultasImpl();

            if (consulta.existeCorreoElectronico(correoElectronico))
            {
                TempData["ErrorRegistro"] = "El correo electrónico o el DNI ya están registrados.";
                return RedirectToAction("irARegistro", "ControladorRegistrarUsuarios"); // Puedes redirigir a una vista de error o a la misma página de registro
            }

            //Si el correo electronico no existe, pasamos al registro del usuario.

            Usuarios nuevoUsuario = new Usuarios(nombreCompleto, correoElectronico, dniUsuario, movilUsuario, contraseña);
            consulta.registrarUsuario(nuevoUsuario);
            TempData["MensajeRegistroExitoso"] = "Usuario registrado con éxito.";
            return RedirectToAction("irAIniciarSesion", "ControladorIniciarSesion");
        }
    }
}
