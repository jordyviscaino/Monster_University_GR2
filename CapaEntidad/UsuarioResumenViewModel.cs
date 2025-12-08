namespace Monster_University_GR2.CapaEntidad
{
    public class UsuarioResumenViewModel
    {
        public string Cedula { get; set; }
        public string NombreCompleto { get; set; }
        public string Email { get; set; }
        public string Estado { get; set; } // 'A' o 'I'
        public string Rol { get; set; } // Por ahora será 'Sin Perfil' o el dato que tengamos
        public DateTime FechaRegistro { get; set; }
    }
}