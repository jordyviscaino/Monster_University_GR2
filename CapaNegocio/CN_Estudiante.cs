using Monster_University_GR2.CapaDatos;
using Monster_University_GR2.CapaEntidad;
using System.Collections.Generic;

namespace Monster_University_GR2.CapaNegocio
{
    public class CN_Estudiante
    {
        private CD_Estudiante objDatos = new CD_Estudiante();

        public List<EstudianteResumenDTO> Listar()
        {
            return objDatos.ListarEstudiantes();
        }

        public BoletinEstudianteViewModel ObtenerBoletin(string id)
        {
            return objDatos.ObtenerBoletin(id);
        }
    }
}