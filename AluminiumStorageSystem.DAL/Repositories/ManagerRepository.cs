using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AluminiumStorageSystem.CORE;
using AluminiumStorageSystem.CORE.Dtos;
using AluminiumStorageSystem.DAL.Queries;
using Npgsql;

namespace AluminiumStorageSystem.DAL.Repositories
{
    public class ManagerRepository
    {
        public List<ManagerDto> GetAllManagers()
        {
            using (var connection = new NpgsqlConnection(Options.ConnectionString))
            {
                connection.Open();

                var command = new NpgsqlCommand(ManagerQueries.GetAllManagers, connection);

                using (var reader = command.ExecuteReader())
                {
                    var managers = new List<ManagerDto>();
                    while (reader.Read())
                    {
                        managers.Add(new ManagerDto
                        {
                            Id = reader.GetInt32("id"),
                            Name = reader.GetString("name"),
                            Phone = reader.GetString("phone"),
                            Email = reader.IsDBNull("email") ? null : reader.GetString("email"),
                            Position = reader.IsDBNull("position") ? null : reader.GetString("position")
                        });
                    }
                    return managers;
                }
            }
        }

        public ManagerDto GetManagerById(int id)
        {
            using (var connection = new NpgsqlConnection(Options.ConnectionString))
            {
                connection.Open();

                var command = new NpgsqlCommand(ManagerQueries.GetManagerById, connection);
                command.Parameters.AddWithValue("@id", id);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new ManagerDto
                        {
                            Id = reader.GetInt32("id"),
                            Name = reader.GetString("name"),
                            Phone = reader.GetString("phone"),
                            Email = reader.IsDBNull("email") ? null : reader.GetString("email"),
                            Position = reader.IsDBNull("position") ? null : reader.GetString("position")
                        };
                    }
                }
                return null;
            }
        }

        public bool ManagerExists(int id)
        {
            var manager = GetManagerById(id);
            return manager != null;
        }
    }
}
