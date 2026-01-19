using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;


using Monster_University_GR2.CapaNegocio;
using Monster_University_GR2.CapaEntidad;
using Monster_University_GR2.Models;

namespace Monster_University_GR2.Controllers
{
    public class AccessController : Controller
    {
        private readonly ServicioAcceso _servicioAcceso;
        private readonly ServicioCorreo _servicioCorreo;

        public AccessController(ServicioAcceso servicioAcceso, ServicioCorreo servicioCorreo)
        {
            _servicioAcceso = servicioAcceso;
            _servicioCorreo = servicioCorreo;
        }

        [HttpGet]
        public IActionResult Login()
        {
            // Si ya hay sesión, ir al inicio
            if (HttpContext.Session.GetString("UsuarioSesion") != null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string userLogin, string passLogin)
        {
            Usuario usuario = await _servicioAcceso.ValidarLogin(userLogin, passLogin);

            if (usuario != null)
            {

                if (usuario.DebeCambiarPwd == "S")
                {
                    TempData["EmailPendiente"] = usuario.Email;
                    return RedirectToAction("CambiarClave");
                }

                UsuarioSesion sesionModel = new UsuarioSesion
                {
                    Id = usuario.Id,
                    Nombre = usuario.Nombres,
                    Apellido = usuario.Apellidos,
                    Email = usuario.Email,
                    RolCodigo = usuario.Roles.FirstOrDefault() ?? "INV", // Rol principal
                    Permisos = new List<string>() // Aquí luego cargaremos permisos de security_profiles
                };

                string jsonSesion = JsonSerializer.Serialize(sesionModel);
                HttpContext.Session.SetString("UsuarioSesion", jsonSesion);

                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Credenciales incorrectas o usuario inactivo.";
            return View();
        }

    
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegistroViewModel modelo)
        {
            if (!ModelState.IsValid)
            {
                return View(modelo);
            }

            string resultado = await _servicioAcceso.RegistrarUsuarioInvitado(
                modelo.Cedula,
                modelo.Nombres,
                modelo.Apellidos,  // Ya no dará error
                modelo.Email,
                modelo.TelefonoCelular, // Ya no dará error
                modelo.Password
            );

            if (resultado == "OK")
            {
                ViewBag.Exito = "Cuenta creada correctamente.";
                return View("Login");
            }
            else
            {
                ViewBag.Error = resultado;
                return View(modelo);
            }
        }

[HttpGet]
        public IActionResult Recuperar()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Recuperar(string correo)
        {
            // 1. Generar clave temporal y actualizar BD (flag: 'S')
            string claveTemporal = await _servicioAcceso.RecuperarContrasena(correo);

            if (claveTemporal != null)
            {
                // 2. Enviar correo
                string asunto = "Recuperación de Acceso - Monster University";
                string cuerpo = $@"
                    <h3>Restablecimiento de Contraseña</h3>
                    <p>Has solicitado recuperar tu acceso al sistema académico.</p>
                    <p>Tu contraseña temporal es: <strong>{claveTemporal}</strong></p>
                    <p>Por seguridad, el sistema te pedirá cambiarla inmediatamente al ingresar.</p>
                    <hr>
                    <small>Si no solicitaste esto, contacta a Soporte TI.</small>";

                await _servicioCorreo.EnviarCorreo(correo, asunto, cuerpo);

                ViewBag.Exito = "Se ha enviado una contraseña temporal a tu correo.";
            }
            else
            {
            
                ViewBag.Error = "El correo ingresado no se encuentra registrado.";
            }

            return View();
        }

    
        [HttpGet]
        public IActionResult CambiarClave()
        {
            string email = TempData["EmailPendiente"] as string;

            if (string.IsNullOrEmpty(email))
            {
                string jsonSesion = HttpContext.Session.GetString("UsuarioSesion");
                if (!string.IsNullOrEmpty(jsonSesion))
                {
                    var sesion = JsonSerializer.Deserialize<UsuarioSesion>(jsonSesion);
                    email = sesion.Email;
                }
            }

            if (string.IsNullOrEmpty(email))
            {
                return RedirectToAction("Login");
            }

            CambiarClaveViewModel model = new CambiarClaveViewModel { Email = email };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CambiarClave(CambiarClaveViewModel modelo)
        {
            if (!ModelState.IsValid)
            {
                return View(modelo);
            }

            bool resultado = await _servicioAcceso.CambiarContrasena(modelo.Email, modelo.NewPassword);

            if (resultado)
            {
           
                ViewBag.Exito = "Contraseña actualizada. Ingresa nuevamente.";
                return View("Login");
            }

            ViewBag.Error = "Error al actualizar la contraseña.";
            return View(modelo);
        }

  
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}