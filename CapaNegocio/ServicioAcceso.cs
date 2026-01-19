using Monster_University_GR2.CapaDatos;
using Monster_University_GR2.CapaEntidad;
using Monster_University_GR2.Colecciones;
using Monster_University_GR2.CapaNegocio.Recursos;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace Monster_University_GR2.CapaNegocio
{
    public class ServicioAcceso
    {
        private readonly UsuariosCollection _usuariosCollection;

        public ServicioAcceso(UsuariosCollection usuariosCollection)
        {
            _usuariosCollection = usuariosCollection;
        }

        public async Task<Usuario> ValidarLogin(string email, string passwordPlano)
        {
            var usuario = await _usuariosCollection.ObtenerPorEmail(email);

            if (usuario == null) return null;

            string hashEntrante = Utilidades.EncriptarClave(passwordPlano);

            if (usuario.PasswordHash == hashEntrante)
            {
                if (usuario.Estado != "ACTIVO") return null;
                return usuario;
            }

            return null;
        }

       public async Task<string> RegistrarUsuarioInvitado(string cedula, string nombres, string apellidos, string email, string celular, string passwordPlano)
        {
            // A. Validar duplicados
            var usuarioExistente = await _usuariosCollection.ObtenerPorEmail(email);
            if (usuarioExistente != null)
            {
                return "El correo ya está registrado en el sistema.";
            }

            
            string hashPassword = Utilidades.EncriptarClave(passwordPlano);

            Usuario nuevoUsuario = new Usuario
            {
                Cedula = cedula,
                Nombres = nombres.ToUpper(),
                Apellidos = apellidos.ToUpper(),
                Email = email,
                PasswordHash = hashPassword,
                Estado = "ACTIVO",
                DebeCambiarPwd = "N",
                Roles = new List<string> { "EST" }, // Rol Estudiante

                DatosPersonales = new DatosPersonales
                {
                    Telefono = celular,
                    Direccion = "S/D", // Sin Dirección
                    SexoCodigo = "M",
                    EstadoCivilCodigo = "SOLTERO",

                    FechaNacimiento = DateTime.Now
                },
                InfoEmpleado = new InfoEmpleado
                {
                    Departamento = "ACA", 
                    Cargo = "Estudiante"  
                }
            };

          
            try
            {
                await _usuariosCollection.InsertarUsuario(nuevoUsuario);
                return "OK";
            }
            catch (Exception ex)
            {
                
                return "Error de validación de base de datos: " + ex.Message;
            }
        }

       public async Task<bool> CambiarContrasena(string email, string nuevaClavePlana)
        {
            var usuario = await _usuariosCollection.ObtenerPorEmail(email);
            if (usuario == null) return false;

            string nuevoHash = Utilidades.EncriptarClave(nuevaClavePlana);

          
            await _usuariosCollection.ActualizarClave(usuario.Id, nuevoHash, "N");

            return true;
        }

        public async Task<string> RecuperarContrasena(string email)
        {
            var usuario = await _usuariosCollection.ObtenerPorEmail(email);
            if (usuario == null) return null;

            
            string claveTemporal = Guid.NewGuid().ToString().Substring(0, 8);
            string hashTemporal = Utilidades.EncriptarClave(claveTemporal);

           
            await _usuariosCollection.ActualizarClave(usuario.Id, hashTemporal, "S");

            return claveTemporal; // Retornamos la clave plana para enviarla por correo
        }
    }
}