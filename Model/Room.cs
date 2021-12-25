using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using smarthome.Helper;
using smarthome.Model.Modules;
using smarthome.Model.Modules.Devices;

namespace smarthome.Model
{
    public class Name
    {
        public string text {get; set;}
        public float X {get;set;}
        public float Y {get;set;}
    }
    public class RoomDevices
    {
        public List<DeviceLED> LEDs {get; set;}
        public List<DeviceCurtain> Curtains {get; set;}
        public List<DeviceWindoors> Windoors {get; set;}
        public List<BaseDevice> Smartplugs {get; set;}
    }
    public class Room
    {
        public int RoomId {get; set;}
        public virtual List<BaseDevice> roomDevices {get;set;}

        [NotMapped]
        public RoomDevices devices 
        {
            get
            {
                return GetDevices();
            }
            set{}
        }
        public string area {get;set;}
        public string border {get;set;}

        [JsonIgnore]
        public string roomName {get; set;}

        [JsonIgnore]
        public float X {get;set;}
        
        [JsonIgnore]
        public float Y {get;set;}

        [NotMapped]
        public Name name {
            get{
                Name result = new Name();
                result.text = roomName;
                result.X = X;
                result.Y = Y;

                return result;
            }
            set{
                roomName = name.text;
                X = name.X;
                Y = name.Y;
            }
        }

        public int HomeId {get;set;}
        

        RoomDevices GetDevices()
        {
            RoomDevices result = new RoomDevices();
            result.Curtains = GetDeviceCurtains();
            result.LEDs = GetDeviceLEDs();
            result.Windoors = GetDeviceWindoors();
            result.Smartplugs = GetSmartPlugs();

            return result;
        }
        
        List<BaseDevice> GetSmartPlugs()
        {
            List<BaseDevice> devices = roomDevices.Where(x => x.deviceType == DeviceType.SmartPlug).ToList();

            return devices;
        }
        List<DeviceWindoors> GetDeviceWindoors()
        {
            List<BaseDevice> devices = roomDevices.Where(x => x.deviceType == DeviceType.Windoor).OrderBy(x => x.Id).ToList();

            List<DeviceWindoors> result = new List<DeviceWindoors>();

            foreach (var device in devices)
            {
                result.Add(device.Cast<DeviceWindoors>());
            }

            return result;
        }

        List<DeviceCurtain> GetDeviceCurtains()
        {
            List<BaseDevice> devices = roomDevices.Where(x => x.deviceType == DeviceType.Curtain).OrderBy(x => x.Id).ToList();

            List<DeviceCurtain> result = new List<DeviceCurtain>();

            foreach (var device in devices)
            {
                result.Add(device.Cast<DeviceCurtain>());
            }

            return result;
        }

        List<DeviceLED> GetDeviceLEDs()
        {
            List<BaseDevice> devices = roomDevices.Where(x => x.deviceType == DeviceType.LED).OrderBy(x => x.Id).ToList();

            List<DeviceLED> result = new List<DeviceLED>();

            foreach (var device in devices)
            {
                result.Add(device.Cast<DeviceLED>());
            }

            return result;
        }
    }
}