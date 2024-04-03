using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BaseLibrary.Entities
{
    public class IoT_Device
    {

        //Relationshiop: Many to one
        public Employee4RetailStore? Employees { get; set; }
    }
}
