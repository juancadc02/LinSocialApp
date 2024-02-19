using DB;
using LinkSocial1.DTO;
using LinkSocial1.Servicios;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LinkSocial1.Controllers
{
    public class ControladorAjustes : Controller
    {

        private readonly GestorLinkSocialDbContext _contexto;

        /// <summary>
        /// Contructor con el contexto.
        /// </summary>
        /// <param name="contexto"></param>
        public ControladorAjustes(GestorLinkSocialDbContext contexto)
        {
            _contexto = contexto;
        }
        public IActionResult irAAjustes()
        {
            try
            {
                return View("~/Views/Ajustes/PaginaAjuste.cshtml");

            }
            catch (Exception ex)
            {
                Console.WriteLine("Se ha producido un error: {0}", ex);
                return View("~/Views/Errores/paginaError.cshtml");
            }
        }
        public IActionResult irAModificarContraseña()
        {
            try
            {
                return View("~/Views/Ajustes/ModificarContraseña.cshtml");

            }
            catch (Exception ex)
            {
                Console.WriteLine("Se ha producido un error: {0}", ex);
                return View("~/Views/Errores/paginaError.cshtml");
            }
        }

        [HttpPost]
        public IActionResult ActualizarContraseña(ModificarContraseñaDTO modelo)
        {
            try
            {
                ServicioEncriptar encriptarContraseña = new ServicioEncriptarImpl();

                // Comprobamos que los datos del formulario están rellenos.
                if (!ModelState.IsValid)
                {
                    return View("~/Views/Ajustes/ModificarContraseña.cshtml", modelo);
                }
                var claimsPrincipal = User;
                string idUsuario = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                // Busca al usuario por su ID en la base de datos.
                var usuario = _contexto.Usuarios.FirstOrDefault(u => u.idUsuario == Convert.ToInt32(idUsuario));

                // Si el usuario es distinto de null, actualiza la contraseña.
                if (usuario != null)
                {
                    usuario.contraseña = encriptarContraseña.Encriptar(modelo.Password);

                    _contexto.Entry(usuario).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    _contexto.SaveChanges();
                }


                return View("~/Views/Ajustes/PaginaAjuste.cshtml");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Se ha producido un error: {0}", ex);
                return View("~/Views/Errores/paginaError.cshtml");
            }
        }

    }
}
