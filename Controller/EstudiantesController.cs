using Microsoft.AspNetCore.Mvc;
using Monster_University_GR2.CapaNegocio;

namespace Monster_University_GR2.Controllers
{
    public class EstudiantesController : Controller
    {
        // ---------------------------------------------------------
        // 1. ZONA ADMINISTRATIVA (CRUD)
        // Ruta: /Estudiantes/Index
        // ---------------------------------------------------------
        public IActionResult Index()
        {
            CN_Estudiante logica = new CN_Estudiante();
            var lista = logica.Listar();
            return View(lista); // Retorna la vista con botones de Editar/Eliminar
        }

        // ---------------------------------------------------------
        // 2. ZONA DE REPORTES (Solo Lectura)
        // Ruta: /Estudiantes/ListaReportes
        // ---------------------------------------------------------
        public IActionResult ListaReportes()
        {
            CN_Estudiante logica = new CN_Estudiante();
            var lista = logica.Listar();
            return View(lista); // Retorna la vista con botones de Imprimir PDF
        }

        // ---------------------------------------------------------
        // 3. FUNCIONALIDAD DEL MODAL (Compartida)
        // ---------------------------------------------------------
        // En EstudiantesController.cs

        public IActionResult BoletinPartial(string id)
        {
            // Validación básica
            if (string.IsNullOrEmpty(id)) return Content("Error: ID no proporcionado.");

            CN_Estudiante logica = new CN_Estudiante();
            var modelo = logica.ObtenerBoletin(id);

            if (modelo == null) return Content("Error: Estudiante no encontrado.");

            // Retorna SOLO el HTML del reporte, sin layout
            return PartialView("_BoletinPartial", modelo);
        }
    }
}