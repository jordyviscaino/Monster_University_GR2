using Microsoft.Extensions.Configuration; 
using MongoDB.Bson;
using MongoDB.Driver;
using Monster_University_GR2.CapaEntidad;

namespace Monster_University_GR2.CapaDatos
{
    public class ContextoMongo
    {
        private readonly IMongoDatabase _database;

        public ContextoMongo(IConfiguration configuration)
        {
            var sección = configuration.GetSection("MongoSettings");
            var cadenaConexion = sección["ConnectionString"];
            var nombreDb = sección["DatabaseName"];

            if (string.IsNullOrWhiteSpace(cadenaConexion))
                throw new InvalidOperationException("Falta MongoSettings:ConnectionString en la configuración.");

            var mongoUrl = MongoUrl.Create(cadenaConexion);
            var mongoClient = new MongoClient(mongoUrl);

            _database = mongoClient.GetDatabase(!string.IsNullOrWhiteSpace(nombreDb) ? nombreDb : mongoUrl.DatabaseName);
        }

  
        public IMongoCollection<T> ObtenerColeccion<T>(string nombreColeccion)
        {
            return _database.GetCollection<T>(nombreColeccion);
        }

        public IMongoCollection<Usuario> Usuarios => _database.GetCollection<Usuario>("users");

        public IMongoCollection<BsonDocument> ObtenerColeccionGenerica(string nombreColeccion)
        {
            return _database.GetCollection<BsonDocument>(nombreColeccion);
        }
    }
}