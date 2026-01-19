/*using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Monster_University_GR2.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("cedula")]
        public string Cedula { get; set; }

        [BsonElement("nombres")]
        public string Nombres { get; set; }

        [BsonElement("apellidos")]
        public string Apellidos { get; set; }

        [BsonElement("email")]
        public string Email { get; set; }

        [BsonElement("password_hash")]
        public string PasswordHash { get; set; }

        [BsonElement("estado")]
        public string Estado { get; set; } // "ACTIVO" o "INACTIVO"

        [BsonElement("roles")]
        public List<string> Roles { get; set; } = new List<string>();

        // Usamos la clase externa DatosPersonales (Models/DatosPersonales.cs)
        [BsonElement("datos_personales")]
        public DatosPersonales DatosPersonales { get; set; }

        [BsonElement("info_empleado")]
        public InfoEmpleado InfoEmpleado { get; set; }

        // Propiedad extra para saber cuándo se creó (si no existe en DB, usa fecha actual)
        [BsonIgnore]
        public DateTime FechaRegistro => Id != null a? new ObjectId(Id).CreationTime : DateTime.Now;
    }
}*/