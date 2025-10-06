using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AluminiumStorageSystem.CORE;
using AluminiumStorageSystem.CORE.AluminiumStorageSystem.CORE.Exceptions;
using AluminiumStorageSystem.CORE.Dtos;
using AluminiumStorageSystem.DAL.Queries;
using Npgsql;

namespace AluminiumStorageSystem.DAL.Repositories
{
    public class ScrapTypeRepository
    {
        public List<ScrapTypeDto> GetScrapTypes()
        {
            using (var connection = new NpgsqlConnection(Options.ConnectionString))
            {
                connection.Open();

                var command = new NpgsqlCommand(ScrapTypeQueries.GetScrapTypes, connection);

                using (var reader = command.ExecuteReader())
                {
                    var scrapTypes = new List<ScrapTypeDto>();

                    while (reader.Read())
                    {
                        scrapTypes.Add(new ScrapTypeDto
                        {
                            Id = reader.GetInt32("id"),
                            name = reader.GetString("name")
                        });
                    }

                    return scrapTypes;
                }
            }
        }

        public ScrapTypeDto GetScrapTypeById(int id)
        {
            using (var connection = new NpgsqlConnection(Options.ConnectionString))
            {
                connection.Open();

                var command = new NpgsqlCommand(ScrapTypeQueries.GetScrapTypeById, connection);
                command.Parameters.AddWithValue("@id", id);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new ScrapTypeDto
                        {
                            Id = reader.GetInt32("id"),
                            name = reader.GetString("name")
                        };
                    }
                }

                throw new ScrapTypeNotFoundException(id); 
            }
        }
        public bool ScrapTypeExists(int id)
        {
            var scrapType = GetScrapTypeById(id);
            return scrapType != null;
        }
    }
}