using System;
using smarthome.Config;
using smarthome.Model;
using smarthome.Model.Modules;
using smarthome.Model.Modules.Devices;
using smarthome.Model.Modules.MainModule;

namespace smarthome.Services
{
    public class SerialService
    {

        public DeviceType decoderDevice(BaseDevice baseDevice)
        {
            try
            {
                DeviceType result; 

                string[] datas = baseDevice.serialNumber.Split(config.splitchar);

                result = (DeviceType)int.Parse(datas[config.type_Indx]);

                if(result > DeviceType.SmartPlug || result < DeviceType.LED)
                    return 0;

                return result;
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return 0;
            }
        }

        public Module decodeModule(Module module)
        {
            Module result = module;

            string[] datas = module.serialNumber.Split(config.splitchar);

            result.moduleType = (ModuleType)int.Parse(datas[config.type_Indx]);

            return result;
        }
    }
}