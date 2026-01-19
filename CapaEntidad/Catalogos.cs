using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Monster_University_GR2.CapaEntidad
{
    // Mapeo para la colección 'sexos'
    [BsonIgnoreExtraElements]
    public class SexoCatalogo
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("codigo")]
        public string Codigo { get; set; } // Ej: "M", "F"

        [BsonElement("descripcion")]
        public string Descripcion { get; set; } // Ej: "Masculino"
    }

    // Mapeo para la colección 'estados_civiles'
    [BsonIgnoreExtraElements]
    public class EstadoCivilCatalogo
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("codigo")]
        public string Codigo { get; set; } // Ej: "S", "C"

        [BsonElement("descripcion")]
        public string Descripcion { get; set; } // Ej: "Soltero"
    }
}