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
            // Instanciamos el contexto usando el método que creamos o inyección (por ahora instanciamos para simplificar como en tu ejemplo anterior)
            // NOTA: Lo ideal en .NET 8 es inyección, pero para mantener la estructura de tu lógica anterior:

            // Usaremos una instancia nueva del contexto generado
            using (var db = new MonsterContext())
            {
                // EJECUTAR SP EN .NET 8
                // Importante: Los nombres de columnas en el SP deben coincidir con la clase UsuarioSesion
                // OJO: Si tu SP devuelve columnas con nombres raros (ej: XEUSU_LOGIN),
                // asegúrate de usar alias en el SP o mapear bien. 

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
        // ... imports ...

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
        // ... imports ...

        // 1. OBTENER UN USUARIO ESPECÍFICO (Para Ver Detalle y para cargar el Editar)
        public UsuarioCrearViewModel ObtenerUsuarioCompleto(string cedula)
        {
            using (var db = new MonsterContext())
            {
                // Buscamos la persona y su usuario asociado
                var persona = db.PeperPers.FirstOrDefault(p => p.PeperCodigo == cedula);
                var usuario = db.XeusuUsuars.FirstOrDefault(u => u.PeperCodigo == cedula);

                if (persona == null || usuario == null) return null;

                // Mapeamos a un ViewModel lleno (Usamos el de Crear porque tiene todos los campos)
                return new UsuarioCrearViewModel
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

        // 2. GUARDAR CAMBIOS
        public bool EditarUsuario(PeperPer persona, XeusuUsuar usuario, out string mensaje)
        {
            mensaje = string.Empty;
            using (var db = new MonsterContext())
            {
                using (var transaccion = db.Database.BeginTransaction())
                {
                    try
                    {
                        // Buscar entidades originales
                        var pOriginal = db.PeperPers.Find(persona.PeperCodigo);
                        var uOriginal = db.XeusuUsuars.FirstOrDefault(u => u.PeperCodigo == persona.PeperCodigo);

                        if (pOriginal == null || uOriginal == null)
                        {
                            mensaje = "El usuario no existe.";
                            return false;
                        }

                        // Actualizar Persona
                        pOriginal.PeperNombre = persona.PeperNombre;
                        pOriginal.PeperApellido = persona.PeperApellido;
                        pOriginal.PeperDireccion = persona.PeperDireccion;
                        pOriginal.PeperCelular = persona.PeperCelular;
                        pOriginal.PeperTeldom = persona.PeperTeldom;
                        pOriginal.PeperFechanaci = persona.PeperFechanaci;
                        pOriginal.PeperCargas = persona.PeperCargas;
                        pOriginal.PsexCodigo = persona.PsexCodigo;
                        pOriginal.PeescCodigo = persona.PeescCodigo;
                        pOriginal.PeperEmail = persona.PeperEmail;

                        // Actualizar Usuario (Login y Estado)
                        uOriginal.XeusuLogin = usuario.XeusuLogin; // Permitimos cambiar correo/login
                        uOriginal.XeestCodigo = usuario.XeestCodigo; // Permitimos cambiar estado (A/I)
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
        public bool DarDeBajaUsuario(string cedula, out string mensaje)
        {
            mensaje = string.Empty;
            using (var db = new MonsterContext())
            {
                try
                {
                    // Buscamos al usuario por la cédula de la persona asociada
                    var usuario = db.XeusuUsuars.FirstOrDefault(u => u.PeperCodigo == cedula);

                    if (usuario == null)
                    {
                        mensaje = "Usuario no encontrado.";
                        return false;
                    }

                    usuario.XeestCodigo = "I"; // Inactivo
                    usuario.XeusuFecmod = DateTime.Now;

                    db.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    mensaje = "Error al dar de baja: " + ex.Message;
                    return false;
                }
            }
        }
    }
}