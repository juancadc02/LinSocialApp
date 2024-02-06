using DB.Modelo;

namespace LinkSocial1.DTO
{
    /// <summary>
    /// Clase para mostrar los comentarios con el nombre de quien los ha puesto.
    /// </summary>
    public class ComentarioConUsuarioViewModel
    {
        public Comentarios Comentario { get; set; }
        public Usuarios Usuario { get; set; }
    }
}
