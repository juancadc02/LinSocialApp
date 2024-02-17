using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.Modelo
{
    public class Mensajes
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int idMensaje {  get; set; }
       
        [ForeignKey("usuariosEnvia")]
        public int idUsuarioQueEnvia { get; set; }
       
        [ForeignKey("usuariosRecibe")]
        public int idUsuarioQueRecibe { get; set; }
       
        public string contenidoMensaje { get; set; }
       
        public DateTime fchEnvioMensaje { get; set; }


        public Usuarios usuariosEnvia { get; set; }
        public Usuarios usuariosRecibe { get; set; }


        #region Constructores
        public Mensajes()
        {
        }

        public Mensajes(int idUsuarioQueEnvia, int idUsuarioQueRecibe, string contenidoMensaje, DateTime fchEnvioMensaje)
        {
            this.idUsuarioQueEnvia = idUsuarioQueEnvia;
            this.idUsuarioQueRecibe = idUsuarioQueRecibe;
            this.contenidoMensaje = contenidoMensaje;
            this.fchEnvioMensaje = fchEnvioMensaje;
        }

        #endregion
    }
}
