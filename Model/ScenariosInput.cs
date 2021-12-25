using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using smarthome.Model.Modules.Devices;

namespace smarthome.Model
{
    public class ScenariosInput : Scenario
    {
        public List<ScenarioAction> _actions{get;set;}
        
    }
}