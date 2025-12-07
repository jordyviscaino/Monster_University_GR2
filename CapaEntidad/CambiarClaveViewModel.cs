using System.ComponentModel.DataAnnotations;

namespace Monster_University_GR2.CapaEntidad
{
    public class CambiarClaveViewModel
    {
        [Required]
        public string Email { get; set; } // Lo pasaremos oculto para saber a quién actualizar

        [Required(ErrorMessage = "La nueva contraseña es obligatoria")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Mínimo 6 caracteres")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar nueva contraseña")]
        [Compare("NewPassword", ErrorMessage = "Las contraseñas no coinciden.")]
        public string ConfirmPassword { get; set; }
    }
}