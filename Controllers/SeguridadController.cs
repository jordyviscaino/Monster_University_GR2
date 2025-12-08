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
            CN_Seguridad logica = new CN_Seguridad();
            var asignados = logica.ObtenerAsignados(idPerfil);
            var noAsignados = logica.ObtenerNoAsignados(idPerfil);

            return Json(new { asignados = asignados, disponibles = noAsignados });
        }

        [HttpPost]
        public IActionResult Asignar(string login, string idPerfil)
        {
            CN_Seguridad logica = new CN_Seguridad();
            bool result = logica.Asignar(login, idPerfil);
            return Json(new { exito = result });
        }

        [HttpPost]
        public IActionResult Desasignar(string login, string idPerfil)
        {
            CN_Seguridad logica = new CN_Seguridad();
            bool result = logica.Desasignar(login, idPerfil);
            return Json(new { exito = result });
        }
    }
}