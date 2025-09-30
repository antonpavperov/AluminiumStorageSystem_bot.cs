using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AluminiumStorageSystem.CORE.Dtos
{
    public class ApplicationDto
{
    public int Id { get; set; }
    public int SupplierId { get; set; }
    public int ManagerId { get; set; }
    public DateTime ApplicationDate { get; set; }
    public string Status { get; set; }
    public string Comment { get; set; }
    public List<ApplicationItemDto> Items { get; set; } = new List<ApplicationItemDto>();
    }

public class ApplicationItemDto
{
    public int Id { get; set; }
    public int ScrapTypeId { get; set; }
    public decimal Quantity { get; set; }
    public string ScrapTypeName { get; set; }
    public decimal Price { get; set; }
    public decimal TotalPrice { get; set; }
}
}
