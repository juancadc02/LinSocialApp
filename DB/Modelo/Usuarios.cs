﻿using System;
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
        public DateTime fchRegistro { get; set; }
        public DateTime fchNacimiento { get; set; }
        public string rolAcceso { get; set; }
        public string? tokenRecuperacion { get; set; }
        public DateTime? fchVencimientoToken { get; set; }
        //Para poder tener imagen los usuarios.
        public string? rutaImagen { get; set; }
        public bool correoConfirmado { get; set; }




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

        public Usuarios(string nombreCompleto, string correoElectronico, string dniUsuario, string movilUsuario, string contraseña, DateTime fchRegistro, DateTime fchNacimiento, string rolAcceso, string? tokenRecuperacion, DateTime? fchVencimientoToken) : this(nombreCompleto, correoElectronico, dniUsuario, movilUsuario, contraseña)
        {
            this.fchRegistro = fchRegistro;
            this.fchNacimiento = fchNacimiento;
            this.rolAcceso = rolAcceso;
            this.tokenRecuperacion = tokenRecuperacion;
            this.fchVencimientoToken = fchVencimientoToken;
        }
        //Constructor para el registro de usuarios.
        public Usuarios(string nombreCompleto, string correoElectronico, string dniUsuario, string movilUsuario, string contraseña, DateTime fchRegistro, DateTime fchNacimiento, string rolAcceso, string rutaImagen, bool correoConfirmado, string tokenRecuperacion) : this(nombreCompleto, correoElectronico, dniUsuario, movilUsuario, contraseña)
        {
            this.fchRegistro = fchRegistro;
            this.fchNacimiento = fchNacimiento;
            this.rolAcceso = rolAcceso;
            this.rutaImagen = rutaImagen;
            this.correoConfirmado = correoConfirmado;
            this.tokenRecuperacion = tokenRecuperacion;
        }










        #endregion

    }
}
