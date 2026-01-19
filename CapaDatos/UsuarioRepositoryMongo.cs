/*using System;
using System.Collections.Generic;
using MongoDB.Driver;
using Monster_University_GR2.Models;

namespace Monster_University_GR2.CapaDatos
{
    public class UsuarioRepositoryMongo : IUsuarioRepository
    {
        private readonly ContextoMongo _context;

        public UsuarioRepositoryMongo(ContextoMongo context)
        {
            _context = context;
        }

        public List<User> GetAll() => _context.Users.Find(_ => true).ToList();

        public User GetByCedula(string cedula) => _context.Users.Find(u => u.Cedula == cedula).FirstOrDefault();

        // CAMBIO: Ahora devuelve string con el error (vacío si fue éxito)
        public string Create(User user)
        {
            try
            {
                _context.Users.InsertOne(user);
                return ""; // Éxito
            }
            catch (Exception ex)
            {
                return ex.Message; // Devuelve el error real de Mongo
            }
        }

        public string Update(string cedula, User user)
        {
            try
            {
                var filter = Builders<User>.Filter.Eq(u => u.Cedula, cedula);

                // Reemplazamos el documento completo (excepto _id) para asegurar que todo se guarde
                // Esto soluciona problemas de campos parciales
                user.Id = GetByCedula(cedula).Id; // Mantenemos el ID original de Mongo

                var result = _context.Users.ReplaceOne(filter, user);

                if (result.IsAcknowledged && result.MatchedCount > 0) return "";
                return "No se encontró el usuario o no hubo cambios.";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string Delete(string cedula)
        {
            try
            {
                var result = _context.Users.DeleteOne(u => u.Cedula == cedula);
                if (result.DeletedCount > 0) return "";
                return "No se encontró el usuario para eliminar.";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}*/