namespace Monster_University_GR2.CapaEntidad
{
    public class RolDTO // Usaremos este para el ComboBox
    {
        public string CodigoCompuesto { get; set; } // Formato: "DEP|ROL" (Ej: "SYS|ADM")
        public string Descripcion { get; set; }
    }

    public class PerfilUsuarioDTO // Este para las tablas
    {
        public string Login { get; set; }
        public string NombreCompleto { get; set; }
    }
}