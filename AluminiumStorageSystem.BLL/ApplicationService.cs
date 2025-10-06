using AluminiumStorageSystem.CORE.AluminiumStorageSystem.CORE.Exceptions;
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
            try
            {
                var application = _appRepo.GetApplicationById(applicationId);

                
                var scrapTypes = _scrapTypeRepo.GetScrapTypes();
                var supplier = _supplierRepo.GetSupplierById(application.SupplierId); // ← ПРАВИЛЬНЫЙ репозиторий
                var manager = _managerRepo.GetManagerById(application.ManagerId);

                var applicationDetails = new ApplicationDetailsDto
                {
                    Id = application.Id,
                    ApplicationDate = application.ApplicationDate,
                    Status = application.Status,
                    Comment = application.Comment,
                    
                    SupplierName = supplier?.Name ?? "Неизвестный поставщик",
                    ManagerName = manager?.Name ?? "Неизвестный менеджер",
                    ManagerPhone = manager?.Phone,
                    Items = application.Items.Select(item =>
                    {
                        var scrapType = scrapTypes.FirstOrDefault(st => st.Id == item.ScrapTypeId);
                        return new ApplicationItemDetailsDto
                        {
                            ScrapTypeName = scrapType?.name ?? "Неизвестный тип лома",
                            Quantity = item.Quantity,
                            Price = item.Price,
                            TotalPrice = item.TotalPrice
                        };
                    }).ToList()
                };

                return applicationDetails;
            }
            catch (ApplicationNotFoundException ex)
            {
                
                throw;
            }
            catch (Exception ex)
            {
                
                throw new Exception($"Ошибка при получении заявки {applicationId}", ex);
            }
        }
        public ApplicationDto GetApplicationById(int id)
        {
            try
            {
                
                var application = _appRepo.GetApplicationBasicInfo(id);
                var items = _appRepo.GetApplicationItems(id);

                application.Items = items;
                return application;
            }
            catch (ApplicationNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ServiceException($"Ошибка при получении заявки {id}", ex);
            }
        }

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

