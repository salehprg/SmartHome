using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using smarthome.Model.Modules.Devices;

namespace smarthome.Model
{
    public class Scenario
    {
        public int ScenarioId {get; set;}
        public bool running {get;set;}
        public string ScenarioName {get;set;}
        public string cronjob {get;set;}

        [NotMapped]
        public List<BaseDevice> DeviceStats{get;set;}
        public List<ScenarioAction> actions{get;set;}
        
    }
}