using System.Collections.Generic;

namespace Monster_University_GR2.CapaEntidad
{
    // Para la tabla principal (Index)
    public class EstudianteResumenDTO
    {
        public string CodigoEstudiante { get; set; } // Cédula
        public string NombreCompleto { get; set; }
        public string Email { get; set; }
        public string Carrera { get; set; } // "Sustos", "Ingeniería", etc.
    }

    // Para el Reporte / Boletín
    public class BoletinNotaDTO
    {
        public string CodigoMateria { get; set; }
        public string NombreMateria { get; set; }
        public int Creditos { get; set; }
        public double Nota1 { get; set; } // Parcial 1
        public double Nota2 { get; set; } // Parcial 2
        public double Nota3 { get; set; } // Parcial 3
        public double Promedio { get; set; }
        public string Estado { get; set; } // APROBADO / REPROBADO
    }

    public class BoletinEstudianteViewModel
    {
        public EstudianteResumenDTO Estudiante { get; set; }
        public List<BoletinNotaDTO> Notas { get; set; }
        public double PromedioGeneral { get; set; }
    }
}