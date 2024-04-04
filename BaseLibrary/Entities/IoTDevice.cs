using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BaseLibrary.Entities
{
    public class IoTDevice
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        //Relationshiop: Many to one
        public Store? Store { get; set; }
        public int StoreId { get; set; }

        public StoreArea? StoreArea { get; set; }
        public int StoreAreaId { get; set; }

        public Employee4RetailStore? Employees { get; set; }
        public int EmployeesId { get; set; }
    }
}
