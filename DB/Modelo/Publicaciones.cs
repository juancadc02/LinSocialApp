﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.Modelo
{
    public class Publicaciones
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int idPublicacion { get; set; }
        [ForeignKey("usuarios")]
        public int idUsuario { get; set; }
        public DateTime fchPublicacion { get; set; }
        public string contenidoPublicacion { get; set; }
        public string ? pieDeFoto { get; set; }
        public Usuarios usuarios { get; set; }

        #region Constructores
      


        

        public Publicaciones()
        {
            
        }

        public Publicaciones(int idUsuario, DateTime fchPublicacion, string contenidoPublicacion, string pieDeFoto)
        {
            this.idUsuario = idUsuario;
            this.fchPublicacion = fchPublicacion;
            this.contenidoPublicacion = contenidoPublicacion;
            this.pieDeFoto = pieDeFoto;
        }





        #endregion
    }
}
