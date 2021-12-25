using smarthome.Model.Modules.Devices;

namespace smarthome.Model
{
    public class ScenarioAction
    {
        public int Id {get;set;}
        public int ScenarioId {get;set;}
        public int deviceId {get;set;}
        public BaseDevice device {get;set;}
        public double delay {get;set;}
        public string status {get;set;}
    }
}