using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace Monster_University_GR2.CapaEntidad
{
    // Mapeo de security_systems.json
    public class Sistema
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("nombre_sistema")]
        public string NombreSistema { get; set; }

        [BsonElement("codigo_sistema")]
        public string CodigoSistema { get; set; } // P, A, R

        [BsonElement("icono")]
        public string Icono { get; set; }

        [BsonElement("opciones")]
        public List<OpcionSistema> Opciones { get; set; }
    }

    public class OpcionSistema
    {
        [BsonElement("codigo_opcion")]
        public string CodigoOpcion { get; set; }

        [BsonElement("titulo")]
        public string Titulo { get; set; }

        [BsonElement("url_controlador")]
        public string Controlador { get; set; }

        [BsonElement("url_accion")]
        public string Accion { get; set; }
    }

    // Mapeo de security_profiles.json
    public class Perfil
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("codigo_perfil")]
        public string CodigoPerfil { get; set; } // ADM, DOC

        [BsonElement("accesos")]
        public List<AccesoPerfil> Accesos { get; set; }
    }

    public class AccesoPerfil
    {
        [BsonElement("sistema_ref")]
        public string SistemaRef { get; set; }

        [BsonElement("opciones_permitidas")]
        public List<string> OpcionesPermitidas { get; set; }
    }
}