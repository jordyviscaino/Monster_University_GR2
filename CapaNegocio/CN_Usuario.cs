using Monster_University_GR2.CapaDatos;
using Monster_University_GR2.CapaEntidad;
using MonsterUniversity_Web.CapaDatos;
using System.Security.Cryptography;
using System.Text;

namespace Monster_University_GR2.CapaNegocio
{
    public class CN_Usuario
    {
        private CD_Usuario objCapaDato = new CD_Usuario();
        private CD_Recursos objRecursos = new CD_Recursos();
        public UsuarioSesion ValidarUsuario(string login, string passwordPlano)
        {
            // 1. Generar Hash (Reciclado de tu proyecto anterior)
            string passHash = GenerarSHA256(passwordPlano);

            // 2. Llamar a la Capa de Datos
            UsuarioSesion usuario = objCapaDato.ValidarAcceso(login);

            // 3. Validar lógica
            if (usuario != null)
            {
                if (usuario.PasswordHash == passHash)
                {
                    // Lógica del Rol Invitado (Reciclada)
                    // Si RolCodigo es null, asignamos "INV"
                    if (string.IsNullOrEmpty(usuario.RolCodigo))
                    {
                        usuario.RolCodigo = "INV";
                    }

                    return usuario;
                }
            }

            return null; // Login fallido
        }

        // Métodos para llenar combos
        public List<PsexSexo> ListarSexos() { return objRecursos.ObtenerSexos(); }
        public List<PeescEstciv> ListarEstados() { return objRecursos.ObtenerEstadosCiviles(); }

        public bool Registrar(RegistroViewModel modelo, out string mensaje)
        {
            // 1. Mapear ViewModel -> Entidad Persona
            PeperPer persona = new PeperPer
            {
                PeperCodigo = modelo.Cedula, // Usamos cédula como PK
                PeperCedula = modelo.Cedula,
                PeperNombre = modelo.Nombre.ToUpper(),
                PeperApellido = modelo.Apellido.ToUpper(),
                PeperEmail = modelo.Email,
                PsexCodigo = modelo.SexoCodigo,
                PeescCodigo = modelo.EstadoCivilCodigo,
                // Defaults obligatorios
                PeperFechanaci = DateTime.Now.AddYears(-18),
                PeperCargas = 0,
                PeperDireccion = "S/D",
                PeperCelular = "0000000000",
                PeperTeldom = "0000000"
            };

            // 2. Mapear ViewModel -> Entidad Usuario
            XeusuUsuar usuario = new XeusuUsuar
            {
                XeusuLogin = modelo.Email, // Login es el correo
                XeusuPaswd = GenerarSHA256(modelo.Password), // Encriptar
                XeestCodigo = "A", // Activo
                XeusuFeccre = DateTime.Now,
                XeusuFecmod = DateTime.Now,
                XeusuPiefir = "WEB_NET8",
                XeusuCambiarPwd = "N"
            };

            // 3. Llamar a Capa Datos
            return objCapaDato.RegistrarUsuario(persona, usuario, out mensaje);
        }
        // Método Hash (Copiado exactamente de tu código anterior)
        public static string GenerarSHA256(string str)
        {
            if (string.IsNullOrEmpty(str)) return "";
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(str);
                var hash = sha256.ComputeHash(bytes);
                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
        }
        // ... dentro de la clase CN_Usuario ...

        public bool RecuperarContrasena(string correo, out string mensaje)
        {
            // 1. Generar Clave Temporal (Ej: A7x#9m)
            string claveTemporal = GenerarClaveAleatoria();

            // 2. Encriptar (SHA256) para la Base de Datos
            string claveHash = GenerarSHA256(claveTemporal);

            // 3. Guardar en Base de Datos
            bool resultado = objCapaDato.RestablecerClave(correo, claveHash, out mensaje);

            if (resultado)
            {
                // 4. Enviar Correo con la Clave TEMPORAL (Plana)
                string asunto = "Restablecer Contraseña - Monster University";
                string cuerpo = $@"
                    <div style='font-family: Arial, sans-serif; padding: 20px; border: 1px solid #ddd;'>
                        <h2 style='color: #004aad;'>Recuperación de Acceso</h2>
                        <p>Hola,</p>
                        <p>Se ha solicitado restablecer tu contraseña para el Sistema Académico.</p>
                        <p>Tu contraseña temporal es: <strong style='font-size: 1.2em; background-color: #f0f0f0; padding: 5px;'>{claveTemporal}</strong></p>
                        <p style='color: red;'>Por seguridad, deberás cambiarla inmediatamente al iniciar sesión.</p>
                        <hr />
                        <small>Si no solicitaste esto, comunícate con soporte.</small>
                    </div>";

                string errorCorreo = "";
                bool correoEnviado = CN_Correo.Enviar(correo, asunto, cuerpo, out errorCorreo);

                if (!correoEnviado)
                {
                    mensaje = "La clave se cambió, pero falló el envío del correo: " + errorCorreo;
                    return false;
                }
                return true;
            }
            return false;
        }

        // Auxiliar para generar clave aleatoria de 6 dígitos
        private string GenerarClaveAleatoria()
        {
            string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            Random random = new Random();
            char[] result = new char[6];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = chars[random.Next(chars.Length)];
            }
            return new string(result);
        }

    }
}