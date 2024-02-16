using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.Modelo
{
    public class LikeUsuariosPublicaciones
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int idLike {  get; set; }
        [ForeignKey("usuario")]
        public int idUsuario { get; set; }
        [ForeignKey("publicaciones")]
        public int idPublicacion { get; set; }
        public DateTime fchLike { get; set; }


        public Usuarios usuario { get; set; }
        public Publicaciones publicaciones { get; set; }

        #region Constructores
        public LikeUsuariosPublicaciones(int idUsuario, int idPublicacion, DateTime fchLike)
        {
            this.idUsuario = idUsuario;
            this.idPublicacion = idPublicacion;
            this.fchLike = fchLike;
        }

        public LikeUsuariosPublicaciones()
        {
            
        }

        #endregion
    }
}
