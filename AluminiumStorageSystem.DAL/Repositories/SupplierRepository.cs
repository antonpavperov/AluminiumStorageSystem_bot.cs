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
    public class SupplierRepository
    {
        public List<SupplierDto> GetAllSuppliers()
        {
            using (var connection = new NpgsqlConnection(Options.ConnectionString))
            {
                connection.Open();

                var command = new NpgsqlCommand(SupplierQueries.GetAllSuppliers, connection);

                using (var reader = command.ExecuteReader())
                {
                    var suppliers = new List<SupplierDto>();
                    while (reader.Read())
                    {
                        suppliers.Add(new SupplierDto
                        {
                            Id = reader.GetInt32("id"),
                            Name = reader.GetString("name")
                        });
                    }
                    return suppliers;
                }
            }
        }

        public SupplierDto GetSupplierById(int id)
        {
            using (var connection = new NpgsqlConnection(Options.ConnectionString))
            {
                connection.Open();

                var command = new NpgsqlCommand(SupplierQueries.GetSupplierById, connection);
                command.Parameters.AddWithValue("@id", id);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new SupplierDto
                        {
                            Id = reader.GetInt32("id"),
                            Name = reader.GetString("name")
                        };
                    }
                }
                return null;
            }
        }

        public bool SupplierExists(int id)
        {
            var supplier = GetSupplierById(id);
            return supplier != null;
        }
    }
}
