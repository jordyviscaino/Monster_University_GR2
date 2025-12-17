using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Monster_University_GR2.CapaEntidad.Validaciones
{
    public class PasswordFuerteAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // 1. Si es nulo, dejamos que el atributo [Required] se encargue
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return ValidationResult.Success;
            }

            string password = value.ToString();

            // 2. Validación de Longitud (Mínimo 8, Máximo 20)
            if (password.Length < 8 || password.Length > 20)
            {
                return new ValidationResult("La contraseña debe tener entre 8 y 20 caracteres.");
            }

            // 3. Validación de Complejidad (Regex)

            // Tiene Mayúscula?
            if (!Regex.IsMatch(password, @"[A-Z]"))
            {
                return new ValidationResult("La contraseña debe contener al menos una letra MAYÚSCULA.");
            }

            // Tiene Minúscula?
            if (!Regex.IsMatch(password, @"[a-z]"))
            {
                return new ValidationResult("La contraseña debe contener al menos una letra minúscula.");
            }

            // Tiene Número?
            if (!Regex.IsMatch(password, @"[0-9]"))
            {
                return new ValidationResult("La contraseña debe contener al menos un NÚMERO.");
            }

            // Tiene Carácter Especial? (@, #, $, %, etc.)
            // Explicación Regex: Busca cualquier carácter que NO sea letra ni número
            if (!Regex.IsMatch(password, @"[\W_]"))
            {
                return new ValidationResult("La contraseña debe contener al menos un carácter especial (ej: @, #, $, .).");
            }

            return ValidationResult.Success;
        }
    }
}