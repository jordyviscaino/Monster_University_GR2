using Microsoft.AspNetCore.Mvc;
using Monster_University_GR2.CapaEntidad; // Importante
using System.Text.Json; // Importante para leer la sesión

namespace Monster_University_GR2.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            // 1. Validar si existe la sesión (Si no, patear al login)
            var sessionString = HttpContext.Session.GetString("UsuarioSesion");

            if (string.IsNullOrEmpty(sessionString))
            {
                return RedirectToAction("Login", "Access");
            }

            // 2. Deserializar el JSON a objeto UsuarioSesion
            UsuarioSesion usuario = JsonSerializer.Deserialize<UsuarioSesion>(sessionString);

            // 3. Enviar a la vista
            return View(usuario);
        }
    }
}