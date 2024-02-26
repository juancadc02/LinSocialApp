using DB.Modelo;
using System.ComponentModel.DataAnnotations.Schema;

namespace LinkSocial1.DTO
{
    /// <summary>
    /// DTO para mostrar las publicaciones de la base de datos en la vista
    /// </summary>
    public class PublicacionesDTO
    {
        
        public int idPublicacion { get; set; }
        [ForeignKey("usuarios")]
        public int idUsuario { get; set; }
        public DateTime fchPublicacion { get; set; }
        public string contenidoPublicacion { get; set; }
        public string? pieDeFoto { get; set; }
        public Usuarios usuarios { get; set; }
    }
}
