using System.Text.Json.Serialization;

namespace BaseLibrary.Entities
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        //Relationshiop: One To Many
        [JsonIgnore]
        public List<IoTDevice>? ioTDDevices { get; set; }
    }
}