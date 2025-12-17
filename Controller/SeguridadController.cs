using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Monster_University_GR2.CapaNegocio;

namespace Monster_University_GR2.Controllers
{
    public class SeguridadController : Controller
    {
        // GET: Seguridad/AsignarRol (Cambia el nombre si gustas)
        public IActionResult AsignarRol()
        {
            CN_Seguridad logica = new CN_Seguridad();
            // Usamos CodigoCompuesto como Value y Descripcion como Texto
            ViewBag.ListaRoles = new SelectList(logica.ListarRoles(), "CodigoCompuesto", "Descripcion");
            return View();
        }

        // ... Los métodos AJAX son idénticos, solo asegúrate que reciben 'idPerfil' 
        // que ahora contendrá "SYS|ADM" ...
        [HttpGet]
        public IActionResult ObtenerTablas(string idPerfil)
        {
            // Debug: Ver si llega el ID
            Console.WriteLine($"--> Recibiendo petición para perfil: {idPerfil}");

            if (string.IsNullOrEmpty(idPerfil))
            {
                return Json(new { asignados = new List<object>(), disponibles = new List<object>() });
            }

            CN_Seguridad logica = new CN_Seguridad();

            // Llamamos a la lógica que separa "ACA|DOC"
            var asignados = logica.ObtenerAsignados(idPerfil);
            var disponibles = logica.ObtenerNoAsignados(idPerfil);

            // Debug: Ver cuántos datos encontró
            Console.WriteLine($"--> Asignados encontrados: {asignados.Count}");
            Console.WriteLine($"--> Disponibles encontrados: {disponibles.Count}");

            return Json(new { asignados = asignados, disponibles = disponibles });
        }

        [HttpPost]
        public IActionResult Asignar(string login, string idPerfil)
        {
            CN_Seguridad logica = new CN_Seguridad();
            bool result = logica.Asignar(login, idPerfil);
            return Json(new { exito = result });
        }

        [HttpPost]
        public IActionResult Desasignar(string login) // Solo necesitamos el login
        {
            CN_Seguridad logica = new CN_Seguridad();
            // Enviamos null en el perfil porque la lógica interna lo fuerza a "INV"
            bool result = logica.Desasignar(login, null);
            return Json(new { exito = result });
        }
    }
}