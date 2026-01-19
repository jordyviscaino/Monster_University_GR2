using System.Collections.Generic;

namespace Monster_University_GR2.CapaEntidad // O Monster_University_GR2.Models
{
    // Para la lista de estudiantes (Buscador)
    public class EstudianteResumenDTO
    {
        public string CodigoEstudiante { get; set; } // Cédula
        public string NombreCompleto { get; set; }
        public string Carrera { get; set; }
    }

    // Para el diseño del Boletín (La hoja impresa)
    public class BoletinEstudianteViewModel
    {
        public string Cedula { get; set; }
        public string Nombres { get; set; }
        public string Carrera { get; set; }
        public string Periodo { get; set; }
        public string FechaGeneracion { get; set; }

        // Lista de materias y notas
        public List<DetalleNotaViewModel> Materias { get; set; } = new List<DetalleNotaViewModel>();
    }

    public class DetalleNotaViewModel
    {
        public string Materia { get; set; }
        public double Nota1 { get; set; }
        public double Nota2 { get; set; }
        public double Nota3 { get; set; }
        public double Promedio { get; set; }
    }
}