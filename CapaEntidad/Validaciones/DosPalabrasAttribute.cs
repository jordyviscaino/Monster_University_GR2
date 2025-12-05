using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Monster_University_GR2.CapaEntidad.Validaciones
{
    public class DosPalabrasAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return ValidationResult.Success; // Dejamos que [Required] haga su trabajo
            }

            string texto = value.ToString().Trim();

            // EXPLICACIÓN REGEX:
            // ^              -> Inicio de la cadena
            // [a-zA-ZñÑáéíóúÁÉÍÓÚ]+ -> Una palabra (letras y tildes)
            // \s             -> Un espacio en blanco (puedes poner \s+ si aceptas varios espacios)
            // [a-zA-ZñÑáéíóúÁÉÍÓÚ]+ -> Otra palabra
            // $              -> Fin de la cadena
            string patron = @"^[a-zA-ZñÑáéíóúÁÉÍÓÚ]+\s[a-zA-ZñÑáéíóúÁÉÍÓÚ]+$";

            if (!Regex.IsMatch(texto, patron))
            {
                return new ValidationResult("Debe ingresar exactamente dos nombres/apellidos separados por un espacio (Ej: 'Juan Pablo').");
            }

            return ValidationResult.Success;
        }
    }

}