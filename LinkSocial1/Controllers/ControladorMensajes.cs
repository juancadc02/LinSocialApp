using DB.Modelo;
using LinkSocial1.Servicios;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LinkSocial1.Controllers
{
    public class ControladorMensajes : Controller
    {
        public IActionResult irAPaginaPrincipalMensajes()
        {
           

            return View("~/Views/Mensajes/PaginaPrincipalMensajes.cshtml");
        }

        public IActionResult buscarUsuarioParaEnviarMensaje(string correoElectronico)
        {
            ServicioConsultas consultas = new ServicioConsultasImpl();

            Usuarios usuarioEncontrado = consultas.buscarUsuario(correoElectronico);

            if (usuarioEncontrado != null)
            {
                string nombreUsuario = usuarioEncontrado.nombreCompleto;
                TempData["mensajeExito"] = "Usuario encontrado";
                return View("~/Views/Mensajes/PaginaPrincipalMensajes.cshtml", usuarioEncontrado);

            }
            else
            {
                TempData["mensajeError"] = "Usuario no encontrado";
                return View("~/Views/Mensajes/PaginaPrincipalMensajes.cshtml");
            }

        }

        public IActionResult paginaChatMensaje(int idUsuarioDestino)
        {
            ServicioConsultas consultas = new ServicioConsultasImpl();
            // Obtener el ID del usuario actual
            var idUsuarioActual = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Aquí debes cargar el historial de mensajes entre el usuario actual y el usuario destino.
            // Puedes utilizar tu servicio o lógica de negocio para obtener los mensajes.
           
            List<Mensajes> historialMensajes = consultas.ObtenerHistorialMensajes(idUsuarioActual, idUsuarioDestino);

            ViewBag.IdUsuarioDestino = idUsuarioDestino;
            return View("~/Views/Mensajes/PaginaChatMensaje.cshtml", historialMensajes);
        }

        [HttpPost]
        public IActionResult enviarMensaje(int idUsuarioDestino, string mensaje)
        {
            ServicioConsultas consulta = new ServicioConsultasImpl();
            // Obtener el ID del usuario actual
            var idUsuarioActual = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Guardar el mensaje en la base de datos
            DateTime fchMensaje = DateTime.Now.ToUniversalTime();
           Mensajes nuevoMensaje = new Mensajes(Convert.ToInt32(idUsuarioActual),idUsuarioDestino,mensaje,fchMensaje);
            consulta.enviarMensaje(nuevoMensaje);
            List<Mensajes> historialMensajes = consulta.ObtenerHistorialMensajes(idUsuarioActual, idUsuarioDestino);

            // Redirige nuevamente a la página de chat para mostrar el mensaje enviado
            return View("~/Views/Mensajes/PaginaChatMensaje.cshtml", historialMensajes);
        }




    }
}
