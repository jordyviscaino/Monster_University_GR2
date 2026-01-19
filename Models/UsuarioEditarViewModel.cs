using System;
using System.ComponentModel.DataAnnotations;

namespace Monster_University_GR2.Models
{
    public class UsuarioEditarViewModel
    {
        // ==========================================
        // IDENTIFICADORES (Se envían ocultos o readonly)
        // ==========================================
        public string Cedula { get; set; }
        public string? Email { get; set; } // Puede ser null si no lo editamos o si es readonly

        // Estos campos son solo visuales en el Edit, por eso aceptamos nulls
        public string? Nombres { get; set; }
        public string? Apellidos { get; set; }

        // ==========================================
        // CAMPOS QUE SÍ ESTAMOS EDITANDO (OBLIGATORIOS)
        // ==========================================

        [Required(ErrorMessage = "El teléfono es obligatorio")]
        public string Telefono { get; set; }

        [Required(ErrorMessage = "La dirección es obligatoria")]
        public string Direccion { get; set; }

        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria")]
        [DataType(DataType.Date)]
        public DateTime FechaNacimiento { get; set; }

        [Required(ErrorMessage = "El sexo es obligatorio")]
        public string SexoCodigo { get; set; }

        [Required(ErrorMessage = "El estado civil es obligatorio")]
        public string EstadoCivilCodigo { get; set; }

        [Required]
        public string EstadoUsuario { get; set; } // "ACTIVO" o "INACTIVO"

        // ==========================================
        // CAMPOS INFORMATIVOS (OPCIONALES PARA EL EDIT)
        // ==========================================

        // AQUÍ ESTABA EL ERROR: Agregamos '?' para que no sean obligatorios
        public string? Rol { get; set; }
        public string? CargasFamiliares { get; set; }
    }
}