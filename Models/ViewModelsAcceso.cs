using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Monster_University_GR2.CapaEntidad.Validaciones; // Para usar tus validaciones personalizadas

namespace Monster_University_GR2.Models
{
  
    public class LoginViewModel
    {
        [Required(ErrorMessage = "El usuario es obligatorio")]
        public string UserLogin { get; set; } // Email

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [DataType(DataType.Password)]
        public string PassLogin { get; set; }
    }

 
    public class RegistroViewModel
    {
        [Required(ErrorMessage = "La cédula es obligatoria")]
        [CedulaEcuador] // Tu validación personalizada
        public string Cedula { get; set; }

        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress]
        [EmailProveedor] // Tu validación de Gmail/Hotmail
        public string Email { get; set; }

        [Required(ErrorMessage = "Los nombres son obligatorios")]
        [Display(Name = "Nombres")]
        public string Nombres { get; set; }

        [Required(ErrorMessage = "Los apellidos son obligatorios")]
        [Display(Name = "Apellidos")]
        public string Apellidos { get; set; } // <--- FALTABA ESTO

        [Required(ErrorMessage = "El celular es obligatorio")]
        [TelefonoEcuador(Tipo = "MOVIL")] // Tu validación
        public string TelefonoCelular { get; set; } // <--- FALTABA ESTO

        [Required]
        [MinLength(6, ErrorMessage = "Mínimo 6 caracteres")]
        [PasswordFuerte] // Tu validación de mayúsculas/números
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Las contraseñas no coinciden")]
        public string ConfirmPassword { get; set; }
    }

    
    public class UsuarioSesion
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public string RolCodigo { get; set; } // Rol principal (ADM, DOC, EST)
        public List<string> Permisos { get; set; } // Lista de permisos
    }
}