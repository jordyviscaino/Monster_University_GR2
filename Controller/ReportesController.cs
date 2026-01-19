using Microsoft.AspNetCore.Mvc;
using Monster_University_GR2.CapaNegocio;
using Monster_University_GR2.CapaDatos;
using Monster_University_GR2.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Monster_University_GR2.Controllers
{
    public class ReportesController : Controller
    {
        private readonly ServicioUsuarios _servicioUsuarios;
        private readonly ContextoMongo _contextoMongo; // Inyectamos tu contexto

        // Actualizamos el constructor para recibir el ContextoMongo
        public ReportesController(ServicioUsuarios servicioUsuarios, ContextoMongo contextoMongo)
        {
            _servicioUsuarios = servicioUsuarios;
            _contextoMongo = contextoMongo;
        }
        // ==========================================
        // 1. REPORTE DE SEGURIDAD (Listado Usuarios)
        // ==========================================
        // GET: Reporte de Seguridad
        [HttpGet]
        public async Task<IActionResult> Seguridad()
        {
            var lista = await _servicioUsuarios.ObtenerTodos();

            // CAMBIO: Le decimos explícitamente que use la vista "ReporteSeguridad"
            return View("ReporteSeguridad", lista);
        }

        // Método para cargar el contenido del Modal (Impresión)
        [HttpGet]
        public async Task<IActionResult> ReporteSeguridadPartial()
        {
            var lista = await _servicioUsuarios.ObtenerTodos();
            // Retorna la vista parcial que subiste (_ReporteSeguridadPartial.cshtml)
            return PartialView("_ReporteSeguridadPartial", lista);
        }

        // ==========================================
        // 2. REPORTE ACADÉMICO (Estudiantes)
        // ==========================================
        [HttpGet]
        public async Task<IActionResult> Academico()
        {
            var listaEstudiantes = await _servicioUsuarios.ObtenerSoloEstudiantes();
            // Retornamos la vista que tú subiste: "ListaReportes.cshtml"
            return View("ListaReportes", listaEstudiantes);
        }

        // GET: Obtiene el PartialView del Boletín (AJAX)
        [HttpGet]
        public async Task<IActionResult> VerBoletin(string id)
        {
            var modeloBoletin = await _servicioUsuarios.GenerarBoletinSimulado(id);
            return PartialView("_BoletinPartial", modeloBoletin);
        }


        [HttpGet]
        public IActionResult Sistema()
        {
            // Lista manual de colecciones que existen en tu Mongo
            // Esto es para llenar el Dropdown en la vista
            ViewBag.Colecciones = new List<string>
            {
                "users",
                "sexos",
                "estados_civiles",
                "sangre",             // Asegúrate que este sea el nombre real en Mongo
                "security_profiles",  // Asegúrate que este sea el nombre real en Mongo
                "carreras"            // Si tienes una de carreras
            };
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerDatosColeccion(string nombreColeccion)
        {
            try
            {
                // 1. Usamos el nuevo método de tu contexto
                var coleccion = _contextoMongo.ObtenerColeccionGenerica(nombreColeccion);

                // 2. Traemos los documentos (limitado a 200 por rendimiento)
                var documentosBson = await coleccion.Find(new BsonDocument()).Limit(200).ToListAsync();

                // 3. Convertimos BsonDocument a una lista de Diccionarios (JSON plano)
                var listaJson = documentosBson.Select(doc =>
                {
                    // Convertimos a diccionario para que el serializador de JSON de .NET lo entienda
                    var dict = doc.ToDictionary();

                    // Ajuste visual: Convertir ObjectId a string simple
                    if (dict.ContainsKey("_id"))
                    {
                        dict["_id"] = dict["_id"].ToString();
                    }

                    // Ajuste para fechas u otros objetos complejos de Mongo
                    var llaves = new List<string>(dict.Keys);
                    foreach (var key in llaves)
                    {
                        if (dict[key] is BsonArray || dict[key] is BsonDocument)
                        {
                            // Si es un sub-objeto o array, lo mostramos como string JSON
                            dict[key] = dict[key].ToJson();
                        }
                    }

                    return dict;
                }).ToList();

                return Json(listaJson);
            }
            catch (Exception ex)
            {
                return BadRequest("Error al leer colección: " + ex.Message);
            }
        }

    }

}