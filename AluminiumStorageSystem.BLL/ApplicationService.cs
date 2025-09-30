using AluminiumStorageSystem.CORE.Dtos;
using AluminiumStorageSystem.CORE.Dtos.AluminiumStorageSystem.CORE.Dtos;
using AluminiumStorageSystem.DAL.Repositories;

namespace AluminiumStorageSystem.BLL
{
    public class ApplicationService
    {
        private readonly ApplicationRepository _appRepo;
        private readonly ScrapTypeRepository _scrapTypeRepo;
        private readonly SupplierRepository _supplierRepo;
        private readonly ManagerRepository _managerRepo;

        public ApplicationService()
        {
            _appRepo = new ApplicationRepository();
            _scrapTypeRepo = new ScrapTypeRepository();
            _supplierRepo = new SupplierRepository();
            _managerRepo = new ManagerRepository();
        }

        public ApplicationDetailsDto GetApplicationDetails(int applicationId)
        {
            // Получаем заявку из Repository
            var application = _appRepo.GetApplicationById(applicationId);
            if (application == null) return null;

            // Создаем DTO с расшифрованными данными
            var applicationDetails = new ApplicationDetailsDto
            {
                Id = application.Id,
                ApplicationDate = application.ApplicationDate,
                Status = application.Status,
                Comment = application.Comment
            };

            // Расшифровываем Supplier
            var supplier = _supplierRepo.GetSupplierById(application.SupplierId);
            applicationDetails.SupplierName = supplier?.Name ?? "Неизвестный поставщик";

            // Расшифровываем Manager
            var manager = _managerRepo.GetManagerById(application.ManagerId);
            applicationDetails.ManagerName = manager?.Name ?? "Неизвестный менеджер";
            applicationDetails.ManagerPhone = manager?.Phone;

            // Расшифровываем Items с названиями ScrapType
            var scrapTypes = _scrapTypeRepo.GetScrapTypes();
            applicationDetails.Items = application.Items.Select(item =>
            {
                var scrapType = scrapTypes.FirstOrDefault(st => st.Id == item.ScrapTypeId);
                return new ApplicationItemDetailsDto
                {
                    ScrapTypeName = scrapType?.name ?? "Неизвестный тип лома",
                    Quantity = item.Quantity,
                    Price = item.Price,
                    TotalPrice = item.TotalPrice
                };
            }).ToList();

            return applicationDetails;
        }

        // Остальные методы остаются без изменений
        public int CreateApplication(ApplicationDto application)
        {
            ValidateApplication(application);
            return _appRepo.AddApplication(application);
        }

        private void ValidateApplication(ApplicationDto application)
        {
            if (application.Items == null || !application.Items.Any())
                throw new ArgumentException("Заявка должна содержать хотя бы один item");

            if (!_supplierRepo.SupplierExists(application.SupplierId))
                throw new ArgumentException($"Supplier с ID {application.SupplierId} не существует");

            if (!_managerRepo.ManagerExists(application.ManagerId))
                throw new ArgumentException($"Manager с ID {application.ManagerId} не существует");

            foreach (var item in application.Items)
            {
                if (!_scrapTypeRepo.ScrapTypeExists(item.ScrapTypeId))
                    throw new ArgumentException($"ScrapType с ID {item.ScrapTypeId} не существует");
            }
        }

        public List<SupplierDto> GetAvailableSuppliers() => _supplierRepo.GetAllSuppliers();
        public List<ManagerDto> GetAvailableManagers() => _managerRepo.GetAllManagers();
        public List<ScrapTypeDto> GetAvailableScrapTypes() => _scrapTypeRepo.GetScrapTypes();
    }
}

