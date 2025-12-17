using Monster_University_GR2.CapaDatos;
using Monster_University_GR2.CapaEntidad;
using System.Collections.Generic;

// ... imports
namespace Monster_University_GR2.CapaNegocio
{
    public class CN_Seguridad
    {
        private CD_Seguridad objDatos = new CD_Seguridad();

        public List<RolDTO> ListarRoles() => objDatos.ListarRoles();

        public List<PerfilUsuarioDTO> ObtenerAsignados(string codigoCompuesto)
        {
            var partes = codigoCompuesto.Split('|'); // Separamos SYS y ADM
            return objDatos.ObtenerAsignados(partes[0], partes[1]);
        }

        public List<PerfilUsuarioDTO> ObtenerNoAsignados(string codigoCompuesto)
        {
            var partes = codigoCompuesto.Split('|');
            return objDatos.ObtenerNoAsignados(partes[0], partes[1]);
        }

        public bool Asignar(string login, string codigoCompuesto)
        {
            var partes = codigoCompuesto.Split('|');
            // Partes[0] = Departamento (ej: ACA), Partes[1] = Rol (ej: DOC)
            return objDatos.AsignarRol(login, partes[0], partes[1]);
        }

        public bool Desasignar(string login, string codigoCompuesto)
        {
            // Al desasignar, ignoramos el código compuesto y lo mandamos a INV
            return objDatos.DesasignarRol(login);
        }
    }
}