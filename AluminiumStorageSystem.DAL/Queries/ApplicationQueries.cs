using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AluminiumStorageSystem.DAL.Queries
{
    public class ApplicationQueries
    {
        public const string AddApplication =
            """
            INSERT INTO applications (supplier_id, manager_id, application_date, status, comment)
            VALUES (@supplierId, @managerId, @applicationDate, @status, @comment)
            RETURNING id;
            """;

        public const string AddApplicationItem =
            """
            INSERT INTO application_items (application_id, scrap_type_id, quantity, price)
            VALUES (@applicationId, @scrapTypeId, @quantity, @price);
            """;

        public const string GetApplicationById =
        """
        SELECT 
            id,
            supplier_id,
            manager_id, 
            application_date,
            status,
            comment
        FROM applications 
        WHERE id = @id;
        """;

        public const string GetApplicationItems =
        """
        SELECT 
            id,
            scrap_type_id,
            quantity,
            price
        FROM application_items 
        WHERE application_id = @applicationId;
        """;
    }
}
