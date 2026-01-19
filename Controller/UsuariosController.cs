using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering; // Necesario para SelectList
using Monster_University_GR2.CapaNegocio;
using Monster_University_GR2.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq; // Necesario para el LINQ en Roles

namespace Monster_University_GR2.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly ServicioUsuarios _servicioUsuarios;

        public UsuariosController(ServicioUsuarios servicioUsuarios)
        {
            _servicioUsuarios = servicioUsuarios;
        }

        private async Task CargarListasViewBag()
        {
            // 1. Listas desde MongoDB (Asíncronas)
            ViewBag.ListaSexos = await _servicioUsuarios.ObtenerListaSexos();
            ViewBag.ListaEstadoCivil = await _servicioUsuarios.ObtenerListaEstadosCiviles();

            // 2. Roles (Desde lógica del servicio, filtrando Estudiante)
            var roles = _servicioUsuarios.ObtenerPerfilesSistema()
                        .Where(x => x.Key != "EST")
                        .Select(x => new SelectListItem { Value = x.Key, Text = x.Value })
                        .ToList();
            ViewBag.ListaRoles = roles;

            // 3. Carreras (Simuladas / Hardcoded)
            ViewBag.ListaCarreras = new List<SelectListItem>
            {
                new SelectListItem { Value = "ING_SOFT", Text = "Ingeniería de Software" },
                new SelectListItem { Value = "ING_TI", Text = "Tecnologías de la Información" },
                new SelectListItem { Value = "DIS_GRAF", Text = "Diseño Gráfico" },
                new SelectListItem { Value = "BIOTEC", Text = "Biotecnología" }
            };

            // 4. Departamentos (Simulados / Hardcoded)
            ViewBag.ListaDepartamentos = new List<SelectListItem>
            {
                 new SelectListItem { Value = "ACA", Text = "Académico" },
                 new SelectListItem { Value = "ADM", Text = "Administrativo" },
                 new SelectListItem { Value = "FIN", Text = "Financiero" }
            };
        }

        // GET: Index (Listado)
        public async Task<IActionResult> Index()
        {
            var lista = await _servicioUsuarios.ObtenerTodos();
            return View(lista);
        }

        // GET: Crear
        public async Task<IActionResult> Crear()
        {
            await CargarListasViewBag();
            return View();
        }

        // POST: Crear
        [HttpPost]
        public async Task<IActionResult> Crear(UsuarioCrearCompletoViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await CargarListasViewBag();
                return View(model);
            }

            string res = await _servicioUsuarios.CrearUsuarioCompleto(model);

            if (res == "OK")
            {
                TempData["Exito"] = "Usuario creado correctamente.";
                return RedirectToAction("Index");
            }

            ViewBag.Error = res; // Error de lógica (ej. correo duplicado)
            await CargarListasViewBag();
            return View(model);
        }

        // GET: Editar
        public async Task<IActionResult> Editar(string id)
        {
            if (string.IsNullOrEmpty(id)) return RedirectToAction("Index");

            var model = await _servicioUsuarios.ObtenerParaEditar(id);
            if (model == null) return RedirectToAction("Index");

            await CargarListasViewBag(); // Importante para que los selects tengan datos
            return View(model);
        }

        // POST: Editar
        [HttpPost]
        public async Task<IActionResult> Editar(UsuarioEditarViewModel model)
        {
            // Nota: Si los campos readonly (Nombre/Apellido) no vienen en el POST,
            // ModelState podría dar error si son [Required].
            // En ese caso, habría que limpiar el error específico o llenar datos dummy.
            // Por ahora probamos así:

            if (!ModelState.IsValid)
            {
                await CargarListasViewBag();
                return View(model);
            }

            await _servicioUsuarios.ActualizarUsuario(model);
            TempData["Exito"] = "Usuario actualizado correctamente.";
            return RedirectToAction("Index");
        }

        // GET: Detalles
        [HttpGet]
        public async Task<IActionResult> Detalles(string id)
        {
            if (string.IsNullOrEmpty(id)) return RedirectToAction("Index");

            var modelo = await _servicioUsuarios.ObtenerParaEditar(id);

            if (modelo == null)
            {
                TempData["Error"] = "No se encontró el usuario solicitado.";
                return RedirectToAction("Index");
            }

            return View(modelo);
        }

        // GET: Eliminar (Llamado desde JS)
        public async Task<IActionResult> Eliminar(string id)
        {
            await _servicioUsuarios.EliminarUsuario(id);
            TempData["Exito"] = "Usuario eliminado.";
            return RedirectToAction("Index");
        }
    }
}