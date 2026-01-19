using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http; // Para Session
using System.Text.Json;        // Para JsonSerializer
using Monster_University_GR2.Models; // <--- ESTA ES LA LÍNEA CLAVE QUE FALTABA

namespace Monster_University_GR2.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            // 1. Validar si existe la sesión
            var sessionString = HttpContext.Session.GetString("UsuarioSesion");

            if (string.IsNullOrEmpty(sessionString))
            {
                // Si no hay sesión, mandar al login
                return RedirectToAction("Login", "Access");
            }

            // 2. Deserializar: Ahora sí reconocerá "UsuarioSesion" gracias al using de arriba
            UsuarioSesion usuario = JsonSerializer.Deserialize<UsuarioSesion>(sessionString);

            // 3. Retornar vista
            return View(usuario);
        }
    }
}