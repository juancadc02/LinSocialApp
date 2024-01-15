using DB.Modelo;

namespace LinkSocial1.Servicios
{
    /// <summary>
    /// Interfaz que declara los métodos que se van a utilizar para realizar consultas
    /// </summary>
    public interface ServicioConsultas
    {
        public bool IniciarSesion(string email_usuario, string clave_usuario);
        /// <summary>
        /// Método encargado de registrar nuevos usuarios
        /// </summary>
        /// <param name="nuevoUsuario"> Objeto usuario que se va registrar en la bbdd</param>
        public void registrarUsuario(Usuarios nuevoUsuario);
        /// <summary>
        /// Interfaz del metodo que comprueba en la base de datos si el email introducido existe o no.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public bool existeCorreoElectronico(string correoElectronico);
        /// <summary>
        /// Interfaz del metodo que comprueba en la base de datos si el dni introducido existe o no.
        /// </summary>
        /// <param name="dni"></param>
        /// <returns></returns>
        public bool existeDNI(string dni);
    }
}
