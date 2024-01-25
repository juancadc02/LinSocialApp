using System.ComponentModel.DataAnnotations;

namespace LinkSocial1.DTO
{
    public class RecuperacionDTO
    {
        [Required(ErrorMessage = "El campo de la contraseña es obligatorio.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "La confirmación de la contraseña es obligatoria.")]
        [Compare("Password", ErrorMessage = "Las contraseñas no coinciden.")]
        public string Password2 { get; set; }

        public string Token { get; set; }
    }
}
