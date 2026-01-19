using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Monster_University_GR2.Models
{
    public class CatalogItem
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("codigo")]
        public string Codigo { get; set; }

        [BsonElement("descripcion")]
        public string Descripcion { get; set; }
    }
}