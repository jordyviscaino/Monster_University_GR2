using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Monster_University_GR2.CapaEntidad.Validaciones
{
    public class CedulaEcuadorAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // 1. Si es nulo, dejamos que el atributo [Required] se encargue.
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return ValidationResult.Success;
            }

            string cedula = value.ToString();

            // 2. Verificar longitud y que sea numérico
            if (cedula.Length != 10 || !long.TryParse(cedula, out _))
            {
                return new ValidationResult("La cédula debe tener 10 dígitos numéricos.");
            }

            // 3. Verificar código de provincia (dos primeros dígitos)
            // Ecuador tiene 24 provincias (01-24) + 30 (extranjeros)
            int provincia = int.Parse(cedula.Substring(0, 2));
            if ((provincia < 1 || provincia > 24) && provincia != 30)
            {
                return new ValidationResult("El código de provincia (dos primeros dígitos) es inválido.");
            }

            // 4. Verificar tercer dígito (debe ser menor a 6 para personas naturales)
            int tercerDigito = int.Parse(cedula.Substring(2, 1));
            if (tercerDigito >= 6)
            {
                // Nota: RUCs y Públicas usan 6 o 9, pero para Cédula personal es < 6
                return new ValidationResult("El tercer dígito es inválido para una cédula de persona natural.");
            }

            // 5. ALGORITMO MÓDULO 10 (Validación matemática)
            if (!ValidarModulo10(cedula))
            {
                return new ValidationResult("La cédula ingresada es incorrecta (dígito verificador inválido).");
            }

            return ValidationResult.Success;
        }

        private bool ValidarModulo10(string cedula)
        {
            int[] coeficientes = { 2, 1, 2, 1, 2, 1, 2, 1, 2 };
            int total = 0;
            int digitoVerificador = int.Parse(cedula.Substring(9, 1));

            for (int i = 0; i < 9; i++)
            {
                int valor = int.Parse(cedula.Substring(i, 1)) * coeficientes[i];

                if (valor >= 10)
                {
                    valor = valor - 9;
                }

                total += valor;
            }

            int residuo = total % 10;
            int resultado;

            if (residuo == 0)
            {
                resultado = 0;
            }
            else
            {
                resultado = 10 - residuo;
            }

            return resultado == digitoVerificador;
        }
    }
}