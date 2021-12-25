using System;
using Newtonsoft.Json;
using smarthome.Model.Modules.Interface;
using smarthome.Model.Modules.MainModule;

namespace smarthome.Model.Camera
{
    public class CameraModel
    {
        public int Id { get; set; }
        public string cameraName { get; set; }
        public string ip { get; set; }
        
    }
}