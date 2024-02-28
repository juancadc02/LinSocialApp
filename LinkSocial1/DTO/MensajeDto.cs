using DB.Modelo;
using System.ComponentModel.DataAnnotations.Schema;

namespace LinkSocial1.DTO
{
    public class MensajeDto
    {

        public int idMensaje { get; set; }

        public int idUsuarioQueEnvia { get; set; }

        public int idUsuarioQueRecibe { get; set; }

        public string contenidoMensaje { get; set; }

        public DateTime fchEnvioMensaje { get; set; }

        public Usuarios usuariosEnvia { get; set; }
        public Usuarios usuariosRecibe { get; set; }
    }
}
