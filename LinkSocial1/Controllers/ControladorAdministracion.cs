using DB;
using DB.Modelo;
using LinkSocial1.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LinkSocial1.Controllers
{
    [Authorize(Roles = "admin")]
    public class ControladorAdministracion : Controller
    {
        private readonly GestorLinkSocialDbContext _contexto;

        public IActionResult irPaginaAdmin()
        {
           
            return View("~/Views/Administracion/PaginaAdministracion.cshtml");
        }
        public IActionResult irUsuario()
        {
            ServicioConsultas consultas = new ServicioConsultasImpl();
            //Creamos una lista y llamamos al metodo lista usuarios.
            List<Usuarios> listaUsuario = consultas.mostrarUsuarios();
            ViewData["listaUsuario"] = listaUsuario;
            return View("~/Views/Administracion/listaUsuario.cshtml");
        }

        
        public IActionResult EditarUsuario(int id)
        {
            // Obtén el usuario de la base de datos usando el ID
            var usuario = _contexto.Usuarios.Find(id);

            // Verifica si el usuario existe
            if (usuario == null)
            {
                return NotFound(); // Puedes manejar la situación en la que el usuario no se encuentra
            }

            

            return View("~/Views/Administracion/paginaEdicionUsuario.cshtml",usuario);

        }

        [HttpPost]
        public IActionResult GuardarEdicion(Usuarios usuarioEditado)
        {
            // Actualiza el estado del usuario en la base de datos
            _contexto.Update(usuarioEditado);
            _contexto.SaveChanges();

            TempData["SuccessMessage"] = "Usuario editado exitosamente";
            return RedirectToAction("irUsuario"); // Redirige a la lista de usuarios u otra vista apropiada
        }
        //Metodo encargado de eliminar al usuario por su id.
        public IActionResult EliminarUsuario(int id)
        {
            var usuarioExistente = _contexto.Usuarios.Find(id);

            if (usuarioExistente == null)
            {
                return NotFound();
            }

            try
            {
                // Verificamos si el usuario a eliminar es el último admin
                if (usuarioExistente.rolAcceso == "admin" && _contexto.Usuarios.Count(u => u.rolAcceso == "admin") == 1)
                {
                    // Si es el último admin, mostramos un mensaje de error
                    TempData["ErrorMessage"] = "No puedes eliminar al último usuario con rol de administrador.";
                    return RedirectToAction("irUsuario");
                }

                // Borramos el usuario y guardamos los cambios 
                _contexto.Usuarios.Remove(usuarioExistente);
                _contexto.SaveChanges();

                // Mostramos mensaje de éxito y redirigimos al listado
                TempData["SuccessMessage"] = "El usuario se ha eliminado correctamente.";
                return RedirectToAction("irUsuario");
            }
            catch (Exception ex)
            {
                // Mostramos error en la consola y redirigimos a la administración.
                Console.WriteLine(ex.Message);
                return RedirectToAction("irPaginaAdmin");
            }
        }



        public ControladorAdministracion(GestorLinkSocialDbContext contexto)
        {
            _contexto = contexto;
        }



    }


}
