using DB;
using DB.Modelo;
using Microsoft.EntityFrameworkCore;

namespace LinkSocial1.Servicios
{
    public class ServicioConsultasImpl:ServicioConsultas
    {
        private readonly GestorLinkSocialDbContext dbContext;

        public ServicioConsultasImpl(GestorLinkSocialDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public ServicioConsultasImpl()
        {
            dbContext = new GestorLinkSocialDbContext();
        }
        public bool IniciarSesion(string correoElectronico, string contraseña, out string rolUsuario)
        {
            ServicioEncriptar encriptarContraseña = new ServicioEncriptarImpl();

            var usuario = dbContext.Usuarios.FirstOrDefault(u => u.correoElectronico == correoElectronico && u.contraseña == encriptarContraseña.Encriptar(contraseña));

            if (usuario == null)
            {
                rolUsuario = null; // Usuario no encontrado, no hay rol que devolver
                return false;
            }
            else
            {
                rolUsuario = usuario.rolAcceso;
                return true;
            }
        }

        public void registrarUsuario(Usuarios nuevoUsuario)
        {
            ServicioEncriptar encriptar = new ServicioEncriptarImpl();
            using(var contexto = new GestorLinkSocialDbContext())
            {
                nuevoUsuario = new Usuarios
                {
                    nombreCompleto=nuevoUsuario.nombreCompleto,
                    correoElectronico=nuevoUsuario.correoElectronico,
                    dniUsuario=nuevoUsuario.dniUsuario,
                    movilUsuario=nuevoUsuario.movilUsuario,
                    contraseña=encriptar.Encriptar(nuevoUsuario.contraseña),
                    fchNacimiento=nuevoUsuario.fchNacimiento,
                    fchRegistro=nuevoUsuario.fchRegistro,
                    rolAcceso=nuevoUsuario.rolAcceso
                };

                contexto.Usuarios.Add(nuevoUsuario);
                contexto.SaveChanges();
            }
        }
        public bool existeCorreoElectronico(string correoElectronico)
        {
            return dbContext.Usuarios.Any(u => u.correoElectronico == correoElectronico);
        }
        public bool existeDNI(string dni)
        {
            return dbContext.Usuarios.Any(u => u.dniUsuario == dni);
        }
    }
}
