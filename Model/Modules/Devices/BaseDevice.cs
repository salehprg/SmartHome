using System;
using Newtonsoft.Json;
using smarthome.Model.Modules.Interface;
using smarthome.Model.Modules.MainModule;

namespace smarthome.Model.Modules.Devices
{
    public class BaseDevice : IBaseDevice
    {
        public int Id { get; set; }
        public int moduleParentId { get; set; }
        public virtual Module moduleParent { get; set; }
        public int registerid { get; set; }
        public int RoomId { get; set; }
        public string name { get; set; }
        public string serialNumber {get;set;}
        public string status { get; set; }
        public DeviceType deviceType { get; set; }

        BaseDevice IBaseDevice.GetBase()
        {
            return this;
        }

        public T Cast<T>()
        {
            string serialize = JsonConvert.SerializeObject(this);
            T result = JsonConvert.DeserializeObject<T>(serialize);

            return result;
        }
    }
}