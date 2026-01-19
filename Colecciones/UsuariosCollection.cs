using Monster_University_GR2.CapaDatos;
using Monster_University_GR2.CapaEntidad;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Monster_University_GR2.Colecciones
{
    public class UsuariosCollection
    {
        private readonly IMongoCollection<Usuario> _usuarios;

        public UsuariosCollection(ContextoMongo contexto)
        {
                    _usuarios = contexto.ObtenerColeccion<Usuario>("users");
        }

        // 1. OBTENER TODOS (Para el Index)
        public async Task<List<Usuario>> ObtenerTodos()
        {
            return await _usuarios.Find(_ => true).ToListAsync();
        }

        public async Task<Usuario> ObtenerPorEmail(string email)
        {
            return await _usuarios.Find(u => u.Email == email).FirstOrDefaultAsync();
        }

        public async Task<Usuario> ObtenerPorCedula(string cedula)
        {
            return await _usuarios.Find(u => u.Cedula == cedula).FirstOrDefaultAsync();
        }

        public async Task InsertarUsuario(Usuario usuario)
        {
            await _usuarios.InsertOneAsync(usuario);
        }

        public async Task ActualizarClave(string idUsuario, string nuevoHash, string debeCambiar)
        {
            var filtro = Builders<Usuario>.Filter.Eq(u => u.Id, idUsuario);
            var update = Builders<Usuario>.Update
                .Set(u => u.PasswordHash, nuevoHash)
                .Set(u => u.DebeCambiarPwd, debeCambiar);

            await _usuarios.UpdateOneAsync(filtro, update);
        }

        public async Task ActualizarDatosGenerales(Usuario u)
        {
            var filtro = Builders<Usuario>.Filter.Eq(x => x.Cedula, u.Cedula);

            var update = Builders<Usuario>.Update
                .Set(x => x.DatosPersonales.Direccion, u.DatosPersonales.Direccion)
                .Set(x => x.DatosPersonales.Telefono, u.DatosPersonales.Telefono)

                .Set(x => x.DatosPersonales.SexoCodigo, u.DatosPersonales.SexoCodigo)
                .Set(x => x.DatosPersonales.EstadoCivilCodigo, u.DatosPersonales.EstadoCivilCodigo)
                .Set(x => x.DatosPersonales.FechaNacimiento, u.DatosPersonales.FechaNacimiento)

                .Set(x => x.Estado, u.Estado);

            await _usuarios.UpdateOneAsync(filtro, update);
        }

        public async Task<bool> EliminarPorCedula(string cedula)
        {
            var resultado = await _usuarios.DeleteOneAsync(u => u.Cedula == cedula);
            return resultado.DeletedCount > 0;
        }
    }
}