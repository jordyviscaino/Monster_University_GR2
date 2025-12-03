using Microsoft.AspNetCore.Mvc;
using Monster_University_GR2.CapaNegocio; // Referencia a Negocio
using Monster_University_GR2.CapaEntidad; // Referencia a Entidad
using System.Text.Json; // Para guardar el objeto en sesión

namespace Monster_University_GR2.Controllers
{
    public class AccessController : Controller
    {
        // GET: Access/Login
        public IActionResult Login()
        {
            // Validar si ya hay sesión (Evitar doble login)
            if (HttpContext.Session.GetString("UsuarioSesion") != null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // POST: Access/Login
        [HttpPost]
        public IActionResult Login(string userLogin, string passLogin)
        {
            // 1. Instanciar la Capa de Negocio
            CN_Usuario logica = new CN_Usuario();

            // 2. Llamar al método de validación
            UsuarioSesion usuario = logica.ValidarUsuario(userLogin, passLogin);

            if (usuario != null)
            {
                // 3. ¡ÉXITO! Guardar en Sesión
                // En .NET 8 Session guarda bytes o strings, así que serializamos a JSON
                string usuarioJson = JsonSerializer.Serialize(usuario);
                HttpContext.Session.SetString("UsuarioSesion", usuarioJson);

                return RedirectToAction("Index", "Home");
            }
            else
            {
                // 4. Fallo
                ViewBag.Error = "Credenciales incorrectas o usuario inactivo.";
                return View();
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // Borrar sesión
            return RedirectToAction("Login");
        }

        // GET: Access/Register
        public IActionResult Register()
        {
            CN_Usuario logica = new CN_Usuario();
            // Llenar ViewBags para los dropdowns
            ViewBag.ListaSexos = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(logica.ListarSexos(), "PsexCodigo", "PsexDescri");
            ViewBag.ListaEstadoCivil = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(logica.ListarEstados(), "PeescCodigo", "PeescDescri");

            return View();
        }

        // POST: Access/Register
        [HttpPost]
        public IActionResult Register(RegistroViewModel modelo)
        {
            CN_Usuario logica = new CN_Usuario();

            if (ModelState.IsValid)
            {
                string mensaje = "";
                bool respuesta = logica.Registrar(modelo, out mensaje);

                if (respuesta)
                {
                    ViewBag.Exito = "Usuario creado correctamente. Ya puede iniciar sesión.";
                    return View("Login"); // O RedirectToAction("Login")
                }
                else
                {
                    ViewBag.Error = mensaje;
                }
            }

            // Recargar listas si falla
            ViewBag.ListaSexos = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(logica.ListarSexos(), "PsexCodigo", "PsexDescri");
            ViewBag.ListaEstadoCivil = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(logica.ListarEstados(), "PeescCodigo", "PeescDescri");

            return View(modelo);
        }
    }
}
