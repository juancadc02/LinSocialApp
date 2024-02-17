using DB.Modelo;
using LinkSocial1.DTO;
using System.Net.Mail;
using static LinkSocial1.Servicios.ServicioConsultasImpl;

namespace LinkSocial1.Servicios
{
    /// <summary>
    /// Interfaz que declara los métodos que se van a utilizar para realizar consultas
    /// </summary>
    public interface ServicioConsultas
    {
        public bool IniciarSesion(string correoElectronico, string contraseña, out int idUsuario, out string rolUsuario);
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

        /// <summary>
        /// Interfaz del metodo que devuelve un listado con todos los usuarios de la base de datos.
        /// </summary>
        public List<Usuarios> mostrarUsuarios();

        public void EnviarEmail(string emailDestino, string nombreUser, string token);


        //Metodos para subir publicaciones 
        public void subirPublicacion(Publicaciones nuevaPublicacion);
        public List<Publicaciones> mostrarPublicaciones();
        public void añadirComentario(Comentarios nuevoComentario);
        public List<ComentarioConUsuarioViewModel> mostrarComentariosConUsuario();

        //Metodos para buscar usuarios 

        public Usuarios buscarUsuario(string correoElectronico);
        public Usuarios buscarUsuarioPorId(int idUsuario);
        public List<Publicaciones> buscarPublicacionesPorIdUsuario(int idUsuario);
        public int ObtenerNumeroSeguidores(int idUsuario);
        public int ObtenerNumeroSeguidos(int idUsuario);


        //Metodos para seguir a un usuario
        public void iniciarSeguimiento(Seguidores nuevoSeguidor);
        public void dejarDeSeguir(int idSeguidorSolicitud, int idSeguidorSeguido);
        public bool estaSiguiendo(int idSeguidorSolicitud, int idSeguidorSeguido);


        //Metodo para likes 
        public void añadirLike(LikeUsuariosPublicaciones nuevoLike);
        public bool usuarioDioLike(int idUsuario, int idPublicacion);
        public void eliminarLike(int idUsuario, int idPublicacion);


        //Metodos enviar mensaje 

        public List<Mensajes> ObtenerHistorialMensajes(string idUsuarioActual, int idUsuarioDestino);

        public void enviarMensaje (Mensajes nuevoMensaje);



    }
}
