using DB.Modelo;
using LinkSocial1.Servicios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LinkSocial1.Controllers
{
    public class ControladorRegistrarUsuarios : Controller
    {

        /// <summary>
        /// Metodo encargado de cargar la pagina del formulario de registro.
        /// </summary>
        /// <returns></returns>
        public IActionResult irARegistro()
        {
            try {
                return View("~/Views/Registro/RegistrarUsuario.cshtml");

            }
            catch (Exception ex)
            {
                Console.WriteLine("Se ha producido un error: {0}", ex);
                return View("~/Views/Errores/paginaError.cshtml");
            }
        }
        
        /// <summary>
        /// Metodo encargado de guardar un registro en la base de datos, comprobando previamente si el correo electronico y el dni no existe,
        /// en el caso de que existiera mostraria un mensaje de error y no podria registrarme.
        /// Por defecto tenemos el campo fechaRegistro que es la fecha de ese momento y el campo del rol del usuario.
        /// </summary>
        /// <param name="nombreCompleto"></param>
        /// <param name="correoElectronico"></param>
        /// <param name="dniUsuario"></param>
        /// <param name="movilUsuario"></param>
        /// <param name="contraseña"></param>
        /// <param name="fchNacimiento"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult RegistrarUsuario(string nombreCompleto,string correoElectronico,string dniUsuario,string movilUsuario,string contraseña,DateTime fchNacimiento)
        {
            try
            {
                ServicioConsultas consulta = new ServicioConsultasImpl();

                if (consulta.existeCorreoElectronico(correoElectronico) || consulta.existeDNI(dniUsuario))
                {
                    TempData["ErrorRegistro"] = "El correo electrónico o el DNI ya están registrados.";
                    return RedirectToAction("irARegistro", "ControladorRegistrarUsuarios"); // Puedes redirigir a una vista de error o a la misma página de registro
                }

                //Si el correo electronico no existe, pasamos al registro del usuario.
                DateTime fchRegistro = DateTime.Now.ToUniversalTime();
                string rolAcceso = "basico";
                Usuarios nuevoUsuario = new Usuarios(nombreCompleto, correoElectronico, dniUsuario, movilUsuario, contraseña, fchRegistro.Date, fchNacimiento.ToUniversalTime(), rolAcceso);
                consulta.registrarUsuario(nuevoUsuario);
                TempData["MensajeRegistroExitoso"] = "Usuario registrado con éxito.";
                return RedirectToAction("irAIniciarSesion", "ControladorIniciarSesion");

            }
            catch (Exception ex)
            {
                Console.WriteLine("Se ha producido un error: {0}", ex);
                return View("~/Views/Errores/paginaError.cshtml");

            }

        }
    }
}
