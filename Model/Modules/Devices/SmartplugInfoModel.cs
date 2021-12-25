using System;
using smarthome.Model.Modules.MainModule;

namespace smarthome.Model.Modules.Devices
{
    public class SmartplugInfoModel
    {
        public int Id {get;set;}
        public DateTime time {get;set;}
        public float voltage {get;set;}
        public float current {get;set;}
        public float watt {get;set;}
        public int baseDeviceId {get;set;}
        public virtual BaseDevice baseDevice {get;set;}
        
    }
}