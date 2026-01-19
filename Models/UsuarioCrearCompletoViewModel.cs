using System;
using System.ComponentModel.DataAnnotations;

namespace Monster_University_GR2.Models
{
    public class UsuarioCrearCompletoViewModel
    {
        // --- DATOS DE CUENTA ---
        [Required(ErrorMessage = "La cédula es obligatoria")]
        public string Cedula { get; set; }

        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        public string Password { get; set; }

        // --- DATOS PERSONALES ---
        [Required(ErrorMessage = "Los nombres son obligatorios")]
        public string Nombres { get; set; }

        [Required(ErrorMessage = "Los apellidos son obligatorios")]
        public string Apellidos { get; set; }

        [Required(ErrorMessage = "El sexo es obligatorio")]
        public string SexoCodigo { get; set; }

        [Required(ErrorMessage = "El estado civil es obligatorio")]
        public string EstadoCivilCodigo { get; set; }

        [Required(ErrorMessage = "El teléfono es obligatorio")]
        public string Telefono { get; set; }

        [Required(ErrorMessage = "La dirección es obligatoria")]
        public string Direccion { get; set; }

        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria")]
        [DataType(DataType.Date)]
        public DateTime FechaNacimiento { get; set; }

        // --- SELECTOR DE TIPO ---
        // "EST" (Estudiante) o "EMP" (Empleado)
        [Required]
        public string TipoUsuario { get; set; }

        // ==========================================
        // CAMPOS OPCIONALES (Para evitar el bloqueo de validación)
        // ==========================================

        // Estudiante (Solo visual por ahora)
        public string? CarreraID { get; set; }
        public int? Semestre { get; set; }

        // Empleado
        public string? RolSeleccionado { get; set; } // El rol viene de la BD

        // Estos los llenaremos automáticamente en el backend según el Rol
        public string? Departamento { get; set; }
        public string? Cargo { get; set; }
    }
}