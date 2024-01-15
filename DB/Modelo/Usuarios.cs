using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.Modelo
{
    public class Usuarios
    {


        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int idUsuario { get; set; }
        public string nombreCompleto { get; set; }
        public string correoElectronico { get; set; }
        public string dniUsuario { get; set; }
        public string movilUsuario { get; set; }
        public string contraseña { get; set; }






        #region Constructores

        //Constructor con todos los campos.

        public Usuarios(string nombreCompleto, string correoElectronico, string dniUsuario, string movilUsuario, string contraseña)
        {
            this.nombreCompleto = nombreCompleto;
            this.correoElectronico = correoElectronico;
            this.dniUsuario = dniUsuario;
            this.movilUsuario = movilUsuario;
            this.contraseña = contraseña;
        }

        //Constructor por defecto.
        public Usuarios()
        {
        }







        #endregion

    }
}
