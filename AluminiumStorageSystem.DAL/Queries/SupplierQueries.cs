using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AluminiumStorageSystem.DAL.Queries
{
    public static class SupplierQueries
    {
        public const string GetAllSuppliers =
            """
            SELECT id, name
            FROM suppliers 
            ORDER BY name;
            """;

        public const string GetSupplierById =
             """
            SELECT id, name
            FROM suppliers 
            WHERE id = @id;
            """;

        public const string SupplierExists =
            """
            SELECT 1 FROM suppliers WHERE id = @id;
            """;
    }
}
