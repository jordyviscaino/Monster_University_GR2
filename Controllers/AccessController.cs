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
            CN_Usuario logica = new CN_Usuario();

            // 1. Validar credenciales
            UsuarioSesion usuario = logica.ValidarUsuario(userLogin, passLogin);

            if (usuario != null)
            {
                // === NUEVA LÓGICA DE SEGURIDAD ===
                // Verificar si tiene la bandera de "Cambiar Contraseña" activa ('S')
                if (usuario.DebeCambiarPassword == "S")
                {
                    // OJO: Aún no creamos esta vista, pero preparémoslo.
                    // Guardamos el correo en TempData para saber a quién cambiarle la clave
                    TempData["CorreoCambio"] = usuario.Email;

                    return RedirectToAction("CambiarClave", "Access");
                }
                // =================================

                // Si no tiene bandera, flujo normal (Dashboard)
                string usuarioJson = JsonSerializer.Serialize(usuario);
                HttpContext.Session.SetString("UsuarioSesion", usuarioJson);

                return RedirectToAction("Index", "Home");
            }
            else
            {
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
        // GET: Access/Recuperar
        // GET: Access/Recuperar
        public IActionResult Recuperar()
        {
            return View();
        }

        // POST: Access/Recuperar
        [HttpPost]
        public IActionResult Recuperar(string correo)
        {
            CN_Usuario logica = new CN_Usuario();
            string mensaje = "";

            bool respuesta = logica.RecuperarContrasena(correo, out mensaje);

            if (respuesta)
            {
                ViewBag.Exito = "Correo enviado exitosamente. Revisa tu bandeja de entrada.";
            }
            else
            {
                ViewBag.Error = mensaje;
            }

            return View();
        }
    }
}
