using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AluminiumStorageSystem.CORE.Dtos
{
    
    namespace AluminiumStorageSystem.CORE.Dtos
    {
        public class ApplicationDetailsDto
        {
            public int Id { get; set; }
            public DateTime ApplicationDate { get; set; }
            public string Status { get; set; }
            public string Comment { get; set; }

            
            public string SupplierName { get; set; }
            public string ManagerName { get; set; }
            public string ManagerPhone { get; set; }

            public List<ApplicationItemDetailsDto> Items { get; set; } = new List<ApplicationItemDetailsDto>();

            
            public decimal TotalAmount => Items.Sum(item => item.TotalPrice);
            public int TotalItems => Items.Count;
            public decimal TotalQuantity => Items.Sum(item => item.Quantity);
        }

        public class ApplicationItemDetailsDto
        {
            public string ScrapTypeName { get; set; }
            public decimal Quantity { get; set; }
            public decimal Price { get; set; }
            public decimal TotalPrice { get; set; }
        }
    }
}
