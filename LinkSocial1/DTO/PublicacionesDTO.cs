using DB.Modelo;
using System.ComponentModel.DataAnnotations.Schema;

namespace LinkSocial1.DTO
{
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
