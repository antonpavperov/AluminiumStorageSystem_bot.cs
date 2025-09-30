// AluminiumStorageSystem.UI/Program.cs
using AluminiumStorageSystem.BLL;
using AluminiumStorageSystem.CORE.Dtos;

namespace AluminiumStorageSystem.UI
{
    class Program
    {
        // В Program.cs
        static void Main(string[] args)
        {
            Console.WriteLine("=== Система учета алюминиевого лома ===");

            var applicationService = new ApplicationService();

            try
            {
                // 1. Создаем тестовую заявку
                Console.WriteLine("\nСоздаем тестовую заявку...");
                var application = new ApplicationDto
                {
                    SupplierId = 1,
                    ManagerId = 1,
                    ApplicationDate = DateTime.Now,
                    Status = "Новая",
                    Comment = "Тестовая заявка из консоли",
                    Items = new List<ApplicationItemDto>
            {
                new ApplicationItemDto { ScrapTypeId = 1, Quantity = 100, Price = 50.0m },
                new ApplicationItemDto { ScrapTypeId = 3, Quantity = 200, Price = 75.5m }
            }
                };

                // 2. Сохраняем заявку и получаем её ID
                int createdApplicationId = applicationService.CreateApplication(application);
                Console.WriteLine($"✅ Заявка создана! ID: {createdApplicationId}");

                // 3. Показываем детальную информацию о созданной заявке
                ShowApplicationDetails(applicationService, createdApplicationId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Ошибка: {ex.Message}");
            }

            Console.WriteLine("\nНажмите любую клавишу для выхода...");
            Console.ReadKey();
        }

        // Вынесем в отдельный метод для чистоты
        static void ShowApplicationDetails(ApplicationService applicationService, int applicationId)
        {
            Console.WriteLine("\n=== ДЕТАЛЬНЫЙ ПРОСМОТР ЗАЯВКИ ===");

            try
            {
                var applicationDetails = applicationService.GetApplicationDetails(applicationId);

                if (applicationDetails != null)
                {
                    Console.WriteLine($"📋 ЗАЯВКА #{applicationDetails.Id}");
                    Console.WriteLine($"📅 Дата создания: {applicationDetails.ApplicationDate:dd.MM.yyyy HH:mm}");
                    Console.WriteLine($"📊 Статус: {applicationDetails.Status}");
                    Console.WriteLine($"🏢 Поставщик: {applicationDetails.SupplierName}");
                    Console.WriteLine($"👨‍💼 Менеджер: {applicationDetails.ManagerName}");

                    if (!string.IsNullOrEmpty(applicationDetails.ManagerPhone))
                        Console.WriteLine($"📞 Телефон менеджера: {applicationDetails.ManagerPhone}");

                    if (!string.IsNullOrEmpty(applicationDetails.Comment))
                        Console.WriteLine($"💬 Комментарий: {applicationDetails.Comment}");

                    Console.WriteLine($"\n📦 ПОЗИЦИИ ЗАЯВКИ ({applicationDetails.TotalItems}):");
                    Console.WriteLine("┌──────────────────────────────────┬──────────┬──────────┬──────────┐");
                    Console.WriteLine("│ Тип лома                         │ Кол-во   │ Цена     │ Сумма    │");
                    Console.WriteLine("├──────────────────────────────────┼──────────┼──────────┼──────────┤");

                    foreach (var item in applicationDetails.Items)
                    {
                        Console.WriteLine($"│ {item.ScrapTypeName,-32} │ {item.Quantity,6}кг │ {item.Price,6}₽ │ {item.TotalPrice,7}₽ │");
                    }

                    Console.WriteLine("├──────────────────────────────────┼──────────┼──────────┼──────────┤");
                    Console.WriteLine($"│ {"ВСЕГО",32} │ {applicationDetails.TotalQuantity,6}кг │ {"",6} │ {applicationDetails.TotalAmount,7}₽ │");
                    Console.WriteLine("└──────────────────────────────────┴──────────┴──────────┴──────────┘");
                }
                else
                {
                    Console.WriteLine("❌ Заявка не найдена");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Ошибка при получении заявки: {ex.Message}");
            }
        }

    }
}