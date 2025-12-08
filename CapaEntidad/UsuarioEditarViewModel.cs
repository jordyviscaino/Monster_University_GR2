using System.ComponentModel.DataAnnotations;
using Monster_University_GR2.CapaEntidad.Validaciones;

namespace Monster_University_GR2.CapaEntidad
{
    public class UsuarioEditarViewModel
    {
        // La Cédula es la llave, no se edita, solo se muestra
        public string Cedula { get; set; }

        [Required]
        [DosPalabras]
        public string Nombre { get; set; }

        [Required]
        [DosPalabras]
        public string Apellido { get; set; }

        [Required]
        [DominioCorreo]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime FechaNacimiento { get; set; }

        [Required]
        public string Direccion { get; set; }

        [Required]
        [RegularExpression(@"^09\d{8}$", ErrorMessage = "Celular inválido")]
        public string Celular { get; set; }

        public string TelefonoDomicilio { get; set; }

        [Required]
        public int CargasFamiliares { get; set; }

        [Required]
        public string SexoCodigo { get; set; }

        [Required]
        public string EstadoCivilCodigo { get; set; }

        // Agregamos Estado para poder Inactivar/Activar desde la edición
        [Required]
        public string EstadoUsuario { get; set; } // 'A' o 'I'
    }
}