//using Microsoft.AspNetCore.Mvc;
//using MongoDB.Bson;
//using MongoDB.Driver;
//using Monster_University_GR2.CapaDatos;
//// Verifica que este sea tu namespace correcto

//namespace Monster_University_GR2.Controllers
//{
//    // 1. ESTO ES LO NUEVO: Forzamos la ruta base
//    [Route("PruebaMongo")]
//    public class PruebaMongoController : Controller
//    {
//        private readonly ContextoMongo _context;

//        public PruebaMongoController(ContextoMongo context)
//        {
//            _context = context;
//        }

//        // 2. ESTO ES LO NUEVO: Forzamos que este método responda a la raíz de /PruebaMongo
//        [HttpGet("")]      // Responde a: /PruebaMongo
//        [HttpGet("Index")] // Responde a: /PruebaMongo/Index
//        public IActionResult Index()
//        {
//            try
//            {
//                // Verificamos conexión
//                var dbName = _context.Users.Database.DatabaseNamespace.DatabaseName;
//                long totalPerfiles = _context.Profiles.CountDocuments(new BsonDocument());

//                ViewBag.Mensaje = $"¡CONEXIÓN EXITOSA! Base de datos: {dbName}. " +
//                                  $"Documentos en Perfiles: {totalPerfiles}";
//            }
//            catch (Exception ex)
//            {
//                ViewBag.Mensaje = $"ERROR DE CONEXIÓN: {ex.Message}";
//            }

//            return View();
//        }
//    }
//}