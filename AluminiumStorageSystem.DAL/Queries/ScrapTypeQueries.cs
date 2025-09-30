using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AluminiumStorageSystem.DAL.Queries
{
    public static class ScrapTypeQueries
    {
        public const string GetScrapTypes =
            """
            SELECT 
                id,
                name
            FROM scrap_types;
            """;

        public const string GetScrapTypeById =
            """
            SELECT 
                id,
                name
            FROM scrap_types
            WHERE id = @id;
            """;
    }

}
