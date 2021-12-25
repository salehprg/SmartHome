using Newtonsoft.Json;
using smarthome.Config;
using smarthome.Model.Modules.MainModule;

namespace smarthome.Model.Modules.Devices
{
    public class DeviceWindoors : BaseDevice
    {
        public bool isOpen
        {
            get {
                bool result = false;

                if(status == config.On.ToString())
                    result = true;
                else if(status == config.Off.ToString())
                    result = false;
                else
                    result = string.IsNullOrEmpty(status) ? false : bool.Parse(status);

                return result;
            }
            set {
                try
                {
                    bool.Parse(value.ToString());

                    status = value.ToString();
                }
                catch
                {
                    status = "false";
                }
            }
        }
    }
}