using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering; // NECESARIO para SelectListItem

namespace Monster_University_GR2.CapaEntidad
{
    public class UsuarioListViewModel
    {
        public string Cedula { get; set; }
        [Display(Name = "Nombre Completo")]
        public string NombreCompleto { get; set; }
        public string Email { get; set; }
        public string Estado { get; set; }
        public string Rol { get; set; }
    }

    public class UsuarioCreateViewModel
    {
        [Required(ErrorMessage = "La cédula es obligatoria")]
        [Display(Name = "Cédula")]
        public string Cedula { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El apellido es obligatorio")]
        public string Apellido { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Contraseña")]
        public string PasswordHash { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [Display(Name = "Contraseña")]
        [DataType(DataType.Password)] // <--- Importante para que se vea oculto en el HTML
        public string Password { get; set; } // <--- CAMBIADO DE PasswordHash A Password
        // --- DATOS PERSONALES (Faltaban aquí) ---

        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de Nacimiento")]
        public DateTime FechaNacimiento { get; set; }

        [Required(ErrorMessage = "El sexo es obligatorio")]
        [Display(Name = "Sexo")]
        public string SexoCodigo { get; set; }

        [Required(ErrorMessage = "El estado civil es obligatorio")]
        [Display(Name = "Estado Civil")]
        public string EstadoCivilCodigo { get; set; }

        [Display(Name = "Dirección")]
        public string Direccion { get; set; }

        [Display(Name = "Teléfono / Celular")]
        public string Celular { get; set; }

        // --- LISTAS PARA LOS DESPLEGABLES (Dropdowns) ---
        // Estas listas se llenan en el Controlador antes de mostrar la vista
        public IEnumerable<SelectListItem> Sexos { get; set; }
        public IEnumerable<SelectListItem> EstadosCiviles { get; set; }

        // Roles y Estado
        public List<string> Roles { get; set; } // Roles seleccionados
        public IEnumerable<SelectListItem> RolesDisponibles { get; set; } // Lista para elegir
        public string Estado { get; set; }
    }

    public class UsuarioEditViewModel
    {
        // La cédula suele ser ReadOnly en edición, pero la necesitamos para saber a quién editar
        [Required]
        public string Cedula { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        public string Apellido { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        // --- DATOS PERSONALES ---

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de Nacimiento")]
        public DateTime FechaNacimiento { get; set; }

        [Required]
        [Display(Name = "Sexo")]
        public string SexoCodigo { get; set; }

        [Required]
        [Display(Name = "Estado Civil")]
        public string EstadoCivilCodigo { get; set; }

        public string Direccion { get; set; }
        public string Celular { get; set; }
        public string Estado { get; set; }

        // --- LISTAS PARA LOS DESPLEGABLES ---
        // Necesarias para que al editar no se pierdan las opciones del combo
        public IEnumerable<SelectListItem> Sexos { get; set; }
        public IEnumerable<SelectListItem> EstadosCiviles { get; set; }
    }
}