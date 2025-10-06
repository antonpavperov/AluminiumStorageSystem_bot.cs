using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AluminiumStorageSystem.CORE
{
    // AluminiumStorageSystem.CORE/Exceptions/
    namespace AluminiumStorageSystem.CORE.Exceptions
    {
        public class EntityNotFoundException : Exception
        {
            public EntityNotFoundException(string message) : base(message) { }
        }

        public class ApplicationNotFoundException : EntityNotFoundException
        {
            public ApplicationNotFoundException(int id)
                : base($"Заявка с ID {id} не найдена") { }
        }

        public class SupplierNotFoundException : EntityNotFoundException
        {
            public SupplierNotFoundException(int id)
                : base($"Поставщик с ID {id} не найден") { }
        }

        public class ManagerNotFoundException : EntityNotFoundException
        {
            public ManagerNotFoundException(int id)
                : base($"Менеджер с ID {id} не найден") { }
        }

        public class ScrapTypeNotFoundException : EntityNotFoundException
        {
            public ScrapTypeNotFoundException(int id)
                : base($"Тип лома с ID {id} не найден") { }
        }
        public class ServiceException : Exception
        {
            public ServiceException(string message) : base(message) { }

            public ServiceException(string message, Exception innerException)
                : base(message, innerException) { }
        }
    }
}
