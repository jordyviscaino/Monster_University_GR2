using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Monster_University_GR2.CapaEntidad
{
    [BsonIgnoreExtraElements]
    public class Usuario
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
        public string Estado { get; set; }

        [BsonElement("debe_cambiar_pwd")]
        public string DebeCambiarPwd { get; set; }

        [BsonElement("roles")]
        public List<string> Roles { get; set; }

        [BsonElement("datos_personales")]
        public DatosPersonales DatosPersonales { get; set; }

        [BsonElement("info_empleado")]
        public InfoEmpleado InfoEmpleado { get; set; }

        // ==========================================================
        // AGREGAMOS ESTO PARA QUE 'ServicioUsuarios' NO DE ERROR
        // ==========================================================
        // Esto NO crea otra tabla. Se guarda ADENTRO del documento del usuario.
        [BsonElement("datos_academicos")]
        public DatosAcademicos DatosAcademicos { get; set; }
    }

    // CLASES DE APOYO (Objetos incrustados)

    [BsonIgnoreExtraElements]
    public class DatosPersonales
    {
        [BsonElement("sexo")]
        public string SexoCodigo { get; set; }

        [BsonElement("estado_civil")]
        public string EstadoCivilCodigo { get; set; }

        [BsonElement("fecha_nacimiento")]
        public DateTime FechaNacimiento { get; set; }

        [BsonElement("direccion")]
        public string Direccion { get; set; }

        // CORRECCIÓN: Unificamos a un solo campo como está en tu JSON
        [BsonElement("telefono")]
        public string Telefono { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class InfoEmpleado
    {
        [BsonElement("departamento")]
        public string Departamento { get; set; }

        [BsonElement("cargo")]
        public string Cargo { get; set; }
    }

    // DEFINE LA CLASE, AUNQUE NO TENGA TABLA PROPIA
    [BsonIgnoreExtraElements]
    public class DatosAcademicos
    {
        [BsonElement("carrera_id")]
        public string CarreraId { get; set; } // Aquí guardaremos "SOFTWARE", "TI", etc.

        [BsonElement("semestre")]
        public int Semestre { get; set; }
    }
}