using System;
using Newtonsoft.Json;
using smarthome.Model.Modules.MainModule;

namespace smarthome.Model.Modules.Devices
{
    public class DeviceCurtain : BaseDevice
    {
        public int range 
        {
            get {
                return (string.IsNullOrEmpty(status) ? 0 : int.Parse(status));
            }
            set {
                int result = -1;

                int.TryParse(value.ToString() , out result);

                if(result == -1)
                    status = "0";

                status = value.ToString();
            }
        }

    }
}