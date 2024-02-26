using DB.Modelo;
using LinkSocial1.DTO;

namespace LinkSocial1.Servicios
{
    /// <summary>
    /// Interfaz de los metodos para pasar los datos de DAO a DTO
    /// </summary>
    public interface ServicioADto
    {
        public List<PublicacionesDTO> ConvertirListaDAOaDTOPublicaciones(List<Publicaciones> listaDAO);
        public PublicacionesDTO ConvertirDAOaDTOPublicaciones(Publicaciones publicacionDAO);


        public List<ComentariosDTO> ConvertirListaDAOaDTOComentarios(List<Comentarios> listaDAO);
        public ComentariosDTO ConvertirDAOaDTOComentarios(Comentarios comentarioDAO);


        public List<UsuariosDTO> ConvertirListaDAOaDTOUsuarios(List<Usuarios> listaDAO);
      
        public UsuariosDTO ConvertirDAOaDTOUsuarios(Usuarios usuarioDAO);



    }
}
