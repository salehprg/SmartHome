
using System.Collections.Generic;
using smarthome.Model.Modules.Devices;
using smarthome.Model.Modules.Interface;

namespace smarthome.Model.Modules.MainModule
{
    public class Module
    {
        public int Id {get; set;}
        public int slave_id {get; set;}
        public string serialNumber{get;set;}
        public ModuleType moduleType {get;set;}

    }
}