//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using Monster_University_GR2.CapaDatos; // Necesario para MonsterContext
//using Monster_University_GR2.CapaEntidad;
//using Monster_University_GR2.CapaNegocio; // para CN_Permisos
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text.Json; // para serializar sesión

//namespace Monster_University_GR2.Controllers
//{
//    public class SeguridadController : Controller
//    {
//        // Instancia de tu capa de datos existente (la que me mostraste al principio)
//        praivate CD_Seguridad _cdSeguridad = new CD_Seguridad();

//        // -----------------------------------------------------------
//        // VISTA PRINCIPAL
//        // -----------------------------------------------------------
//        public IActionResult AsignarRol()
//        {
//            // Llenar el ComboBox de Roles
//            var lista = _cdSeguridad.ListarRoles();
//            ViewBag.ListaRoles = new SelectList(lista, "CodigoCompuesto", "Descripcion");
//            return View();
//        }

//        // -----------------------------------------------------------
//        // PARTE 1: GESTIÓN DE USUARIOS (TABLA IZQUIERDA / DERECHA)
//        // -----------------------------------------------------------

//        [HttpGet]
//        public JsonResult ObtenerTablas(string idPerfil)
//        {
//            // idPerfil viene como "DEP|ROL" (Ej: "ADM|SEC") o solo "INV"
//            // Debemos separarlo si es compuesto
//            string dep = "";
//            string rol = "";

//            if (idPerfil.Contains("|"))
//            {
//                var partes = idPerfil.Split('|');
//                dep = partes[0];
//                rol = partes[1];
//            }
//            else
//            {
//                // Manejo de casos simples o errores
//                dep = "GEN";
//                rol = idPerfil;
//            }

//            // Usamos tus métodos de CD_Seguridad
//            var asignados = _cdSeguridad.ObtenerAsignados(dep, rol);
//            var disponibles = _cdSeguridad.ObtenerNoAsignados(dep, rol);

//            return Json(new { asignados = asignados, disponibles = disponibles });
//        }

//        [HttpPost]
//        public JsonResult Asignar(string login, string idPerfil)
//        {
//            string dep = "";
//            string rol = "";

//            if (idPerfil.Contains("|"))
//            {
//                var partes = idPerfil.Split('|');
//                dep = partes[0];
//                rol = partes[1];
//            }
//            else
//            {
//                return Json(new { exito = false, mensaje = "Formato de rol incorrecto" });
//            }

//            bool respuesta = _cdSeguridad.AsignarRol(login, dep, rol);
//            return Json(new { exito = respuesta });
//        }

//        [HttpPost]
//        public JsonResult Desasignar(string login)
//        {
//            bool respuesta = _cdSeguridad.DesasignarRol(login);
//            return Json(new { exito = respuesta });
//        }


//        // -----------------------------------------------------------
//        // PARTE 2: GESTIÓN DE MENÚ (CHECKBOXES / ARBOL) - ¡LO QUE DABA 404!
//        // -----------------------------------------------------------

//        [HttpGet]
//        public JsonResult ObtenerOpcionesPorRol(string idPerfil)
//        {
//            // NOTA: idPerfil aquí es el código del rol (Ej: "ADM", "SEC")
//            // Si el combo manda "GEN|ADM", necesitamos extraer solo el "ADM" para la tabla de permisos

//            string codigoRolReal = idPerfil;
//            if (idPerfil.Contains("|"))
//            {
//                var partes = idPerfil.Split('|');
//                codigoRolReal = partes[1]; // Tomamos la parte del ROL (Ej: ADM)
//            }

//            using (var db = new MonsterContext())
//            {
//                // 1. Obtener qué opciones tiene activas este rol
//                var permisosActuales = db.XeoxpOpcpes
//                    .Where(x => x.XeperCodigo == codigoRolReal && x.XeoxpFecret == null)
//                    .Select(x => x.XeopcCodigo).ToList();

//                // 2. Construir el árbol (Sistemas -> Opciones)
//                // Usamos los nombres reales de tu base: XESIS_CODIGO (1 char), XEOPC_CODIGO (3 char)
//                var arbol = db.XesisSistes
//                    .Select(s => new {
//                        sistemaNombre = s.XesisDescri,
//                        opciones = db.XeopcOpcios
//                            .Where(o => o.XesisCodigo == s.XesisCodigo)
//                            .Select(o => new {
//                                codigo = o.XeopcCodigo,
//                                nombre = o.XeopcDescri,
//                                // Marcado TRUE si ya existe en la lista de permisos
//                                marcado = permisosActuales.Contains(o.XeopcCodigo)
//                            }).ToList()
//                    }).ToList();

//                return Json(arbol);
//            }
//        }

//        [HttpPost]
//        public JsonResult GuardarPermisos(string idPerfil, List<string> opciones) // 'opciones' llega como lista de strings
//        {
//            // Limpieza del ID igual que arriba
//            string codigoRolReal = idPerfil;
//            if (idPerfil.Contains("|"))
//            {
//                var partes = idPerfil.Split('|');
//                codigoRolReal = partes[1];
//            }

//            using (var db = new MonsterContext())
//            {
//                using (var transaccion = db.Database.BeginTransaction())
//                {
//                    try
//                    {
//                        if (opciones == null) opciones = new List<string>();

//                        // A. Desactivar (Soft Delete) los que ya no están marcados
//                        var aBorrar = db.XeoxpOpcpes
//                            .Where(x => x.XeperCodigo == codigoRolReal
//                                     && x.XeoxpFecret == null
//                                     && !opciones.Contains(x.XeopcCodigo))
//                            .ToList();

//                        foreach (var item in aBorrar)
//                        {
//                            item.XeoxpFecret = DateTime.Now;
//                        }

//                        // B. Insertar los nuevos marcados
//                        // Primero vemos cuáles ya tenemos activos para no duplicar
//                        var existentes = db.XeoxpOpcpes
//                            .Where(x => x.XeperCodigo == codigoRolReal && x.XeoxpFecret == null)
//                            .Select(x => x.XeopcCodigo)
//                            .ToList();

//                        // Filtramos solo los nuevos
//                        var nuevos = opciones.Except(existentes);

//                        foreach (var codigo in nuevos)
//                        {
//                            db.XeoxpOpcpes.Add(new XeoxpOpcpe
//                            {
//                                XeperCodigo = codigoRolReal,
//                                XeopcCodigo = codigo,
//                                XeoxpFecasi = DateTime.Now,
//                                XeoxpFecret = null
//                            });
//                        }

//                        db.SaveChanges();
//                        transaccion.Commit();

//                        // --- ACTUALIZAR SESIÓN DEL USUARIO ACTUAL SI PERTENECE AL ROL MODIFICADO ---
//                        try
//                        {
//                            string usuarioJson = HttpContext.Session.GetString("UsuarioSesion");
//                            if (!string.IsNullOrEmpty(usuarioJson))
//                            {
//                                var usuarioSesion = JsonSerializer.Deserialize<UsuarioSesion>(usuarioJson);
//                                if (usuarioSesion != null && !string.IsNullOrEmpty(usuarioSesion.RolCodigo))
//                                {
//                                    // Si el usuario actual tiene el mismo rol que se modificó, recargar sus permisos
//                                    if (usuarioSesion.RolCodigo == codigoRolReal)
//                                    {
//                                        var cnPerm = new CN_Permisos();
//                                        var arbol = cnPerm.ObtenerOpcionesPorRol(codigoRolReal) ?? new List<SistemaMenuDTO>();
//                                        var codigosActivos = arbol
//                                            .SelectMany(s => s.Opciones ?? new List<OpcionMenuDTO>())
//                                            .Where(o => o.Activo)
//                                            .Select(o => o.Codigo)
//                                            .Distinct()
//                                            .ToList();

//                                        usuarioSesion.Permisos = codigosActivos;
//                                        // Reescribir sesión con permisos actualizados
//                                        HttpContext.Session.SetString("UsuarioSesion", JsonSerializer.Serialize(usuarioSesion));
//                                    }
//                                }
//                            }
//                        }
//                        catch
//                        {
//                            // No bloquear la operación principal si la actualización de sesión falla.
//                        }

//                        return Json(new { exito = true });
//                    }
//                    catch (Exception ex)
//                    {
//                        transaccion.Rollback();
//                        return Json(new { exito = false, mensaje = ex.Message });
//                    }
//                }
//            }
//        }
//    }
//}   