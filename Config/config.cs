using System.IO;

namespace smarthome.Config
{
    public class config
    {

    #region Modbus Configuration
        public static readonly string portLinux = "/dev/serial0";
        public static readonly string portWindows = "COM5";
        public static readonly int txControllPin = 18;       
        public static readonly int baudrate = 115200;    

        public static int GPIOSet_timeout = 25;      

    #endregion

    #region ModbusRegistryConfiguration


        public static readonly int On = 1;
        public static readonly int Off = 0;

        public static readonly int default_slaveId = 1;

        public static readonly int reg_mac_1 = 0;
        public static readonly int reg_mac_2 = 1;
        public static readonly int reg_type = 2;
        public static readonly int reg_function = 3;

        public static readonly int reg_last_info = 10;


    #endregion

    #region ModbusMeasureADSConfig
        public static readonly int reg_volt_measure = 9;
        public static readonly int reg_reset_measure = reg_last_info;
        public static readonly int data_interval = 60;

    #endregion


    #region SerialData_Configuration
        public static readonly int serial_length = 8;

        public static readonly int type_Indx = 0;
        public static readonly string splitchar = " ";
        

    #endregion
        public static readonly int MaxPriceSmartPlug = 500000;

    #region Websocke Event Name

        public static bool AllowPair = false;

        public static readonly string Track_status = "trackstatus";
        public static readonly string Track_pos = "trackposition";
        public static readonly string Track_Name = "trackname";

        public static readonly string PairBluetooth = "pairBluetooth";
        public static readonly string roomsData = "rooms";
        public static readonly string DeviceData = "devices";
        public static readonly string ModuleData = "modules";
        public static readonly string AlertEvent = "AlertSmartPlug";


    #endregion

    #region WifiConfiguration
        private static string[] wifiConfigure =  {"ctrl_interface=DIR=/var/run/wpa_supplicant GROUP=netdev",
                                        "update_config=1",
                                        "country=IR",
                                        "network={",
                                        "   ssid=\"SSID\"",
                                        "   psk=\"PASS\"",
                                        "}"};
        public static string WifiPass = "PASS";
        public static string WifiSSID = "SSID";
        public static string GetWIFIConfDir()
        {
            string dir = Directory.GetCurrentDirectory();

            return dir + "/_WiFi.conf";
        }

        public static void makeWPASupplicant()
        {
            string confDir = GetWIFIConfDir();

            if(!File.Exists(confDir))
            {
                FileStream fs = File.Create(confDir);
                fs.Close();
            }

            File.WriteAllLines(confDir , wifiConfigure);
        }

    #endregion

    }
}