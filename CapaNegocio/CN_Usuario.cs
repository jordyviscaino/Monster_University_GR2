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
            string passHash = GenerarSHA256(passwordPlano);
            UsuarioSesion usuario = objCapaDato.ValidarAcceso(login);

            if (usuario != null)
            {
                if (usuario.PasswordHash == passHash)
                {
                    // Si no tiene rol asignado, es INVITADO por defecto visualmente
                    if (string.IsNullOrEmpty(usuario.RolCodigo))
                    {
                        usuario.RolCodigo = "INV";
                    }
                    return usuario;
                }
            }
            return null;
        }

        public bool RecuperarContrasena(string correo, out string mensaje)
        {
            string claveTemporal = GenerarClaveAleatoria();
            string claveHash = GenerarSHA256(claveTemporal);
            bool resultado = objCapaDato.RestablecerClave(correo, claveHash, out mensaje);

            if (resultado)
            {
                string asunto = "Restablecer Contraseña - Monster University";
                string cuerpo = $@"<h2 style='color:#004aad'>Tu clave temporal: {claveTemporal}</h2>
                                   <p>Cámbiala al iniciar sesión.</p>";

                string errorCorreo = "";
                // Asegúrate que tu CN_Correo soporte el parámetro out errorCorreo como lo dejamos antes
                bool correoEnviado = CN_Correo.Enviar(correo, asunto, cuerpo, out errorCorreo);

                if (!correoEnviado)
                {
                    mensaje = "Clave cambiada, pero error SMTP: " + errorCorreo;
                    return false;
                }
                return true;
            }
            return false;
        }

        public bool CambiarClaveObligatoria(string correo, string nuevaClave, out string mensaje)
        {
            string hash = GenerarSHA256(nuevaClave);
            return objCapaDato.ActualizarPassword(correo, hash, out mensaje);
        }

        public bool Registrar(RegistroViewModel modelo, out string mensaje)
        {
            // A. Mapear Persona
            PeperPer persona = new PeperPer
            {
                PeperCodigo = modelo.Cedula,
                PeperCedula = modelo.Cedula,
                PeperNombre = modelo.Nombre.ToUpper(),
                PeperApellido = modelo.Apellido.ToUpper(),
                PeperEmail = modelo.Email,
                PeperFechanaci = DateTime.Now.AddYears(-18), // Default
                PeperDireccion = "S/D",
                PeperCelular = "0000000000",
                PeperTeldom = "0000000",
                PeperCargas = 0,
                PsexCodigo = modelo.SexoCodigo,
                PeescCodigo = modelo.EstadoCivilCodigo
            };

            // B. Mapear Usuario
            XeusuUsuar usuario = new XeusuUsuar
            {
                XeusuLogin = modelo.Email,
                XeusuPaswd = GenerarSHA256(modelo.Password),
                XeestCodigo = "A",
                XeusuFeccre = DateTime.Now,
                XeusuFecmod = DateTime.Now,
                XeusuPiefir = "WEB_PUBLIC",
                XeusuCambiarPwd = "N" // Él puso su clave, no necesita cambiarla
            };

            // C. CREAR EMPLEADO GENÉRICO (Para que aparezca como Invitado)
            // Esto permite que el Admin lo vea en la "Tabla Izquierda" de Asignación de Roles
            PeempEmple empleadoInv = new PeempEmple
            {
                PeempCodigo = modelo.Cedula,
                PeperCodigo = modelo.Cedula,
                PedepCodigo = "GEN", // Dept General
                PerolCodigo = "INV", // Rol Invitado
      
            };

            // Usamos el método potente de la capa de datos
            // Pasamos NULL en estudiante porque es un registro público genérico
            return objCapaDato.RegistrarUsuarioComplejo(persona, usuario, empleadoInv, null, out mensaje);
        }

        public bool RegistrarUsuarioDinámico(UsuarioCrearCompletoViewModel modelo, out string mensaje)
        {
            if (modelo.FechaNacimiento < new DateTime(1753, 1, 1))
            {
                modelo.FechaNacimiento = new DateTime(2000, 1, 1);
            }

            // 2. Mapeo Persona (PEPER_PERS)
            PeperPer persona = new PeperPer
            {
                PeperCodigo = modelo.Cedula,
                PeperCedula = modelo.Cedula,
                PeperNombre = modelo.Nombre.ToUpper(),
                PeperApellido = modelo.Apellido.ToUpper(),
                PeperEmail = modelo.Email,
                PeperFechanaci = modelo.FechaNacimiento,
                PeperDireccion = modelo.Direccion.ToUpper(),
                PeperCelular = modelo.Celular,
                PeperTeldom = modelo.TelefonoDomicilio ?? "0000000",
                PeperCargas = modelo.CargasFamiliares,
                PsexCodigo = modelo.SexoCodigo,
                PeescCodigo = modelo.EstadoCivilCodigo
            };

            // 3. Mapeo Usuario (XEUSU_USUAR)
            XeusuUsuar usuario = new XeusuUsuar
            {
                XeusuLogin = modelo.Email,
                XeusuPaswd = GenerarSHA256(modelo.Password),
                XeestCodigo = "A",
                XeusuFeccre = DateTime.Now,
                XeusuFecmod = DateTime.Now,
                XeusuPiefir = "ADMIN_CRUD",
                XeusuCambiarPwd = "S"
            };

            // 4. Lógica Dinámica (SOLO CAMPOS REALES)
            PeempEmple empleado = null;
            AeestEstu estudiante = null;

            if (modelo.TipoVinculacion == "EMP")
            {
                empleado = new PeempEmple
                {
                    PeempCodigo = modelo.Cedula,
                    PeperCodigo = modelo.Cedula,
                    PedepCodigo = "GEN", // Dept. General
                    PerolCodigo = "INV"  // Rol Invitado
                                         // ELIMINADO: PeempFeccon y PeempSueldo (No existen en tu BD)
                };
            }
            else if (modelo.TipoVinculacion == "EST")
            {
                estudiante = new AeestEstu
                {
                    AeestCodigo = modelo.Cedula,
                    PeperCodigo = modelo.Cedula
                    // ELIMINADO: AeestFechIngreso (No existe en tu BD)
                };
            }

            return objCapaDato.RegistrarUsuarioComplejo(persona, usuario, empleado, estudiante, out mensaje);
        }
       public List<UsuarioResumenViewModel> ObtenerListaUsuarios()
        {
            return objCapaDato.ListarUsuarios();
        }

        public UsuarioEditarViewModel ObtenerParaEditar(string cedula)
        {
            var datos = objCapaDato.ObtenerUsuarioCompleto(cedula);
            if (datos == null) return null;

            return new UsuarioEditarViewModel
            {
                Cedula = datos.Cedula,
                Nombre = datos.Nombre,
                Apellido = datos.Apellido,
                Email = datos.Email,
                FechaNacimiento = datos.FechaNacimiento,
                Direccion = datos.Direccion,
                Celular = datos.Celular,
                TelefonoDomicilio = datos.TelefonoDomicilio,
                CargasFamiliares = datos.CargasFamiliares,
                SexoCodigo = datos.SexoCodigo,
                EstadoCivilCodigo = datos.EstadoCivilCodigo,
                EstadoUsuario = datos.Password
            };
        }

        public bool Editar(UsuarioEditarViewModel modelo, out string mensaje)
        {
            PeperPer persona = new PeperPer
            {
                PeperCodigo = modelo.Cedula,
                PeperNombre = modelo.Nombre.ToUpper(),
                PeperApellido = modelo.Apellido.ToUpper(),
                PeperEmail = modelo.Email,
                PeperFechanaci = modelo.FechaNacimiento,
                PeperDireccion = modelo.Direccion.ToUpper(),
                PeperCelular = modelo.Celular,
                PeperTeldom = modelo.TelefonoDomicilio,
                PeperCargas = modelo.CargasFamiliares,
                PsexCodigo = modelo.SexoCodigo,
                PeescCodigo = modelo.EstadoCivilCodigo
            };

            XeusuUsuar usuario = new XeusuUsuar
            {
                XeusuLogin = modelo.Email,
                XeestCodigo = modelo.EstadoUsuario
            };

            return objCapaDato.EditarUsuario(persona, usuario, out mensaje);
        }

        public bool Eliminar(string cedula, out string mensaje)
        {

            return objCapaDato.EliminarUsuarioTotal(cedula, out mensaje);
        }

   public List<PsexSexo> ListarSexos() { return objRecursos.ObtenerSexos(); }
        public List<PeescEstciv> ListarEstados() { return objRecursos.ObtenerEstadosCiviles(); }

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

        private string GenerarClaveAleatoria()
        {
            string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            Random random = new Random();
            char[] result = new char[6];
            for (int i = 0; i < result.Length; i++) result[i] = chars[random.Next(chars.Length)];
            return new string(result);
        }
    }
}