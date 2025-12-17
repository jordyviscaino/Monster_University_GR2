using Microsoft.EntityFrameworkCore;
using Monster_University_GR2.CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Monster_University_GR2.CapaDatos
{
    public class CD_Seguridad
    {
        // 1. LISTAR ROLES (Combo Box)
        public List<RolDTO> ListarRoles()
        {
            using (var db = new MonsterContext())
            {
                var lista = from rol in db.PerolRoles
                            join dep in db.PedepDepars on rol.PedepCodigo equals dep.PedepCodigo
                            where rol.PerolCodigo != "INV"
                            select new RolDTO
                            {
                                CodigoCompuesto = rol.PedepCodigo + "|" + rol.PerolCodigo,
                                Descripcion = dep.PedepDescri + " | " + rol.PerolDescri
                            };
                return lista.ToList();
            }
        }

        // 2. OBTENER ASIGNADOS (Tabla Derecha)
        public List<PerfilUsuarioDTO> ObtenerAsignados(string depCodigo, string rolCodigo)
        {
            using (var db = new MonsterContext())
            {
                var query = from emp in db.PeempEmples
                            join per in db.PeperPers on emp.PeperCodigo equals per.PeperCodigo
                            join usu in db.XeusuUsuars on per.PeperCodigo equals usu.PeperCodigo
                            where emp.PedepCodigo == depCodigo && emp.PerolCodigo == rolCodigo
                            select new PerfilUsuarioDTO
                            {
                                Login = usu.XeusuLogin,
                                NombreCompleto = per.PeperNombre + " " + per.PeperApellido
                            };

                return query.ToList();
            }
        }

        // 3. OBTENER DISPONIBLES (Tabla Izquierda)
        public List<PerfilUsuarioDTO> ObtenerNoAsignados(string depCodigo, string rolCodigo)
        {
            using (var db = new MonsterContext())
            {
                // Paso A: Obtener las Cédulas de quienes YA tienen el cargo seleccionado
                var cedulasOcupadas = db.PeempEmples
                                        .Where(e => e.PedepCodigo == depCodigo && e.PerolCodigo == rolCodigo)
                                        .Select(e => e.PeperCodigo)
                                        .ToList(); // Traemos a memoria para comparar strings limpiamente

                // Paso B: Consultar el Universo de Usuarios Activos
                // Usamos sintaxis de método para facilitar el Left Join y el filtrado
                var query = from usu in db.XeusuUsuars
                            join per in db.PeperPers on usu.PeperCodigo equals per.PeperCodigo
                            // LEFT JOIN con Empleados para ver su rol actual (si tienen)
                            join emp in db.PeempEmples on per.PeperCodigo equals emp.PeperCodigo into empGroup
                            from empActual in empGroup.DefaultIfEmpty()

                            where !cedulasOcupadas.Contains(usu.PeperCodigo) // Excluir a los que ya tienen el rol
                               && usu.XeestCodigo == "A" // Solo usuarios activos

                            select new PerfilUsuarioDTO
                            {
                                Login = usu.XeusuLogin,
                                // Formateamos el nombre para mostrar información útil
                                NombreCompleto = per.PeperNombre + " " + per.PeperApellido +
                                                 (empActual != null ? " [" + empActual.PerolCodigo + "]" : " [SIN ROL]")
                            };

                // El Distinct es vital aquí porque el Join puede multiplicar filas si hay inconsistencias
                return query.Distinct().ToList();
            }
        }

        // 4. ASIGNAR (Mover derecha)
        public bool AsignarRol(string login, string depDestino, string rolDestino)
        {
            using (var db = new MonsterContext())
            {
                using (var transaccion = db.Database.BeginTransaction())
                {
                    try
                    {
                        var usuario = db.XeusuUsuars.FirstOrDefault(u => u.XeusuLogin == login);
                        if (usuario == null) return false;

                        // --- ACTUALIZACIÓN RRHH (PEEMP_EMPLE) ---
                        var empleadoExistente = db.PeempEmples.FirstOrDefault(e => e.PeperCodigo == usuario.PeperCodigo);

                        if (empleadoExistente != null)
                        {
                            // UPDATE: Cambiamos de cargo
                            empleadoExistente.PedepCodigo = depDestino;
                            empleadoExistente.PerolCodigo = rolDestino;
                        }
                        else
                        {
                            // INSERT: Nuevo contrato
                            var nuevoEmpleado = new PeempEmple
                            {
                                PeempCodigo = usuario.PeperCodigo,
                                PeperCodigo = usuario.PeperCodigo,
                                PedepCodigo = depDestino,
                                PerolCodigo = rolDestino
                            };
                            db.PeempEmples.Add(nuevoEmpleado);
                        }

                        // --- ACTUALIZACIÓN SEGURIDAD (XEUXP_USUPE) ---
                        SincronizarPerfilSistema(db, login, rolDestino);

                        db.SaveChanges();
                        transaccion.Commit();
                        return true;
                    }
                    catch (Exception)
                    {
                        transaccion.Rollback();
                        return false;
                    }
                }
            }
        }

        // 5. DESASIGNAR (Mover izquierda)
        public bool DesasignarRol(string login)
        {
            return AsignarRol(login, "GEN", "INV");
        }

        // MÉTODO PRIVADO CORREGIDO: Propiedades reales de tu BD
        private void SincronizarPerfilSistema(MonsterContext db, string login, string rolRRHH)
        {
            // 1. Buscar perfil activo anterior
            // CORRECCIÓN: Usamos XeuxpFeccad (Fecha Caducidad) en lugar de Fecret
            var perfilAnterior = db.XeuxpUsupes
                                   .FirstOrDefault(x => x.XeusuLogin == login && x.XeuxpFecret == null);

            if (perfilAnterior != null)
            {
                db.XeuxpUsupes.Remove(perfilAnterior);
            }

            // 2. Determinar nuevo perfil
            string nuevoPerfil = rolRRHH;
            if (!db.XeperPerfis.Any(p => p.XeperCodigo == nuevoPerfil))
            {
                nuevoPerfil = "INV"; // Fallback si no existe mapeo
            }

            // 3. Insertar nuevo
            db.XeuxpUsupes.Add(new XeuxpUsupe
            {
                XeusuLogin = login,
                XeperCodigo = nuevoPerfil,
                XeuxpFecasi = DateTime.Now,
            });
        }
    }
}