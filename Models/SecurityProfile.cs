using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class SecurityProfile
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [BsonElement("nombre_perfil")]
    public string NombrePerfil { get; set; } // Ej: ADMINISTRADOR

    [BsonElement("accesos")]
    public List<ProfileAccess> Accesos { get; set; } = new List<ProfileAccess>();
}

public class ProfileAccess
{
    [BsonElement("sistema_id")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string SistemaId { get; set; }

    [BsonElement("opciones_permitidas")]
    public List<string> OpcionesPermitidas { get; set; } // Lista de códigos ["OP_MAT_01"]
}