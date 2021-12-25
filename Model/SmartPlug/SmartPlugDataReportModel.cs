using System.Collections.Generic;
using smarthome.Model.Modules.MainModule;
using smarthome.Model.Modules.Devices;

namespace smarthome.Model.SmartPlug
{
    public class SmartPlugDataReportModel
    {
        public float price {get;set;}
        public float totalWatt {get;set;}
        public BaseDevice baseDevice {get;set;}
        public List<SmartplugInfoModel> infoModels {get;set;}
    }
}