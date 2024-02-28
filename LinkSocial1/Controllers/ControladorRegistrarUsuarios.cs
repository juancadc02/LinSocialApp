using DB;
using DB.Modelo;
using LinkSocial1.Servicios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LinkSocial1.Controllers
{
    public class ControladorRegistrarUsuarios : Controller
    {
        private readonly GestorLinkSocialDbContext _contexto;

        public ControladorRegistrarUsuarios(GestorLinkSocialDbContext contexto)
        {
            _contexto = contexto;
        }
        /// <summary>
        /// Metodo encargado de cargar la pagina del formulario de registro.
        /// </summary>
        /// <returns></returns>
        public IActionResult irARegistro()
        {
            try {
                ServicioConsultas consultar = new ServicioConsultasImpl();
                consultar.log("Entrando en el metodo abre la pagina para registrar a un usuario");
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
        public IActionResult RegistrarUsuario(string nombreCompleto, string correoElectronico, string dniUsuario, string movilUsuario, string contraseña, DateTime fchNacimiento, IFormFile imagen)
        {
            try
            {
                ServicioConsultas consulta = new ServicioConsultasImpl();
                consulta.log("Entrando en el método que registra un usuario en la base de datos");

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
                    return RedirectToAction("irARegistro", "ControladorRegistrarUsuarios");
                }

                // Si el correo electrónico no existe, pasamos al registro del usuario.
                DateTime fchRegistro = DateTime.Now.ToUniversalTime();
                string rolAcceso = "basico";

                // Generar token para la confirmación de correo
                string tokenConfirmacion = Guid.NewGuid().ToString();

                bool correoConfirmado = false;
                Usuarios nuevoUsuario = new Usuarios(nombreCompleto, correoElectronico, dniUsuario, movilUsuario, contraseña, fchRegistro.Date, fchNacimiento.ToUniversalTime(), rolAcceso, rutaImagen, correoConfirmado, tokenConfirmacion);
                consulta.registrarUsuario(nuevoUsuario);

                // Enviar correo de confirmación
                consulta.EnviarEmailConfirmacion(correoElectronico, nombreCompleto, tokenConfirmacion);

                TempData["MensajeRegistroExitoso"] = "Usuario registrado con éxito. Por favor, confirme su correo electrónico.";
                return RedirectToAction("irAIniciarSesion", "ControladorIniciarSesion");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Se ha producido un error: {0}", ex);
                return View("~/Views/Errores/paginaError.cshtml");
            }
        }
        [HttpGet]
        public IActionResult ConfirmarCuenta(string token)
        {
            try
            {
                ServicioConsultas consultar = new ServicioConsultasImpl();
                consultar.log("Entrando en el método que confirma la cuenta del usuario");

                // Buscar usuario por token y actualizar la confirmación de correo
                var user= _contexto.Usuarios.SingleOrDefault(u => u.tokenRecuperacion == token);

                if (user != null)
                {
                    user.correoConfirmado = true;
                    user.tokenRecuperacion = null;
                    user.fchVencimientoToken = null;

                    _contexto.Entry(user).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    _contexto.SaveChanges();

                    TempData["MensajeConfirmacionExitosa"] = "¡Cuenta confirmada exitosamente!";
                    return View("~/Views/InicioSesion/IniciarSesion.cshtml");
                }

                TempData["MensajeTokenInvalido"] = "El token ha expirado o no es válido.";
                return View("~/Views/Errores/paginaError.cshtml");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Se ha producido un error: {0}", ex);
                return View("~/Views/Errores/paginaError.cshtml");
            }
        }
    }
}

