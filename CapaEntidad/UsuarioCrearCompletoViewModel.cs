using System;
using System.ComponentModel.DataAnnotations;
using Monster_University_GR2.CapaEntidad.Validaciones;

namespace Monster_University_GR2.CapaEntidad
{
    public class UsuarioCrearCompletoViewModel
    {
        [Required(ErrorMessage = "Debe seleccionar el tipo de vinculación")]
        public string TipoVinculacion { get; set; } // Valores: "EST" (Estudiante) o "EMP" (Empleado)
        // --- DATOS DE IDENTIFICACIÓN ---
        [Required(ErrorMessage = "La cédula es obligatoria")]
        [CedulaEcuador(ErrorMessage = "Cédula inválida.")]
        public string Cedula { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [DosPalabras(ErrorMessage = "Ingrese dos nombres")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El apellido es obligatorio")]
        [DosPalabras(ErrorMessage = "Ingrese dos apellidos")]
        public string Apellido { get; set; }

        [Required(ErrorMessage = "El correo es obligatorio")]
        [DominioCorreo(ErrorMessage = "Dominio inválido")]
        public string Email { get; set; }

        // --- DATOS ADICIONALES (NUEVOS) ---
        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria")]
        [DataType(DataType.Date)]
        public DateTime FechaNacimiento { get; set; }

        [Required(ErrorMessage = "La dirección es obligatoria")]
        [StringLength(200, ErrorMessage = "La dirección es muy larga")]
        public string Direccion { get; set; }

        [Required(ErrorMessage = "El celular es obligatorio")]
        [RegularExpression(@"^09\d{8}$", ErrorMessage = "El celular debe empezar con 09 y tener 10 dígitos.")]
        public string Celular { get; set; }

        [Display(Name = "Teléfono Domicilio")]
        [RegularExpression(@"^0\d{6,8}$", ErrorMessage = "Formato convencional inválido (Ej: 022...)")]
        public string TelefonoDomicilio { get; set; }

        [Required]
        [Range(0, 20, ErrorMessage = "Número de cargas inválido")]
        [Display(Name = "Cargas Familiares")]
        public int CargasFamiliares { get; set; }

        // --- CATALOGOS ---
        [Required(ErrorMessage = "Seleccione Género")]
        public string SexoCodigo { get; set; }

        [Required(ErrorMessage = "Seleccione Estado Civil")]
        public string EstadoCivilCodigo { get; set; }
        [Display(Name = "Foto de Perfil")]
        public IFormFile FotoPerfil { get; set; }
        // --- SEGURIDAD ---
        // El admin asigna una contraseña inicial (o generamos una por defecto)
        [Required(ErrorMessage = "Asigne una contraseña temporal")]
        [PasswordFuerte]
        public string Password { get; set; }
    }
}

