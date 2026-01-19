using System.Collections.Generic;

namespace Monster_University_GR2.CapaEntidad
{
    public class SistemaMenuDTO
    {
        public string Codigo { get; set; } 
        public string Nombre { get; set; }
        public List<OpcionMenuDTO> Opciones { get; set; }

        public SistemaMenuDTO() { Opciones = new List<OpcionMenuDTO>(); }
    }

    public class OpcionMenuDTO
    {
        public string Codigo { get; set; } 
        public string Nombre { get; set; }
        public bool Activo { get; set; }  
    }
}