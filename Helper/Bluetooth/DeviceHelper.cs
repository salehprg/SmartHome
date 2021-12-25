using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Tmds.DBus;

namespace smarthome.Helper.Bluetooth
{
    public class DeviceHelper
    {
        public static string GetConnectedDeviceId()
        {
            if(RuntimeInformation.IsOSPlatform(OSPlatform.Linux))    
            {
                try
                {
                    string devices = ShellHelper.Run("/bin/bluetoothctl" , "devices");

                    var deviceId = devices.Split(" ")[1];
                    Console.WriteLine(deviceId);

                    return deviceId;
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.StackTrace);

                    return "";
                }
                
            }

            return "A4:45:GT:65:G4";
        }
    }
}