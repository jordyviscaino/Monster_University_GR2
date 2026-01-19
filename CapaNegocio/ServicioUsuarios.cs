using MongoDB.Driver;
using Monster_University_GR2.CapaDatos;
using Monster_University_GR2.CapaEntidad;
using Monster_University_GR2.Colecciones;
using Monster_University_GR2.Models;
using Monster_University_GR2.CapaNegocio.Recursos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Monster_University_GR2.CapaNegocio
{
    public class ServicioUsuarios
    {
        private readonly UsuariosCollection _usuariosCollection;
        private readonly IMongoCollection<SexoCatalogo> _sexos;
        private readonly IMongoCollection<EstadoCivilCatalogo> _estadosCiviles;

        public ServicioUsuarios(UsuariosCollection usuariosCollection, ContextoMongo contexto)
        {
            _usuariosCollection = usuariosCollection;
            _sexos = contexto.ObtenerColeccion<SexoCatalogo>("sexos");
            _estadosCiviles = contexto.ObtenerColeccion<EstadoCivilCatalogo>("estados_civiles");
        }

        // ==========================================
        // MÉTODOS AUXILIARES (Listas)
        // ==========================================
        public async Task<List<SelectListItem>> ObtenerListaSexos()
        {
            var lista = await _sexos.Find(_ => true).ToListAsync();
            return lista.Select(x => new SelectListItem { Value = x.Codigo, Text = x.Descripcion }).ToList();
        }

        public async Task<List<SelectListItem>> ObtenerListaEstadosCiviles()
        {
            var lista = await _estadosCiviles.Find(_ => true).ToListAsync();
            return lista.Select(x => new SelectListItem { Value = x.Codigo, Text = x.Descripcion }).ToList();
        }

        public List<KeyValuePair<string, string>> ObtenerPerfilesSistema()
        {
            return new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("ADM", "Administrador (Control Total)"),
                new KeyValuePair<string, string>("DOC", "Docente (Gestión Académica)"),
                new KeyValuePair<string, string>("SEC", "Secretaria (Gestión Administrativa)"),
                new KeyValuePair<string, string>("EST", "Estudiante (Acceso Limitado)")
            };
        }

        // ==========================================
        // METODO PRIVADO: NORMALIZAR ESTADO CIVIL
        // ==========================================
        // Este método evita que Mongo rechace "UNIÓN LIBRE"
        private string NormalizarEstadoCivilParaMongo(string descripcion)
        {
            if (string.IsNullOrEmpty(descripcion)) return "SOLTERO";

            string valorUpper = descripcion.ToUpper();

            // La validación de Mongo solo acepta: SOLTERO, CASADO, DIVORCIADO, VIUDO
            // Si es Unión Libre, lo mapeamos a SOLTERO (o CASADO si prefieres) para evitar el crash.
            if (valorUpper.Contains("LIBRE") || valorUpper.Contains("UNION"))
            {
                return "SOLTERO"; // Ajuste de compatibilidad
            }

            return valorUpper;
        }

        // ==========================================
        // CREAR USUARIO
        // ==========================================
        public async Task<string> CrearUsuarioCompleto(UsuarioCrearCompletoViewModel model)
        {
            // 1. Validaciones Previas
            var existe = await _usuariosCollection.ObtenerPorEmail(model.Email);
            if (existe != null) return "El correo ya existe.";
            if (await _usuariosCollection.ObtenerPorCedula(model.Cedula) != null) return "La cédula ya existe.";

            // 2. Normalizar Estado Civil (Evitar error Code 121 de Mongo)
            var estadoCivilObj = await _estadosCiviles.Find(x => x.Codigo == model.EstadoCivilCodigo).FirstOrDefaultAsync();
            string descRaw = estadoCivilObj != null ? estadoCivilObj.Descripcion : "SOLTERO";
            string estadoCivilFinal = NormalizarEstadoCivilParaMongo(descRaw);

            // 3. Crear Objeto Base
            Usuario u = new Usuario
            {
                Cedula = model.Cedula,
                Nombres = model.Nombres.ToUpper(),
                Apellidos = model.Apellidos.ToUpper(),
                Email = model.Email,
                PasswordHash = Utilidades.EncriptarClave(model.Password),
                Estado = "ACTIVO",
                DebeCambiarPwd = "S",
                Roles = new List<string>()
            };

            u.DatosPersonales = new DatosPersonales
            {
                Direccion = model.Direccion,
                Telefono = model.Telefono,
                SexoCodigo = model.SexoCodigo,
                EstadoCivilCodigo = estadoCivilFinal, // Usamos la versión segura
                FechaNacimiento = model.FechaNacimiento
            };

            // 4. Lógica según Tipo de Usuario
            if (model.TipoUsuario == "EST")
            {
                // ESTUDIANTE:
                // Asignamos rol EST
                u.Roles.Add("EST");

                // Asignamos datos académicos "dummy" o por defecto, ya que es Work In Progress
                u.DatosAcademicos = new DatosAcademicos
                {
                    CarreraId = "SOFTWARE_DEFAULT", // Valor interno por defecto
                    Semestre = 1
                };

                // Info empleado vacía o genérica
                u.InfoEmpleado = new InfoEmpleado { Departamento = "ACADEMICO", Cargo = "ESTUDIANTE" };
            }
            else
            {
                // EMPLEADO / DOCENTE:
                // Si no seleccionó rol (aunque debería), ponemos INV (Invitado)
                string rolAsignar = !string.IsNullOrEmpty(model.RolSeleccionado) ? model.RolSeleccionado : "INV";
                u.Roles.Add(rolAsignar);

                // Autocompletamos Departamento y Cargo basado en el Rol para que Mongo no quede vacío
                string depto = "ADMINISTRACION";
                string cargo = "ASISTENTE";

                if (rolAsignar == "DOC") { depto = "DOCENCIA"; cargo = "DOCENTE TITULAR"; }
                if (rolAsignar == "ADM") { depto = "RECTORADO"; cargo = "ADMINISTRADOR"; }
                if (rolAsignar == "SEC") { depto = "SECRETARIA"; cargo = "SECRETARIA GENERAL"; }

                u.InfoEmpleado = new InfoEmpleado
                {
                    Departamento = depto,
                    Cargo = cargo
                };

                u.DatosAcademicos = null;
            }

            try
            {
                await _usuariosCollection.InsertarUsuario(u);
                return "OK";
            }
            catch (Exception ex)
            {
                return "Error Mongo: " + ex.Message;
            }
        }

        // ==========================================
        // OBTENER PARA EDITAR
        // ==========================================
        public async Task<UsuarioEditarViewModel> ObtenerParaEditar(string cedula)
        {
            var u = await _usuariosCollection.ObtenerPorCedula(cedula);
            if (u == null) return null;

            // Recuperar el código (S, C, D) basado en la descripción guardada
            var estadoEnBD = u.DatosPersonales.EstadoCivilCodigo;

            // Buscamos coincidencia exacta o parcial
            var catEstado = await _estadosCiviles.Find(x => x.Descripcion.ToUpper() == estadoEnBD).FirstOrDefaultAsync();

            // Si no encuentra (por ejemplo si guardamos SOLTERO en vez de UNION LIBRE), devolvemos S por defecto
            string codigoEstado = catEstado != null ? catEstado.Codigo : "S";

            return new UsuarioEditarViewModel
            {
                Cedula = u.Cedula,
                Nombres = u.Nombres,
                Apellidos = u.Apellidos,
                Email = u.Email,
                Direccion = u.DatosPersonales.Direccion,
                Telefono = u.DatosPersonales.Telefono,
                EstadoUsuario = u.Estado,
                FechaNacimiento = u.DatosPersonales.FechaNacimiento,
                SexoCodigo = u.DatosPersonales.SexoCodigo,
                EstadoCivilCodigo = codigoEstado,
                Rol = string.Join(",", u.Roles)
            };
        }

        // ==========================================
        // ACTUALIZAR USUARIO
        // ==========================================
        public async Task<bool> ActualizarUsuario(UsuarioEditarViewModel model)
        {
            var u = await _usuariosCollection.ObtenerPorCedula(model.Cedula);
            if (u == null) return false;

            // 1. Obtener Descripción
            var estadoObj = await _estadosCiviles.Find(x => x.Codigo == model.EstadoCivilCodigo).FirstOrDefaultAsync();
            string descRaw = estadoObj != null ? estadoObj.Descripcion : "SOLTERO";

            // 2. Normalizar para cumplir regla estricta de Mongo (Corrección del error 121)
            string estadoCivilFinal = NormalizarEstadoCivilParaMongo(descRaw);

            u.DatosPersonales.Direccion = model.Direccion;
            u.DatosPersonales.Telefono = model.Telefono;
            u.DatosPersonales.SexoCodigo = model.SexoCodigo;
            u.DatosPersonales.EstadoCivilCodigo = estadoCivilFinal; // Guardamos el valor seguro
            u.DatosPersonales.FechaNacimiento = model.FechaNacimiento;
            u.Estado = model.EstadoUsuario;

            await _usuariosCollection.ActualizarDatosGenerales(u);
            return true;
        }

        public async Task<List<UsuarioResumenViewModel>> ObtenerTodos()
        {
            var listaBD = await _usuariosCollection.ObtenerTodos();
            return listaBD.Select(u => new UsuarioResumenViewModel
            {
                Cedula = u.Cedula,
                NombresCompletos = $"{u.Nombres} {u.Apellidos}",
                Email = u.Email,
                RolPrincipal = (u.Roles != null && u.Roles.Count > 0) ? u.Roles[0] : "S/R",
                Estado = u.Estado,
                FechaRegistro = DateTime.Now.ToShortDateString()
            }).ToList();
        }

        public async Task<bool> EliminarUsuario(string cedula)
        {
            return await _usuariosCollection.EliminarPorCedula(cedula);
        }

        // ... (Tus otros métodos existen aquí) ...

        // 1. LISTAR SOLO ESTUDIANTES (Para la pantalla de búsqueda)
        public async Task<List<EstudianteResumenDTO>> ObtenerSoloEstudiantes()
        {
            // Filtramos quienes tengan el rol "EST"
            var todos = await _usuariosCollection.ObtenerTodos();
            var estudiantes = todos.Where(u => u.Roles.Contains("EST")).ToList();

            return estudiantes.Select(e => new EstudianteResumenDTO
            {
                CodigoEstudiante = e.Cedula,
                NombreCompleto = $"{e.Nombres} {e.Apellidos}",
                // Si DatosAcademicos es null, ponemos algo por defecto
                Carrera = (e.DatosAcademicos != null) ? e.DatosAcademicos.CarreraId : "Ingeniería Software"
            }).ToList();
        }

        // 2. GENERAR BOLETÍN (SIMULADO VISUALMENTE)
        public async Task<BoletinEstudianteViewModel> GenerarBoletinSimulado(string cedula)
        {
            var u = await _usuariosCollection.ObtenerPorCedula(cedula);
            if (u == null) return null;

            // Creamos el contenedor del reporte
            var reporte = new BoletinEstudianteViewModel
            {
                Cedula = u.Cedula,
                Nombres = $"{u.Nombres} {u.Apellidos}",
                Carrera = (u.DatosAcademicos != null) ? u.DatosAcademicos.CarreraId : "INGENIERÍA DE SOFTWARE",
                Periodo = "2024 - 2025",
                FechaGeneracion = DateTime.Now.ToString("dd/MM/yyyy HH:mm"),
                Materias = new List<DetalleNotaViewModel>()
            };

            // DATOS FALSOS (MOCK) PARA PROBAR EL DISEÑO VISUAL
            // Cuando tengas base de datos de notas, esto se reemplazará por una consulta real.
            reporte.Materias.Add(new DetalleNotaViewModel { Materia = "CÁLCULO DIFERENCIAL", Nota1 = 8.5, Nota2 = 9.0, Nota3 = 8.0, Promedio = 8.5 });
            reporte.Materias.Add(new DetalleNotaViewModel { Materia = "PROGRAMACIÓN I", Nota1 = 10, Nota2 = 9.5, Nota3 = 10, Promedio = 9.83 });
            reporte.Materias.Add(new DetalleNotaViewModel { Materia = "COMUNICACIÓN ORAL", Nota1 = 7.0, Nota2 = 6.5, Nota3 = 7.0, Promedio = 6.83 });
            reporte.Materias.Add(new DetalleNotaViewModel { Materia = "FÍSICA CLÁSICA", Nota1 = 6.0, Nota2 = 8.0, Nota3 = 9.0, Promedio = 7.6 });

            return reporte;
        }

    }
}