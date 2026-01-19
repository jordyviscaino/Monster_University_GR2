using System;

namespace Monster_University_GR2.Models 
{
    public class UsuarioResumenViewModel
    {
        public string Cedula { get; set; }
        public string NombresCompletos { get; set; }
        public string Email { get; set; }
        public string RolPrincipal { get; set; } 
        public string Estado { get; set; }
        public string FechaRegistro { get; set; }
    }
}