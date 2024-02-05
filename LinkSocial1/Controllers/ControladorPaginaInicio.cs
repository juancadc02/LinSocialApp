using DB.Modelo;
using LinkSocial1.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LinkSocial1.Controllers
{
    [Authorize]
    public class ControladorPaginaInicio : Controller
    {
        /// <summary>
        /// Metodo encargado de devolver la pagina de inicio.
        /// </summary>
        /// <returns></returns>
        public ActionResult cargarPaginaInicio()
        {
            try
            {
                ServicioConsultas consultas = new ServicioConsultasImpl();
                //Creamos una lista y llamamos al metodo lista usuarios.
                List<Publicaciones> listaPublicaciones = consultas.mostrarPublicaciones();
                ViewData["listaPublicaciones"] = listaPublicaciones;
                return View("~/Views/Home/PaginaInicio.cshtml");

            }catch (Exception ex)
            {
                Console.WriteLine("Se ha producido un error: {0}", ex);
                return View("~/Views/Errores/paginaError.cshtml");
            }
        }
    }
}
