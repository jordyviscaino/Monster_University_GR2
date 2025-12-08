using Microsoft.EntityFrameworkCore;
using Monster_University_GR2.CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Monster_University_GR2.CapaDatos
{
    public class CD_Seguridad
    {
        // 1. Listar Roles (PEROL_ROLES)
        // Concatenamos Departamento y Rol porque es llave compuesta
        public List<RolDTO> ListarRoles()
        {
            using (var db = new MonsterContext())
            {
                return db.PerolRoles
                    .Select(r => new RolDTO
                    {
                        // Guardamos "SYS|ADM" para saber ambos datos luego
                        CodigoCompuesto = r.PedepCodigo + "|" + r.PerolCodigo,
                        Descripcion = r.PerolDescri
                    }).ToList();
            }
        }

        // 2. Obtener Usuarios CON ese Rol (Tabla Derecha)
        // Buscamos en PEEMP_EMPLE
        public List<PerfilUsuarioDTO> ObtenerAsignados(string depCodigo, string rolCodigo)
        {
            using (var db = new MonsterContext())
            {
                var query = from empleado in db.PeempEmples
                            join persona in db.PeperPers on empleado.PeperCodigo equals persona.PeperCodigo
                            // Unimos con Usuario para sacar el Login
                            join usuario in db.XeusuUsuars on persona.PeperCodigo equals usuario.PeperCodigo
                            where empleado.PedepCodigo == depCodigo
                               && empleado.PerolCodigo == rolCodigo
                            select new PerfilUsuarioDTO
                            {
                                Login = usuario.XeusuLogin,
                                NombreCompleto = persona.PeperNombre + " " + persona.PeperApellido
                            };

                return query.ToList();
            }
        }

        // 3. Obtener Usuarios SIN ese Rol (Tabla Izquierda)
        public List<PerfilUsuarioDTO> ObtenerNoAsignados(string depCodigo, string rolCodigo)
        {
            using (var db = new MonsterContext())
            {
                // Subconsulta: Cédulas que YA tienen este rol asignado
                var cedulasAsignadas = db.PeempEmples
                                         .Where(e => e.PedepCodigo == depCodigo && e.PerolCodigo == rolCodigo)
                                         .Select(e => e.PeperCodigo);

                // Consulta: Usuarios Activos ('A') que NO están en la lista de arriba
                var query = from usuario in db.XeusuUsuars
                            join persona in db.PeperPers on usuario.PeperCodigo equals persona.PeperCodigo
                            where !cedulasAsignadas.Contains(persona.PeperCodigo)
                               && usuario.XeestCodigo == "A" // <-- Estado Correcto
                            select new PerfilUsuarioDTO
                            {
                                Login = usuario.XeusuLogin,
                                NombreCompleto = persona.PeperNombre + " " + persona.PeperApellido
                            };

                return query.ToList();
            }
        }

        // 4. ASIGNAR ROL (Insertar en PEEMP_EMPLE)
        public bool AsignarRol(string login, string depCodigo, string rolCodigo)
        {
            using (var db = new MonsterContext())
            {
                using (var transaccion = db.Database.BeginTransaction())
                {
                    try
                    {
                        // 1. Obtener el código de persona (Cédula) basado en el Login
                        var usuario = db.XeusuUsuars.FirstOrDefault(u => u.XeusuLogin == login);
                        if (usuario == null) return false;

                        // 2. Crear registro de Empleado
                        // NOTA: PEEMP_CODIGO es PK. Usaremos la misma Cédula + Rol para hacerlo único o la Cédula sola si es 1 a 1.
                        // Para simplificar y evitar errores de longitud, usaremos la Cédula como código de empleado temporalmente.
                        // Si un usuario puede tener varios roles, necesitaríamos lógica para generar un código único (ej: EMP001, EMP002).

                        // Validar si ya existe
                        bool existe = db.PeempEmples.Any(e => e.PeperCodigo == usuario.PeperCodigo && e.PerolCodigo == rolCodigo);
                        if (existe) return false;

                        var nuevoEmpleado = new PeempEmple
                        {
                            PeempCodigo = usuario.PeperCodigo, // Usamos la Cédula como ID de empleado
                            PeperCodigo = usuario.PeperCodigo,
                            PedepCodigo = depCodigo,
                            PerolCodigo = rolCodigo
                        };

                        db.PeempEmples.Add(nuevoEmpleado);
                        db.SaveChanges();
                        transaccion.Commit();
                        return true;
                    }
                    catch
                    {
                        transaccion.Rollback();
                        return false;
                    }
                }
            }
        }

        // 5. DESASIGNAR ROL (Borrar de PEEMP_EMPLE)
        public bool DesasignarRol(string login, string depCodigo, string rolCodigo)
        {
            using (var db = new MonsterContext())
            {
                try
                {
                    // Buscamos al usuario
                    var usuario = db.XeusuUsuars.FirstOrDefault(u => u.XeusuLogin == login);
                    if (usuario == null) return false;

                    // Buscamos el registro en Empleados
                    var empleado = db.PeempEmples
                        .FirstOrDefault(e => e.PeperCodigo == usuario.PeperCodigo
                                          && e.PedepCodigo == depCodigo
                                          && e.PerolCodigo == rolCodigo);

                    if (empleado != null)
                    {
                        db.PeempEmples.Remove(empleado);
                        db.SaveChanges();
                    }
                    return true;
                }
                catch { return false; }
            }
        }
    }
}