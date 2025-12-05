namespace Monster_University_GR2.CapaEntidad
{
    public class UsuarioSesion
    {
        public string Login { get; set; }
        public string PasswordHash { get; set; } // XEUSU_PASWD
        public string Estado { get; set; }       // XEEST_CODIGO
        public string Nombre { get; set; }       // PEPER_NOMBRE
        public string Apellido { get; set; }     // PEPER_APELLIDO
        public string Email { get; set; }        // PEPER_EMAIL
        public string? RolCodigo { get; set; }   // PEROL_CODIGO (Puede ser null)
        public string? DepartamentoCodigo { get; set; }
        public string? CodigoEstudiante { get; set; }
        public string DebeCambiarPassword { get; set; }
    }
}
