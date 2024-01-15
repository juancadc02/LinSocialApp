using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.Modelo
{
    public class Accesos
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int idAcceso { get; set; }
        public int codigoAcceso { get; set; }
        public string descripcionAcceso { get; set; }


        #region Contructores

        //Contructor con todos los campos
        public Accesos(int idAcceso, int codigoAcceso, string descripcionAcceso)
        {
            this.idAcceso = idAcceso;
            this.codigoAcceso = codigoAcceso;
            this.descripcionAcceso = descripcionAcceso;
        }
        //Contructor por defecto.
        public Accesos()
        {

        }

        #endregion
    }
}
