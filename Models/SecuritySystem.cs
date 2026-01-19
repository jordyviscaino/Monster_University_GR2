using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class SecuritySystem
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [BsonElement("nombre_sistema")]
    public string NombreSistema { get; set; }

    [BsonElement("icono")]
    public string Icono { get; set; }

    [BsonElement("estado")]
    public string Estado { get; set; }

    [BsonElement("opciones")]
    public List<SecurityOption> Opciones { get; set; } = new List<SecurityOption>();
}

public class SecurityOption
{
    [BsonElement("codigo_opcion")]
    public string CodigoOpcion { get; set; } // ID Lógico (ej: OP_MAT_01)

    [BsonElement("titulo")]
    public string Titulo { get; set; }

    [BsonElement("url_controlador")]
    public string UrlControlador { get; set; }

    [BsonElement("url_accion")]
    public string UrlAccion { get; set; }

    [BsonElement("icono")]
    public string Icono { get; set; }
}