using Monster_University_GR2.CapaEntidad.Validaciones;
using System.ComponentModel.DataAnnotations;

namespace Monster_University_GR2.CapaEntidad
{
    public class RegistroViewModel
    {
        // --- CÉDULA ---
        [Required(ErrorMessage = "La cédula es obligatoria")]
        // Quitamos el Regex simple y ponemos nuestra validación completa:
        [CedulaEcuador(ErrorMessage = "La cédula ingresada no es válida.")]
        public string Cedula { get; set; }

        // --- NOMBRES Y APELLIDOS ---
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(50, ErrorMessage = "El nombre es muy largo")]
        [RegularExpression(@"^[a-zA-ZñÑáéíóúÁÉÍÓÚ\s]+$", ErrorMessage = "El nombre solo debe contener letras.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El apellido es obligatorio")]
        [StringLength(50, ErrorMessage = "El apellido es muy largo")]
        [RegularExpression(@"^[a-zA-ZñÑáéíóúÁÉÍÓÚ\s]+$", ErrorMessage = "El apellido solo debe contener letras.")]
        public string Apellido { get; set; }

        // --- CORREO ---
        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress(ErrorMessage = "El formato del correo no es válido")]
        public string Email { get; set; }

        // --- COMBOS ---
        [Required(ErrorMessage = "Seleccione el género")]
        public string SexoCodigo { get; set; }

        [Required(ErrorMessage = "Seleccione el estado civil")]
        public string EstadoCivilCodigo { get; set; }

        // --- CONTRASEÑA ---
        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar contraseña")]
        [Compare("Password", ErrorMessage = "Las contraseñas no coinciden.")]
        public string ConfirmPassword { get; set; }
    }
}