using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Monster_University_GR2.CapaEntidad;
using Monster_University_GR2.CapaNegocio;

namespace Monster_University_GR2.Controllers
{
    public class UsuariosController : Controller
    {
        // GET: Usuarios/Crear
        public IActionResult Crear()
        {
            CargarListas();
            return View();
        }

        private async Task GuardarFoto(IFormFile foto, string cedula)
        {
            if (foto != null && foto.Length > 0)
            {
                // 1. Definir la ruta: wwwroot/imagenes/usuarios
                string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "imagenes", "usuarios");

                // Crear carpeta si no existe
                if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);

                // 2. Definir nombre de archivo: SIEMPRE será {cedula}.jpg para reemplazar la anterior
                string filePath = Path.Combine(folderPath, $"{cedula}.jpeg");

                // 3. Guardar (Sobreescribe si ya existe)
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await foto.CopyToAsync(stream);
                }
            }
        }

        // POST: Usuarios/Crear
        [HttpPost]
        public async Task<IActionResult> Crear(UsuarioCrearCompletoViewModel modelo)
        {
            if (ModelState.IsValid)
            {
                CN_Usuario logica = new CN_Usuario();
                string mensaje = "";

                bool resultado = logica.RegistrarUsuarioDinámico(modelo, out mensaje);

                if (resultado)
                {
                    await GuardarFoto(modelo.FotoPerfil, modelo.Cedula);
                    TempData["NotificacionExito"] = $"El usuario {modelo.Nombre} {modelo.Apellido} ha sido creado correctamente.";                    // Por ahora recargamos la misma página limpia (luego iremos al Index/Tabla)
                    return RedirectToAction("Crear");
                }
                else
                {
                    ViewBag.NotificacionError = mensaje;
                }
            }
            else
            {
                // ERROR DE VALIDACIÓN (Ej: Campos vacíos) -> Nos quedamos aquí
                ViewBag.NotificacionError = "Hay campos incompletos o inválidos. Por favor revise el formulario.";
            }

            // Si falla, recargamos listas y devolvemos el modelo con errores
            CargarListas();
            return View(modelo);
        }

        // Método auxiliar para no repetir código
        private void CargarListas()
        {
            CN_Usuario logica = new CN_Usuario();
            ViewBag.ListaSexos = new SelectList(logica.ListarSexos(), "PsexCodigo", "PsexDescri");
            ViewBag.ListaEstadoCivil = new SelectList(logica.ListarEstados(), "PeescCodigo", "PeescDescri");
        }
        // GET: Usuarios/Index
        public IActionResult Index()
        {
            CN_Usuario logica = new CN_Usuario();
            var lista = logica.ObtenerListaUsuarios();

            return View(lista);
        }
        // ...
        // GET: Usuarios/Detalles/17...
        public IActionResult Detalles(string id)
        {
            if (string.IsNullOrEmpty(id)) return RedirectToAction("Index");

            CN_Usuario logica = new CN_Usuario();
            // Reutilizamos el método de obtención, nos sirve el modelo Editar para mostrar
            var modelo = logica.ObtenerParaEditar(id);

            if (modelo == null) return RedirectToAction("Index");

            return View(modelo);
        }

        // GET: Usuarios/Editar/17...
        public IActionResult Editar(string id)
        {
            if (string.IsNullOrEmpty(id)) return RedirectToAction("Index");

            CN_Usuario logica = new CN_Usuario();
            var modelo = logica.ObtenerParaEditar(id);

            if (modelo == null) return RedirectToAction("Index");

            CargarListas(); // Carga combos
            return View(modelo);
        }

        // POST: Usuarios/Editar
        [HttpPost]
        public async Task<IActionResult> Editar(UsuarioEditarViewModel modelo)
        {
            if (ModelState.IsValid)
            {
                CN_Usuario logica = new CN_Usuario();
                string mensaje = "";
                bool resultado = logica.Editar(modelo, out mensaje);

                if (resultado)
                {
                    if (modelo.FotoPerfil != null)
                    {
                        await GuardarFoto(modelo.FotoPerfil, modelo.Cedula);
                    }
                    TempData["Exito"] = "Usuario actualizado correctamente.";
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Error = mensaje;
                }
            }

            CargarListas();
            return View(modelo);
        }

        // GET: Usuarios/Eliminar/17...
        public IActionResult Eliminar(string id)
        {
            CN_Usuario logica = new CN_Usuario();
            string mensaje = "";

            bool resultado = logica.Eliminar(id, out mensaje);

            if (resultado)
            {
                TempData["Exito"] = "Usuario dado de baja correctamente.";
            }
            else
            {
                TempData["Error"] = mensaje; // Usamos TempData para que el mensaje sobreviva al Redirect
            }

            return RedirectToAction("Index");
        }
        // GET: Usuarios/ReporteSeguridad (La vista principal con botón imprimir)
        public IActionResult ReporteSeguridad()
        {
            CN_Usuario logica = new CN_Usuario();
            var lista = logica.ObtenerListaUsuarios(); // Reutilizamos el método existente
            return View(lista);
        }

        // GET: Usuarios/ReporteSeguridadPartial (Lo que va dentro del Modal)
        public IActionResult ReporteSeguridadPartial()
        {
            CN_Usuario logica = new CN_Usuario();
            var lista = logica.ObtenerListaUsuarios();
            return PartialView("_ReporteSeguridadPartial", lista);
        }

        // Método auxiliar para guardar la foto en el servidor

    }
}