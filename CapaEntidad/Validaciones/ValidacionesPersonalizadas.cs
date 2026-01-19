using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;

namespace Monster_University_GR2.CapaEntidad.Validaciones
{
    // 1. VALIDACIÓN DE NOMBRE COMPUESTO (Mínimo 2 palabras: "JORDY ANTHONY")
    public class NombreCompuestoAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                // Dejamos que [Required] haga su trabajo si es nulo
                return ValidationResult.Success;
            }

            string texto = value.ToString().Trim();
            // Dividimos por espacios para contar palabras
            string[] palabras = texto.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (palabras.Length < 2)
            {
                return new ValidationResult($"El campo {validationContext.DisplayName} debe contener al menos dos nombres/apellidos separados por espacio (Ej: JORDY ANTHONY).");
            }

            return ValidationResult.Success;
        }
    }

    // 2. VALIDACIÓN DE PROVEEDOR DE CORREO (Gmail, Hotmail, Outlook)
    public class EmailProveedorAttribute : ValidationAttribute
    {
        private readonly string[] _dominiosPermitidos = { "gmail.com", "hotmail.com", "outlook.com", "live.com", "yahoo.com" };

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null) return ValidationResult.Success;

            string email = value.ToString().ToLower();

            // Verificamos si contiene alguno de los dominios permitidos
            bool esValido = _dominiosPermitidos.Any(d => email.EndsWith("@" + d));

            if (!esValido)
            {
                return new ValidationResult("El correo debe ser de un proveedor válido (Gmail, Hotmail, Outlook, Yahoo).");
            }

            return ValidationResult.Success;
        }
    }

    // 3. VALIDACIÓN DE CONTRASEÑA FUERTE
    public class PasswordFuerteAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null) return ValidationResult.Success;

            string pass = value.ToString();

            // Reglas: Mínimo 8 caracteres, 1 Mayúscula, 1 Número, 1 Símbolo (opcional según rigor)
            // Regex explicada:
            // (?=.*[A-Z]) -> Al menos una mayúscula
            // (?=.*\d)    -> Al menos un número
            // .{8,}       -> Mínimo 8 caracteres

            var regex = new Regex(@"^(?=.*[A-Z])(?=.*\d).{8,}$");

            if (!regex.IsMatch(pass))
            {
                return new ValidationResult("La contraseña es débil. Debe tener mínimo 8 caracteres, incluir al menos una mayúscula y un número.");
            }

            return ValidationResult.Success;
        }
    }

    // 4. VALIDACIÓN DE TELÉFONO ECUADOR (Móvil o Fijo)
    public class TelefonoEcuadorAttribute : ValidationAttribute
    {
        public string Tipo { get; set; } = "MOVIL"; // "MOVIL" o "FIJO"

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null) return ValidationResult.Success;
            string numero = value.ToString();

            if (!long.TryParse(numero, out _))
                return new ValidationResult("El teléfono solo debe contener números.");

            if (Tipo == "MOVIL")
            {
                // Celular Ecuador: Empieza con 09 y tiene 10 dígitos
                if (numero.Length != 10 || !numero.StartsWith("09"))
                {
                    return new ValidationResult("El celular debe tener 10 dígitos y empezar con 09.");
                }
            }
            else // FIJO
            {
                // Fijo Ecuador: Usualmente 7 dígitos + código área (ej 02), total 9 o 7 según se guarde
                // Asumiremos formato nacional de 9 dígitos incluyendo código provincia (Ej: 022555555)
                if (numero.Length < 7 || numero.Length > 10)
                {
                    return new ValidationResult("El teléfono fijo debe tener un formato válido (Incluya código de provincia).");
                }
            }

            return ValidationResult.Success;
        }
    }
}