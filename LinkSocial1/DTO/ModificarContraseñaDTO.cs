using System.ComponentModel.DataAnnotations;

namespace LinkSocial1.DTO
{
    public class ModificarContraseñaDTO
    {
        public int idUsuario { get; set; }

        [Required(ErrorMessage = "El campo de la contraseña es obligatorio.")]
        [RegularExpression(@"^(?=.*[a-zA-Z]).+$", ErrorMessage = "La contraseña debe incluir al menos una letra.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "La confirmación de la contraseña es obligatoria.")]
        [Compare("Password", ErrorMessage = "Las contraseñas no coinciden.")]
        public string Password2 { get; set; }
    }
}
