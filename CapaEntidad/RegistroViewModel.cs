using Monster_University_GR2.CapaEntidad.Validaciones;
using System.ComponentModel.DataAnnotations;

namespace Monster_University_GR2.CapaEntidad
{
    public class RegistroViewModel
    {
        // --- CÉDULA (Ya la tenías) ---
        [Required(ErrorMessage = "La cédula es obligatoria")]
        [CedulaEcuador(ErrorMessage = "La cédula no es válida (algoritmo Ecuador).")]
        public string Cedula { get; set; }

        // --- NOMBRES (Nueva Validación) ---
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [DosPalabras(ErrorMessage = "Debe ingresar sus dos nombres (Ej: Juan Carlos)")]
        public string Nombre { get; set; }

        // --- APELLIDOS (Nueva Validación) ---
        [Required(ErrorMessage = "El apellido es obligatorio")]
        [DosPalabras(ErrorMessage = "Debe ingresar sus dos apellidos (Ej: Pérez López)")]
        public string Apellido { get; set; }

        // --- CORREO (Nueva Validación) ---
        [Required(ErrorMessage = "El correo es obligatorio")]
        [DominioCorreo(ErrorMessage = "El dominio del correo no parece válido.")]
        public string Email { get; set; }

        // ... El resto sigue igual (Sexos, Password, etc.) ...
        [Required(ErrorMessage = "Seleccione el género")]
        public string SexoCodigo { get; set; }

        [Required(ErrorMessage = "Seleccione el estado civil")]
        public string EstadoCivilCodigo { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Mínimo 6 caracteres")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Las contraseñas no coinciden")]
        public string ConfirmPassword { get; set; }
    }
}