using Npgsql;
using System.Data;
using AluminiumStorageSystem.CORE;
using AluminiumStorageSystem.CORE.Dtos;
using AluminiumStorageSystem.DAL.Queries;
using static System.Net.Mime.MediaTypeNames;
using AluminiumStorageSystem.CORE.AluminiumStorageSystem.CORE.Exceptions;

namespace AluminiumStorageSystem.DAL.Repositories
{
    public class ApplicationRepository
    {
        
        public int AddApplication(ApplicationDto application)
        {
            using (var connection = new NpgsqlConnection(Options.ConnectionString))
            {
                connection.Open();

                
                int newApplicationId = CreateApplicationRecord(application, connection);

               
                AddApplicationItems(newApplicationId, application.Items, connection);

                return newApplicationId;
            }
        }

        
        public ApplicationDto GetApplicationById(int id)
        {
            using (var connection = new NpgsqlConnection(Options.ConnectionString))
            {
                connection.Open();

                
                ApplicationDto application = GetApplicationBasicInfo(id);
                if (application == null) return null;

               
                application.Items = GetApplicationItems(id);

                return application;
            }
        }

        
        private int CreateApplicationRecord(ApplicationDto application, NpgsqlConnection connection)
        {
            var command = new NpgsqlCommand(ApplicationQueries.AddApplication, connection);
            command.Parameters.AddWithValue("@supplierId", application.SupplierId);
            command.Parameters.AddWithValue("@managerId", application.ManagerId);
            command.Parameters.AddWithValue("@applicationDate", application.ApplicationDate);
            command.Parameters.AddWithValue("@status", application.Status);
            command.Parameters.AddWithValue("@comment", (object)application.Comment ?? DBNull.Value);

            return (int)command.ExecuteScalar();
        }

        
        private void AddApplicationItems(int applicationId, List<ApplicationItemDto> items, NpgsqlConnection connection)
        {
            if (!items.Any()) return;

            foreach (var item in items)
            {
                var command = new NpgsqlCommand(ApplicationQueries.AddApplicationItem, connection);
                command.Parameters.AddWithValue("@applicationId", applicationId);
                command.Parameters.AddWithValue("@scrapTypeId", item.ScrapTypeId);
                command.Parameters.AddWithValue("@quantity", item.Quantity);
                command.Parameters.AddWithValue("@price", item.Price);
                command.ExecuteNonQuery();
            }
        }


        public ApplicationDto GetApplicationBasicInfo(int id)
{
    using (var connection = new NpgsqlConnection(Options.ConnectionString))
    {
        connection.Open();
        
        var command = new NpgsqlCommand(ApplicationQueries.GetApplicationById, connection);
        command.Parameters.AddWithValue("@id", id);

        using (var reader = command.ExecuteReader())
        {
            if (reader.Read())
            {
                return new ApplicationDto
                {
                    Id = reader.GetInt32("id"),
                    SupplierId = reader.GetInt32("supplier_id"),
                    ManagerId = reader.GetInt32("manager_id"),
                    ApplicationDate = reader.GetDateTime("application_date"),
                    Status = reader.GetString("status"),
                    Comment = reader.IsDBNull("comment") ? null : reader.GetString("comment")
                };
            }
        }
        
        // ✅ ВЕЗДЕ исключения вместо null
        throw new ApplicationNotFoundException(id);
    }
}

public List<ApplicationItemDto> GetApplicationItems(int applicationId)
{
    using (var connection = new NpgsqlConnection(Options.ConnectionString))
    {
        connection.Open();

        var items = new List<ApplicationItemDto>();
        var command = new NpgsqlCommand(ApplicationQueries.GetApplicationItems, connection);
        command.Parameters.AddWithValue("@applicationId", applicationId);

        using (var reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                items.Add(new ApplicationItemDto
                {
                    Id = reader.GetInt32("id"),
                    ScrapTypeId = reader.GetInt32("scrap_type_id"),
                    Quantity = reader.GetDecimal("quantity"),
                    Price = reader.GetDecimal("price")
                });
            }
        }
        
        // ✅ Возвращаем пустой список вместо null
        return items;
    }
}
}}