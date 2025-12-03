using Microsoft.EntityFrameworkCore; // Necesario para .ToList() async o normal
using Monster_University_GR2.CapaEntidad;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq;

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
    }
}