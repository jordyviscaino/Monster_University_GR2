using Microsoft.EntityFrameworkCore; // Necesario para .ToList() async o normal
using Monster_University_GR2.CapaEntidad;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Monster_University_GR2.CapaDatos
{
    public class CD_Usuario
    {
        // Método para validar acceso llamando al SP
        public UsuarioSesion ValidarAcceso(string login)
        {
      
            // Usaremos una instancia nueva del contexto generado
            using (var db = new MonsterContext())
            {

                // Asumiendo que el SP devuelve las columnas tal cual las definimos:
                var usuario = db.Database.SqlQueryRaw<UsuarioSesion>(
                    "EXEC sp_ValidarAcceso @UsuarioIngresado = {0}", login
                ).AsEnumerable().FirstOrDefault();

                return usuario;
            }
        }
        public bool RegistrarUsuario(PeperPer persona, XeusuUsuar usuario, out string mensaje)
        {
            mensaje = string.Empty;
            using (var db = new MonsterContext())
            {
                // Iniciar Transacción (EF Core)
                using (IDbContextTransaction transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        // 1. Validaciones directas en BD
                        if (db.PeperPers.Any(p => p.PeperCedula == persona.PeperCedula))
                        {
                            mensaje = "La cédula ya existe.";
                            return false;
                        }
                        if (db.PeperPers.Any(p => p.PeperEmail == persona.PeperEmail))
                        {
                            mensaje = "El correo ya está registrado.";
                            return false;
                        }

                        // 2. Insertar Persona
                        db.PeperPers.Add(persona);
                        db.SaveChanges(); // Genera el ID si fuera identity (aquí usamos cédula)

                        // 3. Insertar Usuario
                        // Aseguramos la relación
                        usuario.PeperCodigo = persona.PeperCodigo;
                        db.XeusuUsuars.Add(usuario);
                        db.SaveChanges();

                        // 4. Confirmar
                        transaction.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        mensaje = "Error al registrar: " + ex.Message;
                        return false;
                    }
                }
            }
        }

        public bool RestablecerClave(string correo, string claveHasheada, out string mensaje)
        {
            mensaje = string.Empty;
            using (var db = new MonsterContext())
            {
                try
                {
                    // 1. Buscar Usuario (Por Login o por Correo de Persona)
                    var usuario = db.XeusuUsuars.FirstOrDefault(u => u.XeusuLogin == correo);

                    if (usuario == null)
                    {
                        // Si no está por login, buscamos en la tabla Persona
                        var persona = db.PeperPers.FirstOrDefault(p => p.PeperEmail == correo);
                        if (persona != null)
                        {
                            usuario = db.XeusuUsuars.FirstOrDefault(u => u.PeperCodigo == persona.PeperCodigo);
                        }
                    }

                    if (usuario == null)
                    {
                        mensaje = "No se encontró un usuario con ese correo.";
                        return false;
                    }

                    // 2. Actualizar Datos
                    usuario.XeusuPaswd = claveHasheada;
                    usuario.XeusuCambiarPwd = "S"; // <--- BANDERA IMPORTANTE
                    usuario.XeusuFecmod = DateTime.Now;

                    // 3. Guardar
                    db.Entry(usuario).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    db.SaveChanges();

                    return true;
                }
                catch (Exception ex)
                {
                    mensaje = "Error en BD: " + ex.Message;
                    return false;
                }
            }
        }
        // ...
        public bool ActualizarPassword(string correo, string nuevaClaveHash, out string mensaje)
        {
            mensaje = string.Empty;
            using (var db = new MonsterContext())
            {
                try
                {
                    var usuario = db.XeusuUsuars.FirstOrDefault(u => u.XeusuLogin == correo);

                    if (usuario == null)
                    {
                        mensaje = "Usuario no encontrado.";
                        return false;
                    }

                    usuario.XeusuPaswd = nuevaClaveHash;
                    usuario.XeusuCambiarPwd = "N"; // ¡IMPORTANTE! Apagamos la obligación
                    usuario.XeusuFecmod = DateTime.Now;

                    db.Entry(usuario).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    db.SaveChanges();

                    return true;
                }
                catch (Exception ex)
                {
                    mensaje = "Error al actualizar: " + ex.Message;
                    return false;
                }
            }
        }

public List<UsuarioResumenViewModel> ListarUsuarios()
    {
        using (var db = new MonsterContext())
        {
            // LINQ con JOIN implícito (Include)
            var lista = db.XeusuUsuars
                          .Include(u => u.PeperCodigoNavigation) // Join con Persona
                          .Select(u => new UsuarioResumenViewModel
                          {
                              Cedula = u.PeperCodigo,
                              // Concatenamos nombre y apellido
                              NombreCompleto = u.PeperCodigoNavigation.PeperNombre + " " + u.PeperCodigoNavigation.PeperApellido,
                              Email = u.XeusuLogin,
                              Estado = u.XeestCodigo,
                              FechaRegistro = u.XeusuFeccre,
                              Rol = "N/A" // Lo ajustaremos cuando integremos perfiles
                          })
                          .ToList();

            return lista;
        }
    }
   public UsuarioCrearCompletoViewModel ObtenerUsuarioCompleto(string cedula)
        {
            using (var db = new MonsterContext())
            {
                // Buscamos la persona y su usuario asociado
                var persona = db.PeperPers.FirstOrDefault(p => p.PeperCodigo == cedula);
                var usuario = db.XeusuUsuars.FirstOrDefault(u => u.PeperCodigo == cedula);

                if (persona == null || usuario == null) return null;

                // Mapeamos a un ViewModel lleno (Usamos el de Crear porque tiene todos los campos)
                return new UsuarioCrearCompletoViewModel
                {
                    Cedula = persona.PeperCedula,
                    Nombre = persona.PeperNombre,
                    Apellido = persona.PeperApellido,
                    Email = usuario.XeusuLogin,
                    FechaNacimiento = persona.PeperFechanaci,
                    Direccion = persona.PeperDireccion,
                    Celular = persona.PeperCelular,
                    TelefonoDomicilio = persona.PeperTeldom,
                    CargasFamiliares = (int)persona.PeperCargas,
                    SexoCodigo = persona.PsexCodigo,
                    EstadoCivilCodigo = persona.PeescCodigo,
                    // Campos extra para lógica interna
                    Password = usuario.XeestCodigo // Usamos este campo temporalmente para pasar el ESTADO ('A'/'I')
                };
            }
        }

         public bool EditarUsuario(PeperPer persona, XeusuUsuar usuario, out string mensaje)
        {
            mensaje = string.Empty;
            using (var db = new MonsterContext())
            {
                using (var transaccion = db.Database.BeginTransaction())
                {
                    try
                    {
                        var pOriginal = db.PeperPers.FirstOrDefault(p => p.PeperCodigo == persona.PeperCodigo);
                        var uOriginal = db.XeusuUsuars.FirstOrDefault(u => u.XeusuLogin == usuario.XeusuLogin);

                        if (pOriginal == null || uOriginal == null)
                        {
                            mensaje = "El registro no existe.";
                            return false;
                        }

                        // --- ACTUALIZAR DATOS PERSONALES (Todos) ---
                        pOriginal.PeperNombre = persona.PeperNombre;
                        pOriginal.PeperApellido = persona.PeperApellido;
                        pOriginal.PeperDireccion = persona.PeperDireccion; // Dirección
                        pOriginal.PeperCelular = persona.PeperCelular;     // Celular
                        pOriginal.PeperTeldom = persona.PeperTeldom;       // Fijo
                        pOriginal.PeperFechanaci = persona.PeperFechanaci; // Fecha Nac.
                        pOriginal.PeperCargas = persona.PeperCargas;       // Cargas
                        pOriginal.PsexCodigo = persona.PsexCodigo;         // Sexo
                        pOriginal.PeescCodigo = persona.PeescCodigo;       // Estado Civil
                                                                           // El Email personal también se puede actualizar
                        pOriginal.PeperEmail = persona.PeperEmail;

                        // --- ACTUALIZAR USUARIO ---
                        // Nota: No permitimos cambiar el Login (Correo) fácilmente porque es PK en otras tablas
                        // Pero sí el estado
                        uOriginal.XeestCodigo = usuario.XeestCodigo;
                        uOriginal.XeusuFecmod = DateTime.Now;

                        db.SaveChanges();
                        transaccion.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        transaccion.Rollback();
                        mensaje = "Error al editar: " + ex.Message;
                        return false;
                    }
                }
            }
        }

        // 2. ELIMINACIÓN EN CASCADA (Borrado Físico Total)
        public bool EliminarUsuarioTotal(string cedula, out string mensaje)
        {
            mensaje = string.Empty;
            using (var db = new MonsterContext())
            {
                using (var transaccion = db.Database.BeginTransaction())
                {
                    try
                    {
                        // Buscamos las entidades relacionadas
                        var persona = db.PeperPers.FirstOrDefault(p => p.PeperCodigo == cedula);
                        // Buscamos el usuario por la FK de persona
                        var usuario = db.XeusuUsuars.FirstOrDefault(u => u.PeperCodigo == cedula);

                        if (persona == null)
                        {
                            mensaje = "Usuario no encontrado.";
                            return false;
                        }

                        // --- ORDEN DE BORRADO (De hijos a padres) ---

                        // A. Si tiene usuario, borrar sus PERFILES (XEUXP_USUPE)
                        if (usuario != null)
                        {
                            var perfiles = db.XeuxpUsupes.Where(x => x.XeusuLogin == usuario.XeusuLogin).ToList();
                            if (perfiles.Any()) db.XeuxpUsupes.RemoveRange(perfiles);
                        }

                        // B. Borrar registro de EMPLEADO (PEEMP_EMPLE) si existe
                        var empleado = db.PeempEmples.FirstOrDefault(e => e.PeperCodigo == cedula);
                        if (empleado != null) db.PeempEmples.Remove(empleado);

                        // C. Borrar registro de ESTUDIANTE (AEEST_ESTU) si existe
                        var estudiante = db.AeestEstus.FirstOrDefault(e => e.PeperCodigo == cedula);
                        if (estudiante != null)
                        {
                            // OJO: Si el estudiante ya tiene Matrículas (AAMAT), esto fallará por FK.
                            // Para testeo actual funciona, pero en prod deberíamos validar si tiene historial.
                            db.AeestEstus.Remove(estudiante);
                        }

                        // D. Borrar el USUARIO (XEUSU_USUAR)
                        if (usuario != null) db.XeusuUsuars.Remove(usuario);

                        // E. Finalmente, borrar la PERSONA (PEPER_PERS)
                        db.PeperPers.Remove(persona);

                        db.SaveChanges();
                        transaccion.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        transaccion.Rollback();
                        // Tip: Si falla por FK de Matrículas, el mensaje lo dirá
                        mensaje = "No se puede eliminar: " + ex.Message + (ex.InnerException != null ? " | " + ex.InnerException.Message : "");
                        return false;
                    }
                }
            }
        }
        // En CD_Usuario.cs

        public bool RegistrarUsuarioComplejo(PeperPer p, XeusuUsuar u, PeempEmple emp, AeestEstu est, out string mensaje)
        {
            mensaje = string.Empty;
            using (var db = new MonsterContext())
            {
                using (var transaccion = db.Database.BeginTransaction())
                {
                    try
                    {
                        // 1. Guardar Persona
                        if (db.PeperPers.Any(x => x.PeperCedula == p.PeperCedula))
                        {
                            mensaje = "La cédula ya existe.";
                            return false;
                        }
                        db.PeperPers.Add(p);
                        db.SaveChanges();

                        // 2. Guardar Usuario
                        u.PeperCodigo = p.PeperCodigo;
                        db.XeusuUsuars.Add(u);
                        db.SaveChanges();

                        // 3. Guardar Perfil de Sistema (XEUXP_USUPE) - ¡AQUÍ ESTABA EL ERROR!
                        var perfilSistema = new XeuxpUsupe
                        {
                            XeusuLogin = u.XeusuLogin,

                            // CORRECCIÓN: Usar el nombre de propiedad exacto que generó Entity Framework.
                            // En SQL es XEUXP_FECASI. Verifica en tu clase XeuxpUsupe.cs si es XeuxpFecasi o XeuxpFecasig
                            XeuxpFecasi = DateTime.Now,  // <--- ESTA FECHA FALTABA Y CAUSABA EL OVERFLOW

                            XeuxpFecret = null
                        };

                        // Asignar código de perfil según el caso
                        if (emp != null) perfilSistema.XeperCodigo = "INV";
                        if (est != null) perfilSistema.XeperCodigo = "EST";

                        db.XeuxpUsupes.Add(perfilSistema);
                        db.SaveChanges();

                        // 4. Guardar Específico
                        if (emp != null)
                        {
                            emp.PeperCodigo = p.PeperCodigo;
                            db.PeempEmples.Add(emp);
                        }
                        else if (est != null)
                        {
                            est.PeperCodigo = p.PeperCodigo;
                            db.AeestEstus.Add(est);
                        }

                        db.SaveChanges();
                        transaccion.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        transaccion.Rollback();
                        // Tip: Agrega ex.InnerException para ver detalles reales
                        mensaje = "Error: " + ex.Message + (ex.InnerException != null ? " | " + ex.InnerException.Message : "");
                        return false;
                    }
                }
            }
        }
    }
}