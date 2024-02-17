using DB.Modelo;
using LinkSocial1.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LinkSocial1.Controllers
{
    [Authorize] //Tiene que tener la sesion iniciada para navegar esta pagina.
    public class ControladorMensajes : Controller
    {
        /// <summary>
        /// Cargamos la pagina de inicio de los mensaje
        /// </summary>
        /// <returns></returns>
        public IActionResult irAPaginaPrincipalMensajes()
        {
            try
            {
                return View("~/Views/Mensajes/PaginaPrincipalMensajes.cshtml");
            }catch (Exception ex)
            {
                Console.WriteLine("Se ha producido un error: {0}", ex);
                return View("~/Views/Errores/paginaError.cshtml");
            }
        }

        public IActionResult buscarUsuarioParaEnviarMensaje(string correoElectronico)
        {
            try
            {
                ServicioConsultas consultas = new ServicioConsultasImpl();

                //Obtenemos el usuario que hemos introducido en el formulario por el correo.
                Usuarios usuarioEncontrado = consultas.buscarUsuario(correoElectronico);

                //Si existe el usuario lo mostramos.
                if (usuarioEncontrado != null)
                {
                    string nombreUsuario = usuarioEncontrado.nombreCompleto;
                    TempData["mensajeExito"] = "Usuario encontrado";
                    return View("~/Views/Mensajes/PaginaPrincipalMensajes.cshtml", usuarioEncontrado);

                }
                else //Si no existe el usuario mostramos que no existe.
                {
                    TempData["mensajeError"] = "Usuario no encontrado";
                    return View("~/Views/Mensajes/PaginaPrincipalMensajes.cshtml");
                }
            }catch (Exception ex)
            {
                Console.WriteLine("Se ha producido un error: {0}", ex);
                return View("~/Views/Errores/paginaError.cshtml");
            }

        }

        /// <summary>
        /// Cargamos la pagina del chat con el id del usuario que hemos buscado (al que vamos a enviar el mensaje)
        /// </summary>
        /// <param name="idUsuarioDestino"></param>
        /// <returns></returns>
        public IActionResult paginaChatMensaje(int idUsuarioDestino)
        {
            try
            {
                ServicioConsultas consultas = new ServicioConsultasImpl();
                
                // Obtenemos el ID del usuario actual
                var idUsuarioActual = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                //Obtenemos todos los mensajes entre el usuario que tiene la sesion iniciada y al que queremos enviar un mensaje.
                List<Mensajes> historialMensajes = consultas.ObtenerHistorialMensajes(idUsuarioActual, idUsuarioDestino);

                ViewBag.IdUsuarioDestino = idUsuarioDestino;
                //Mostramos la pagina del chat con los mensajes entre esos usuarios.
                return View("~/Views/Mensajes/PaginaChatMensaje.cshtml", historialMensajes);
            }catch (Exception ex)
            {
                Console.WriteLine("Se ha producido un error: {0}", ex);
                return View("~/Views/Errores/paginaError.cshtml");
            }
        }

        /// <summary>
        /// Metodo para enviar un mensaje al usuario que hemos buscado.
        /// Recibe el id del usuario que queremos enviar el mensaje y el contenido del mensaje
        /// </summary>
        /// <param name="idUsuarioDestino"></param>
        /// <param name="mensaje"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult enviarMensaje(int idUsuarioDestino, string mensaje)
        {
            try
            {
                ServicioConsultas consulta = new ServicioConsultasImpl();
                // Obtenemos el ID del usuario actual
                var idUsuarioActual = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                // Guardamos el mensaje en la base de datos
                DateTime fchMensaje = DateTime.Now.ToUniversalTime();
                Mensajes nuevoMensaje = new Mensajes(Convert.ToInt32(idUsuarioActual), idUsuarioDestino, mensaje, fchMensaje);
                consulta.enviarMensaje(nuevoMensaje);
                //Obtenemos de nuevo los mensajes actualizados
                List<Mensajes> historialMensajes = consulta.ObtenerHistorialMensajes(idUsuarioActual, idUsuarioDestino);

                // Redirige nuevamente a la página de chat para mostrar el mensaje enviado
                return View("~/Views/Mensajes/PaginaChatMensaje.cshtml", historialMensajes);
            }catch(Exception ex)
            {
                Console.WriteLine("Se ha producido un error: {0}", ex);
                return View("~/Views/Errores/paginaError.cshtml");
            }
        }




    }
}
