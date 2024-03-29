﻿using DB.Modelo;
using LinkSocial1.DTO;
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
                ServicioConsultas consultas = new ServicioConsultasImpl();
                consultas.log("Entrando en el metodo que abre la vista de mensajes");
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
                ServicioADto Adto = new ServicioADtoImpl();
                consultas.log("Entrando en el metodo que busca una usuario para enviar mensaje por el correo electronico");

                //Obtenemos el usuario que hemos introducido en el formulario por el correo.
                Usuarios usuarioEncontrado = consultas.buscarUsuario(correoElectronico);

                UsuariosDTO usuarioDto =Adto.ConvertirDAOaDTOUsuarios(usuarioEncontrado);
                //Si existe el usuario lo mostramos.
                if (usuarioDto != null)
                {
                    string nombreUsuario = usuarioDto.nombreCompleto;
                    TempData["mensajeExito"] = "Usuario encontrado";
                    return View("~/Views/Mensajes/PaginaPrincipalMensajes.cshtml", usuarioDto);

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
                ServicioADto Adto = new ServicioADtoImpl();

                consultas.log("Entrando en el metodo que abre la pagina de chat con el usuario buscado por el correo");

                // Obtenemos el ID del usuario actual
                var idUsuarioActual = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                // Obtenemos todos los mensajes entre el usuario que tiene la sesión iniciada y al que queremos enviar un mensaje.
                List<Mensajes> historialMensajes = consultas.ObtenerHistorialMensajes(idUsuarioActual, idUsuarioDestino);
                List<MensajeDto> historialMensajesDto = Adto.ConvertirListaDAOaDTOMensajes(historialMensajes);
                ViewBag.IdUsuarioDestino = idUsuarioDestino;

                // Mostramos la página del chat con los mensajes entre esos usuarios.
                return View("~/Views/Mensajes/PaginaChatMensaje.cshtml", historialMensajesDto);
            }
            catch (Exception ex)
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
                consulta.log("Entrando en el metodo queenvia un mensaje al usuario que hemos buscado por el correo electronico");

                // Obtenemos el ID del usuario actual
                var idUsuarioActual = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                // Guardamos el mensaje en la base de datos
                DateTime fchMensaje = DateTime.Now.ToUniversalTime();
                Mensajes nuevoMensaje = new Mensajes(Convert.ToInt32(idUsuarioActual), idUsuarioDestino, mensaje, fchMensaje);
                consulta.enviarMensaje(nuevoMensaje);

                // Redirigir a la acción "paginaChatMensaje" con el idUsuarioDestino
                return RedirectToAction("paginaChatMensaje", new { idUsuarioDestino = idUsuarioDestino });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Se ha producido un error: {0}", ex);
                return View("~/Views/Errores/paginaError.cshtml");
            }
        }





    }
}
