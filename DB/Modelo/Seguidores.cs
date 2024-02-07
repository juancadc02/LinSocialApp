using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.Modelo
{
    public class Seguidores
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int idSeguidores {  get; set; }

        [ForeignKey("usuarioSolicitud")]
        public int idSeguidorSolicitud { get; set; } //El que tiene la sesion iniciada

        [ForeignKey("usuarioSeguido")]
        public int idSeguidorSeguido { get; set; } //Al usuario que seguimos 

        public DateTime fchSeguimiento { get; set; }

        public bool siguiendo { get; set; } 


        public Usuarios usuarioSolicitud { get; set; }

        public Usuarios usuarioSeguido { get; set; }




        #region Constructores
        public Seguidores()
        {
        }

       
        public Seguidores(int idSeguidorSolicitud, int idSeguidorSeguido, DateTime fchSeguimiento, bool siguiendo)
        {
            this.idSeguidorSolicitud = idSeguidorSolicitud;
            this.idSeguidorSeguido = idSeguidorSeguido;
            this.fchSeguimiento = fchSeguimiento;
            this.siguiendo = siguiendo;
        }

        #endregion
    }
}
