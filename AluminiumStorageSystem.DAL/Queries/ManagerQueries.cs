using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AluminiumStorageSystem.DAL.Queries
{
    public static class ManagerQueries
    {
        public const string GetAllManagers =
            """
            SELECT id, name, phone, email, position 
            FROM managers 
            ORDER BY name;
            """;

        public const string GetManagerById =
            """
            SELECT id, name, phone, email, position 
            FROM managers 
            WHERE id = @id;
            """;

        public const string ManagerExists =
            """
            SELECT 1 FROM managers WHERE id = @id;
            """;
    }
}
