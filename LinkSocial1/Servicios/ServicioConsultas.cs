using DB.Modelo;
using LinkSocial1.DTO;
using System.Net.Mail;
using static LinkSocial1.Servicios.ServicioConsultasImpl;

namespace LinkSocial1.Servicios
{
    /// <summary>
    /// Interfaz que declara los métodos que se van a utilizar para realizar consultas sobre la base de datos
    /// </summary>
    public interface ServicioConsultas
    {

        #region Metodos de login.
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

        /// <summary>
        /// Interfaz del metodo que envia un correo electronico.
        /// </summary>
        /// <param name="emailDestino">Recibe el email de destino</param>
        /// <param name="nombreUser">El nombre del usuario</param>
        /// <param name="token">El token para ver si a caducado ese correo o no.</param>
        public void EnviarEmail(string emailDestino, string nombreUser, string token);

        #endregion

        #region Metodo para subir publicaciones

        /// <summary>
        /// Interfaz del metodo que añade una nueva publicacion a la base de datos 
        /// </summary>
        /// <param name="nuevaPublicacion">Recibe el objeto de publicacion</param>
        public void subirPublicacion(Publicaciones nuevaPublicacion);
        /// <summary>
        /// Interfaz del metodo que devuelve un listado de las publicaciones de la base de datos.
        /// </summary>
        /// <returns></returns>
        public List<Publicaciones> mostrarPublicaciones();
        /// <summary>
        /// Interfaz del metodo que añade un nuevo comentario a la base de datos 
        /// </summary>
        /// <param name="nuevoComentario">Recibe un objeto comentario</param>
        public void añadirComentario(Comentarios nuevoComentario);
        /// <summary>
        /// Interfaz del metodo que devulve una lista con todos los comentarios de la base de datos.
        /// </summary>
        /// <returns></returns>
        public List<ComentarioConUsuarioViewModel> mostrarComentariosConUsuario();
        /// <summary>
        /// Interfaz del metodo que elimina un registro de publicacion de la base de datos.
        /// </summary>
        /// <param name="idPublicacion">Recibe el id de la publicacion que quieres eliminar</param>
        public void eliminarPublicacion(int idPublicacion);

        #endregion

        #region Metodo para buscar usuarios y mostrar el perfil del usuario

        /// <summary>
        /// Interfaz del metodo que busca al usuario por el correo electronico y devuleve un objeto de usuario con los datos de ese usuario.
        /// </summary>
        /// <param name="correoElectronico">Recibe el correo electronico de el usuario que quieres buscar.</param>
        /// <returns></returns>
        public Usuarios buscarUsuario(string correoElectronico);
        /// <summary>
        /// Interfaz del metodo que busca un usuario por el id del usuario y devuelve el objeto usuario con sus datos.
        /// </summary>
        /// <param name="idUsuario">Recibe el id del usuario</param>
        /// <returns></returns>
        public Usuarios buscarUsuarioPorId(int idUsuario);

        /// <summary>
        /// Interfaz del metodo que devuelve un lista de las publicaciones que tiene ese usuarios
        /// </summary>
        /// <param name="idUsuario">Recibe el id del usuario.</param>
        /// <returns></returns>
        public List<Publicaciones> buscarPublicacionesPorIdUsuario(int idUsuario);
        /// <summary>
        /// Interfaz del metodo que devuelve el numero de seguidores que tiene un usuario.
        /// </summary>
        /// <param name="idUsuario">Recibe el id del usuario</param>
        /// <returns></returns>
        public int ObtenerNumeroSeguidores(int idUsuario);
        /// <summary>
        /// Interfaz del metodo que devuelve el numero de seguidos que tiene un usuario.
        /// </summary>
        /// <param name="idUsuario">Recibe el id del usuario</param>
        /// <returns></returns>
        public int ObtenerNumeroSeguidos(int idUsuario);

        #endregion

        #region Metodos para seguir a un usuario
        /// <summary>
        /// Interfaz del metodo que añade un seguidor en la base de datos.
        /// </summary>
        /// <param name="nuevoSeguidor"> Recibe el objeto seguidor</param>
        public void iniciarSeguimiento(Seguidores nuevoSeguidor);
        /// <summary>
        /// Interfaz del metodo que elimina un registro de la base de datos de la tabla seguidores.
        /// </summary>
        /// <param name="idSeguidorSolicitud">Recibe el id del seguirdor que deja de seguir</param>
        /// <param name="idSeguidorSeguido">Recibe el id del seguidor al que dejamos de seguir</param>
        public void dejarDeSeguir(int idSeguidorSolicitud, int idSeguidorSeguido);
        /// <summary>
        /// Interfaz del metodo que nos dice si seguimos o no al usuario.Devuelve true o false
        /// </summary>
        /// <param name="idSeguidorSolicitud">Recibe el id del seguidor que tiene la sesion iniciada</param>
        /// <param name="idSeguidorSeguido">Recibe el id del seguidor que queremos comprobar si seguimos o no</param>
        /// <returns></returns>
        public bool estaSiguiendo(int idSeguidorSolicitud, int idSeguidorSeguido);
        #endregion

        #region Metodos para dar like
        /// <summary>
        /// Interfaz del metodo que añade un like en la base de datos 
        /// </summary>
        /// <param name="nuevoLike">Recibe el objeto Like</param>
        public void añadirLike(LikeUsuariosPublicaciones nuevoLike);
        /// <summary>
        /// Interfaz del metodo que comprueba si el usuario ha dado like a la publicion o no
        /// </summary>
        /// <param name="idUsuario">Recibe el id del usuario que tiene la sesion iniciada</param>
        /// <param name="idPublicacion">Recibe el id de la publicacion que queremos comprobar </param>
        /// <returns></returns>
        public bool usuarioDioLike(int idUsuario, int idPublicacion);
        /// <summary>
        /// Interfaz del metodo que elimina un registro de la base de datos de la tabla like
        /// </summary>
        /// <param name="idUsuario">Recibe el id del usuario que tiene la sesion iniciada</param>
        /// <param name="idPublicacion">Recibe el id de la publicacion que queremos eliminar el like</param>
        public void eliminarLike(int idUsuario, int idPublicacion);
        #endregion

        #region Metodos para enviar mensaje
        /// <summary>
        /// Interfaz del metodo que devuelve un lista con los mensajes entre el usuario emitente y el usuario receptos
        /// </summary>
        /// <param name="idUsuarioActual">Recibe el id del usuario que tiene la sesion iniciada</param>
        /// <param name="idUsuarioDestino">Recibe el id del usuario al que hemos enviado los mensajes</param>
        /// <returns></returns>
        public List<Mensajes> ObtenerHistorialMensajes(string idUsuarioActual, int idUsuarioDestino);
        /// <summary>
        /// Interfaz del metodo que añade un mensaje a la base de datos 
        /// </summary>
        /// <param name="nuevoMensaje">Recibe el objeto mensaje</param>
        public void enviarMensaje (Mensajes nuevoMensaje);
        #endregion


    }
}
