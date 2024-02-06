using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.Modelo
{
    public class Comentarios
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int idComentario {  get; set; }

        [ForeignKey("usuarios")]
        public int idUsuario { get; set; }

        [ForeignKey("publicaciones")]
        public int idPublicacion {  get; set; }

        public string contenidoComentario { get; set; }

        public DateTime fchComentario { get; set; }

        public Usuarios usuarios { get; set; }
        public Publicaciones publicaciones { get; set; }

        public Comentarios(int idUsuario, int idPublicacion, string contenidoComentario, DateTime fchComentario)
        {
            this.idUsuario = idUsuario;
            this.idPublicacion = idPublicacion;
            this.contenidoComentario = contenidoComentario;
            this.fchComentario = fchComentario;
        }

        public Comentarios()
        {
            
        }
    }
}
