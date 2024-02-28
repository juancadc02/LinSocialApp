using DB.Modelo;
using LinkSocial1.DTO;

namespace LinkSocial1.Servicios
{
    public class ServicioADtoImpl :ServicioADto
    {

        public List<PublicacionesDTO> ConvertirListaDAOaDTOPublicaciones(List<Publicaciones> listaDAO)
        {
            return listaDAO.Select(ConvertirDAOaDTOPublicaciones).ToList();
        }

        public PublicacionesDTO ConvertirDAOaDTOPublicaciones(Publicaciones publicacionDAO)
        {
            return new PublicacionesDTO
            {
                idPublicacion = publicacionDAO.idPublicacion,
                idUsuario = publicacionDAO.idUsuario,
                fchPublicacion = publicacionDAO.fchPublicacion,
                contenidoPublicacion = publicacionDAO.contenidoPublicacion,
                pieDeFoto = publicacionDAO.pieDeFoto
                // Puedes omitir 'usuarios' si no lo necesitas en el DTO
            };
        }





        public List<ComentariosDTO> ConvertirListaDAOaDTOComentarios(List<Comentarios> listaDAO)
        {
            return listaDAO.Select(ConvertirDAOaDTOComentarios).ToList();
        }

        // Método para convertir un objeto ComentarioDAO a un objeto ComentarioDTO
        public ComentariosDTO ConvertirDAOaDTOComentarios(Comentarios comentarioDAO)
        {
            return new ComentariosDTO
            {
                idComentario = comentarioDAO.idComentario,
                idUsuario = comentarioDAO.idUsuario,
                idPublicacion = comentarioDAO.idPublicacion,
                contenidoComentario = comentarioDAO.contenidoComentario,
                fchComentario = comentarioDAO.fchComentario
                // Puedes omitir 'usuarios' y 'publicaciones' si no los necesitas en el DTO
            };
        }



        public List<UsuariosDTO> ConvertirListaDAOaDTOUsuarios(List<Usuarios> listaDAO)
        {
            return listaDAO.Select(ConvertirDAOaDTOUsuarios).ToList();
        }

        // Método para convertir un objeto UsuarioDAO a un objeto UsuarioDTO
        public UsuariosDTO ConvertirDAOaDTOUsuarios(Usuarios usuarioDAO)
        {
            return new UsuariosDTO
            {
                idUsuario = usuarioDAO.idUsuario,
                nombreCompleto = usuarioDAO.nombreCompleto,
                correoElectronico = usuarioDAO.correoElectronico,
                dniUsuario = usuarioDAO.dniUsuario,
                movilUsuario = usuarioDAO.movilUsuario,
                contraseña = usuarioDAO.contraseña,
                fchRegistro = usuarioDAO.fchRegistro,
                fchNacimiento = usuarioDAO.fchNacimiento,
                rolAcceso = usuarioDAO.rolAcceso,
                tokenRecuperacion = usuarioDAO.tokenRecuperacion,
                fchVencimientoToken = usuarioDAO.fchVencimientoToken,
                rutaImagen = usuarioDAO.rutaImagen
            };
        }


        public MensajeDto ConvertirDAOaDTOMensajes(Mensajes mensajesDAO)
        {
            return new MensajeDto
            {
                idMensaje = mensajesDAO.idMensaje,
                contenidoMensaje = mensajesDAO.contenidoMensaje,
                fchEnvioMensaje = mensajesDAO.fchEnvioMensaje,
                idUsuarioQueEnvia=mensajesDAO.idUsuarioQueEnvia,
                idUsuarioQueRecibe=mensajesDAO.idUsuarioQueRecibe,
                usuariosEnvia=mensajesDAO.usuariosEnvia,
                usuariosRecibe=mensajesDAO.usuariosRecibe
                /*     public Usuarios usuariosEnvia { get; set; }
        public Usuarios usuariosRecibe { get; set; }*/



            };
        }
        public List<MensajeDto> ConvertirListaDAOaDTOMensajes(List<Mensajes> listaDAO)
        {
            return listaDAO.Select(ConvertirDAOaDTOMensajes).ToList();

        }

    }
}
