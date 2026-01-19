using System.ComponentModel.DataAnnotations;
using System.Linq; // Necesario para .Contains()

namespace Monster_University_GR2.CapaEntidad.Validaciones
{
    public class DominioCorreoAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return ValidationResult.Success;
            }

            string email = value.ToString().Trim().ToLower(); // Convertir a minúsculas

            // 1. Validar que tenga @ (Seguridad básica)
            if (!email.Contains("@"))
            {
                return new ValidationResult("El correo no es válido.");
            }

            // 2. Extraer el dominio (lo que está después del @)
            string[] partes = email.Split('@');
            if (partes.Length != 2)
            {
                return new ValidationResult("El formato del correo es incorrecto.");
            }
            string dominio = partes[1];

            // 3. LISTA BLANCA DE DOMINIOS PERMITIDOS
            // Aquí defines qué proveedores aceptas.
            string[] dominiosPermitidos = new string[]
            {
                "gmail.com",
                "hotmail.com",
                "outlook.com",
                "live.com",
                "yahoo.com",
                "monster.edu.ec", // Tu dominio institucional
                "yahoo.es",
                "icloud.com"
            };

            // 4. Verificar si el dominio está en la lista
            if (!dominiosPermitidos.Contains(dominio))
            {
                return new ValidationResult($"El dominio '@{dominio}' no es válido. Use: Gmail, Hotmail, Outlook, Yahoo o Institucional.");
            }

            return ValidationResult.Success;
        }
    }
}