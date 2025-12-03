using Monster_University_GR2.CapaDatos;
using Monster_University_GR2.CapaEntidad;
using System.Collections.Generic;
using System.Linq;

namespace MonsterUniversity_Web.CapaDatos
{
    public class CD_Recursos
    {
        public List<PsexSexo> ObtenerSexos()
        {
            using (var db = new MonsterContext())
            {
                return db.PsexSexos.ToList();
            }
        }

        public List<PeescEstciv> ObtenerEstadosCiviles()
        {
            using (var db = new MonsterContext())
            {
                return db.PeescEstcivs.ToList();
            }
        }
    }
}