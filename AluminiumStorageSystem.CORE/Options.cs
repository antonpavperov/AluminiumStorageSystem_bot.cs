using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AluminiumStorageSystem.CORE
{
    public static class Options
    {
      
        public static string ConnectionString
        {
            get
            {
                return "Server = localhost; Port = 5432; User Id = postgres; Password = Gaefamafa12; Database = Aluminium Storage System;";
            }
        }
    }

}
