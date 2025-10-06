// AluminiumStorageSystem.UI/Program.cs
using AluminiumStorageSystem.BLL;

using AluminiumStorageSystem.CORE.Dtos;

namespace AluminiumStorageSystem.UI
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Система учета алюминиевого лома ===");

            var applicationService = new ApplicationService();

            try
            {
                
                Console.WriteLine("\n📋 ДОСТУПНЫЕ ТИПЫ ЛОМА:");
                var scrapTypes = applicationService.GetAvailableScrapTypes();
                for (int i = 0; i < scrapTypes.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {scrapTypes[i].name}");
                }

                
                Console.WriteLine("\n🏢 ДОСТУПНЫЕ ПОСТАВЩИКИ:");
                var suppliers = applicationService.GetAvailableSuppliers();
                for (int i = 0; i < suppliers.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {suppliers[i].Name}");
                }

                
                Console.WriteLine("\n👨‍💼 ДОСТУПНЫЕ МЕНЕДЖЕРЫ:");
                var managers = applicationService.GetAvailableManagers();
                for (int i = 0; i < managers.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {managers[i].Name} ({managers[i].Phone})");
                }

                
                CreateApplicationWithUserInput(applicationService, scrapTypes, suppliers, managers);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Ошибка: {ex.Message}");
            }

            Console.WriteLine("\nНажмите любую клавишу для выхода...");
            Console.ReadKey();
        }

        static void CreateApplicationWithUserInput(ApplicationService applicationService,
            List<ScrapTypeDto> scrapTypes, List<SupplierDto> suppliers, List<ManagerDto> managers)
        {
            Console.WriteLine("\n=== СОЗДАНИЕ НОВОЙ ЗАЯВКИ ===");

            
            Console.WriteLine("\n🔹 ВЫБОР ПОСТАВЩИКА:");
            int supplierIndex = GetUserChoiceIndex("Выберите поставщика (введите номер): ", suppliers.Count);
            int supplierId = suppliers[supplierIndex].Id;

            
            Console.WriteLine("\n🔹 ВЫБОР МЕНЕДЖЕРА:");
            int managerIndex = GetUserChoiceIndex("Выберите менеджера (введите номер): ", managers.Count);
            int managerId = managers[managerIndex].Id;

           
            string comment = GetUserInput("\n💬 Введите комментарий к заявке (или Enter чтобы пропустить): ");

            
            var application = new ApplicationDto
            {
                SupplierId = supplierId,
                ManagerId = managerId,
                ApplicationDate = DateTime.Now,
                Status = "Новая",
                Comment = string.IsNullOrWhiteSpace(comment) ? null : comment
            };

            
            Console.WriteLine("\n📦 ДОБАВЛЕНИЕ ПОЗИЦИЙ ЗАЯВКИ:");
            bool addMoreItems = true;
            int itemNumber = 1;

            while (addMoreItems)
            {
                Console.WriteLine($"\n--- 📍 ПОЗИЦИЯ #{itemNumber} ---");

                
                Console.WriteLine("🔹 ВЫБОР ТИПА ЛОМА:");
                int scrapTypeIndex = GetUserChoiceIndex("Выберите тип лома (введите номер): ", scrapTypes.Count);
                int scrapTypeId = scrapTypes[scrapTypeIndex].Id;

                
                decimal quantity = GetDecimalInput("Введите количество (кг): ");

                // Ввод цены
                decimal price = GetDecimalInput("Введите цену за кг (₽): ");

                
                application.Items.Add(new ApplicationItemDto
                {
                    ScrapTypeId = scrapTypeId,
                    Quantity = quantity,
                    Price = price
                });

                Console.WriteLine($"✅ Позиция #{itemNumber} добавлена: {scrapTypes[scrapTypeIndex].name} - {quantity}кг × {price}₽");

                
                Console.Write("\nДобавить еще одну позицию? (y/n): ");
                addMoreItems = Console.ReadLine()?.ToLower() == "y";
                itemNumber++;
            }

            
            Console.WriteLine($"\n📊 ИТОГО ПОЗИЦИЙ: {application.Items.Count}");
            Console.WriteLine($"💰 ОБЩАЯ СУММА: {application.Items.Sum(item => item.Quantity * item.Price)}₽");

            Console.Write("\nСоздать заявку? (y/n): ");
            if (Console.ReadLine()?.ToLower() == "y")
            {
                Console.WriteLine("\n⏳ Создаем заявку...");
                int createdApplicationId = applicationService.CreateApplication(application);
                Console.WriteLine($"✅ Заявка создана! ID: {createdApplicationId}");

                
                ShowApplicationDetails(applicationService, createdApplicationId);
            }
            else
            {
                Console.WriteLine("❌ Создание заявки отменено");
            }
        }

        static int GetUserChoiceIndex(string prompt, int maxIndex)
        {
            while (true)
            {
                Console.Write(prompt);
                if (int.TryParse(Console.ReadLine(), out int choice) && choice >= 1 && choice <= maxIndex)
                {
                    return choice - 1; 
                }
                Console.WriteLine($"❌ Неверный номер. Введите число от 1 до {maxIndex}.");
            }
        }

        static decimal GetDecimalInput(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                if (decimal.TryParse(Console.ReadLine(), out decimal value) && value > 0)
                {
                    return value;
                }
                Console.WriteLine("❌ Неверное значение. Введите положительное число.");
            }
        }

        static string GetUserInput(string prompt)
        {
            Console.Write(prompt);
            return Console.ReadLine() ?? string.Empty;
        }

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
                    Console.WriteLine("┌──────────────────────────────────┬──────────┬────────────┬────────────┐");
                    Console.WriteLine("│ Тип лома                         │ Кол-во   │ Цена       │ Сумма      │");
                    Console.WriteLine("├──────────────────────────────────┼──────────┼────────────┼────────────┤");

                    foreach (var item in applicationDetails.Items)
                    {
                        Console.WriteLine($"│ {item.ScrapTypeName,-32} │ {item.Quantity,6}кг │ {item.Price,8} руб │ {item.TotalPrice,8} руб │");
                    }

                    Console.WriteLine("├──────────────────────────────────┼──────────┼────────────┼────────────┤");
                    Console.WriteLine($"│ {"ВСЕГО",32} │ {applicationDetails.TotalQuantity,6}кг │ {"",10} │ {applicationDetails.TotalAmount,8} руб │");
                    Console.WriteLine("└──────────────────────────────────┴──────────┴────────────┴────────────┘");
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
    
