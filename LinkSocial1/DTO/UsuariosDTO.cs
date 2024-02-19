namespace LinkSocial1.DTO
{
    public class UsuariosDTO
    {
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
    }
}
