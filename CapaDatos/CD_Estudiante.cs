using Microsoft.EntityFrameworkCore;
using Monster_University_GR2.CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Monster_University_GR2.CapaDatos
{
    public class CD_Estudiante
    {
        // 1. LISTAR TODOS LOS ESTUDIANTES (Para el Index)
        public List<EstudianteResumenDTO> ListarEstudiantes()
        {
            using (var db = new MonsterContext())
            {
                var query = from est in db.AeestEstus
                            join per in db.PeperPers on est.PeperCodigo equals per.PeperCodigo
                            join usu in db.XeusuUsuars on per.PeperCodigo equals usu.PeperCodigo
                            where usu.XeestCodigo == "A"
                            select new EstudianteResumenDTO
                            {
                                CodigoEstudiante = est.AeestCodigo,
                                NombreCompleto = per.PeperNombre + " " + per.PeperApellido,
                                Email = usu.XeusuLogin,
                                // Si tuvieras tabla de carreras, aquí harías el join. 
                                // Por ahora lo dejamos genérico o vacío.
                                Carrera = "Estudiante Regular"
                            };

                return query.ToList();
            }
        }

        // 2. OBTENER PERFIL PARA BOLETÍN (Sin notas falsas)
        public BoletinEstudianteViewModel ObtenerBoletin(string codigoEstudiante)
        {
            using (var db = new MonsterContext())
            {
                // A. Buscar Datos del Estudiante
                var datosEst = (from est in db.AeestEstus
                                join per in db.PeperPers on est.PeperCodigo equals per.PeperCodigo
                                join usu in db.XeusuUsuars on per.PeperCodigo equals usu.PeperCodigo
                                where est.AeestCodigo == codigoEstudiante
                                select new EstudianteResumenDTO
                                {
                                    CodigoEstudiante = est.AeestCodigo,
                                    NombreCompleto = per.PeperNombre + " " + per.PeperApellido,
                                    Email = usu.XeusuLogin,
                                    Carrera = "Estudiante Regular"
                                }).FirstOrDefault();

                if (datosEst == null) return null;

                // B. RETORNAR EL TEMPLATE VACÍO
                // No consultamos materias ni inventamos notas.
                // Devolvemos la lista de notas vacía (Count = 0).
                return new BoletinEstudianteViewModel
                {
                    Estudiante = datosEst,
                    Notas = new List<BoletinNotaDTO>(), // Lista vacía lista para el futuro
                    PromedioGeneral = 0.00
                };
            }
        }
    }
}