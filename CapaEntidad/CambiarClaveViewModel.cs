using System.ComponentModel.DataAnnotations;
using Monster_University_GR2.CapaEntidad.Validaciones; // <--- No olvides esto

namespace Monster_University_GR2.CapaEntidad
{
    public class CambiarClaveViewModel
    {
        [Required]
        public string Email { get; set; }

        [Required(ErrorMessage = "La nueva contraseña es obligatoria")]
        [PasswordFuerte]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar nueva contraseña")]
        [Compare("NewPassword", ErrorMessage = "Las contraseñas no coinciden.")]
        public string ConfirmPassword { get; set; }
    }
}