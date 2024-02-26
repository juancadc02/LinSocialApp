using DB;
using DB.Modelo;
using LinkSocial1.DTO;
using LinkSocial1.Servicios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LinkSocial1.Controllers
{
    public class ControladorRecuperarContraseña : Controller
    {
        private readonly GestorLinkSocialDbContext _contexto;

        public ControladorRecuperarContraseña(GestorLinkSocialDbContext contexto)
        {
            _contexto = contexto;
        }


        public IActionResult irARecuperarContraseña()
        {
            try
            {
                ServicioConsultas consultas = new ServicioConsultasImpl();
                consultas.log("Entrando en el metodo que abre la vista del formulario para recuperar la contraseña");
                return View("~/Views/RecuperarContraseña/RecuperarContraseña.cshtml");
            }catch (Exception ex)
            {
                Console.WriteLine("Se ha producido un error: {0}", ex);
                return View("~/Views/Errores/paginaError.cshtml");
            }
        }

        [HttpPost]
        public IActionResult IniciarRecuperacion(string correoElectronico)
        {
            try
            {
                ServicioEncriptar encriptarContraseña = new ServicioEncriptarImpl();
                ServicioConsultas consultar = new ServicioConsultasImpl();
                consultar.log("Entrando en el metodo que envia un correo electronico al usuario para recuperar la contraseña");


                //Comprobamos que el formulario esta relleno.
                if (!ModelState.IsValid)
                {
                    TempData["MensajeError"] = "Introduzca su correo electronico";
                    return View("~/Views/RecuperarContraseña/RecuperarContraseña.cshtml");
                }

                //Comprobamos que el correo electronico introducido esta registrado, si no lo esta mostramos mensaje de error.
                if (!_contexto.Usuarios.Any(u => u.correoElectronico == correoElectronico))
                {
                    TempData["MensajeError"] = "El correo electrónico no está registrado.";
                    return View("~/Views/RecuperarContraseña/RecuperarContraseña.cshtml");
                }

                // Si pasa la validación busca al usuario por su dirección de correo electrónico.
                var user = _contexto.Usuarios.Where(u => u.correoElectronico == correoElectronico).FirstOrDefault();

                //Si el usuario no es nulo entra.
                if (user != null)
                {
                    // Genera un token único para el usuario y la recuperación.
                    string token = encriptarContraseña.Encriptar(Guid.NewGuid().ToString());

                    // Establece la fecha de vencimiento del token de dos minutos.
                    user.fchVencimientoToken = DateTime.UtcNow.AddMinutes(1);

                    // Asigna el token al usuario y actualiza la base de datos.
                    user.tokenRecuperacion = token;
                    _contexto.Entry(user).State = EntityState.Modified;
                    _contexto.SaveChanges();

                    // Envia un correo electrónico con el enlace de recuperación.
                    consultar.EnviarEmail(user.correoElectronico, user.nombreCompleto, token);
                    //Mostramos mensaje de que el correo ha sido enviado
                    TempData["MensajeRecuperacionExitoso"] = "Correo de recuperacion enviado correctamente";

                    //Redirigimos al inicio de sesion
                    return View("~/Views/InicioSesion/IniciarSesion.cshtml");

                }
                else
                {
                    TempData["MensajeError"] = "La recuperación de contraseña ha fallado. Verifique la información ingresada.";
                    return View("~/Views/RecuperarContraseña/RecuperarContraseña.cshtml");
                }
            }catch (Exception ex)
            {
                Console.WriteLine("Se ha producido un error: {0}", ex);
                return View("~/Views/Errores/paginaError.cshtml");
            }

        }


        /// <summary>
        /// Metodo encargado de abrir la vista que se envia a traves del correo electronico con el token.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Recuperar(string token)
        {
            try
            {
                ServicioConsultas consultar = new ServicioConsultasImpl();
                consultar.log("Entrando en el metodo que abre el enlace que enviamos por correo electronico para recuperar la contraseña");
                // Crea un modelo de vista y asigna el token.
                RecuperacionDTO model = new RecuperacionDTO();
                model.Token = token;


                if (model.Token == null || model.Token == String.Empty)
                {
                    // Si el token es nulo o vacío, muestra la vista de recuperación.
                    ViewBag.TokenNoValido = "El token no es válido";
                    return View("~/Views/RecuperarContraseña/ModificarContraseña.cshtml");
                }

                var user = _contexto.Usuarios
                    .Where(u => u.tokenRecuperacion == model.Token && u.fchVencimientoToken > DateTime.UtcNow)
                    .FirstOrDefault();

                if (user == null)
                {
                    // Si el usuario no es válido o el token ha expirado, muestra un mensaje y redirige al login.
                    TempData["MensajeTokenInvalido"] = "El token ha expirado o no es válido.";

                    return View("~/Views/InicioSesion/IniciarSesion.cshtml");
                }

                return View("~/Views/RecuperarContraseña/ModificarContraseña.cshtml");
            }catch (Exception ex)
            {
                Console.WriteLine("Se ha producido un error: {0}", ex);
                return View("~/Views/Errores/paginaError.cshtml");
            }
        }

        /// <summary>
        /// Acción HTTP POST para procesar la solicitud de recuperación.
        /// </summary>
        /// <param name="modelo">Modelo de la vista de recuperación.</param>
        /// <returns>Vista correspondiente después del procesamiento.</returns>
        [HttpPost]
        public IActionResult Recuperar(RecuperacionDTO modelo)
        {
            try
            {
                ServicioConsultas consultar = new ServicioConsultasImpl();
                consultar.log("Entrando en el metodo que modifica la contraseña del usuario");
                ServicioEncriptar encriptarContraseña = new ServicioEncriptarImpl();

                //Comprobamos que los datos del formulario estan rellenos.
                if (!ModelState.IsValid)
                {
                    return View("~/Views/RecuperarContraseña/ModificarContraseña.cshtml", modelo);
                }

                // Si pasa la validación busca al usuario por el token en la base de datos y por el token que llega de la URL.
                var user = _contexto.Usuarios.Where(u => u.tokenRecuperacion == modelo.Token).FirstOrDefault();

                //Aqui entra si el token es valido y si el usuario es distinto de null.
                if (user != null)
                {
                    // Si se encuentra dicho usuario se actualiza la contraseña y elimina el token de recuperación para que no se vuelva a usar.
                    user.contraseña = encriptarContraseña.Encriptar(modelo.Password);
                    user.tokenRecuperacion = null; //Se establece a null para que el token reclamado ya no se pueda usar nunca más
                    _contexto.Entry(user).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    _contexto.Usuarios.Update(user);
                    _contexto.SaveChanges();
                }

                TempData["MensajeModificacionContraseñaExito"] = "Contraseña modificada correctamente.";

                return View("~/Views/InicioSesion/IniciarSesion.cshtml");
            }catch(Exception ex) {
                Console.WriteLine("Se ha producido un error: {0}", ex);
                return View("~/Views/Errores/paginaError.cshtml");
            }
        }


    }

}
