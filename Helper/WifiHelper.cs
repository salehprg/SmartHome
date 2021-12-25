using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using QRCoder;
using smarthome.Config;
using smarthome.Model.ConnectionInfo;
using smarthome.Model.WifiHelper;

namespace smarthome.Helper
{
    public class WifiHelper
    {
        private string Run(string filename , string args)
        {
            return ShellHelper.Run(filename , args);
        }
        public List<WifiResult> GetWifiAPs()
        {
            try
            {
                List<WifiResult> APs = new List<WifiResult>();

                string fileName = "";
                string argStr = "";
                
                if(RuntimeInformation.IsOSPlatform(OSPlatform.Linux))    
                {
                    fileName = "/sbin/iwlist";
                    argStr = "wlan0 scan";
                }
                else if(RuntimeInformation.IsOSPlatform(OSPlatform.Windows))    
                {
                    fileName = "netsh.exe";
                    argStr = "wlan show networks";
                }

                string result = Run(fileName , argStr);

                if(RuntimeInformation.IsOSPlatform(OSPlatform.Linux))    
                {
                    string[] ssids = result.Split("\n");

                    string quality = "";
                    string ssid = "";

                    for (int i = 1; i < ssids.Length; i++)
                    {
                        if(ssids[i].IndexOf("Quality=") > 0)
                        {
                            quality = ssids[i].Split("Quality=")[1].Split(" ")[0].Trim();
                        }
                        if(ssids[i].IndexOf("ESSID:") > 0)
                        {
                            ssid = ssids[i].Split("ESSID:")[1].Replace("\"" , "").Trim();
                        }

                        if(ssid != string.Empty && quality != string.Empty)
                        {
                            WifiResult wifiResult = new WifiResult();
                            wifiResult.APName = ssid;
                            wifiResult.Quality = quality;

                            APs.Add(wifiResult);

                            ssid = "";
                            quality = "";
                        }
                    }
                }
                else if(RuntimeInformation.IsOSPlatform(OSPlatform.Windows))    
                {
                    string[] aps = result.Split('\n');
                    int start = 4;
                    
                    for(int i = 0; i < (aps.Length - start) / 5; i++)
                    {
                        string ssid = aps[start + i * 5];

                        string name = ssid.Trim().Split(":")[1].Remove(0 , 1);

                        if(name != string.Empty)
                        {
                            WifiResult wifiResult = new WifiResult();
                            wifiResult.APName = name;

                            APs.Add(wifiResult);
                        }               
                    }
                }

                return APs;
            }
            catch(Exception ex)
            {
                Console.Write(ex.Message);
                return null;
            }
        }
    
        public bool ConnectToWifi(string APname , string password)
        {
            try
            {
                string fileName = "";
                string argStr = "";

                string curDir = Directory.GetCurrentDirectory();

                if(RuntimeInformation.IsOSPlatform(OSPlatform.Linux))    
                {
                    string wificonfig = File.ReadAllText(config.GetWIFIConfDir());
                    wificonfig = wificonfig.Replace(config.WifiSSID , APname);
                    wificonfig = wificonfig.Replace(config.WifiPass , password);

                    Console.WriteLine(wificonfig);

                    if(!File.Exists(curDir + "/wpa_supplicant.conf"))
                    {
                        FileStream fs = File.Create(curDir + "/wpa_supplicant.conf");
                        fs.Close();
                    }

                    File.WriteAllText("wpa_supplicant.conf" , wificonfig);

                    fileName = "/bin/cp";
                    argStr = curDir + "/wpa_supplicant.conf /etc/wpa_supplicant/wpa_supplicant.conf";

                    Console.WriteLine(fileName + " " + argStr);
                    Run(fileName , argStr);

                    fileName = "/sbin/wpa_cli";
                    argStr = "-i wlan0 reconfigure";

                    Console.WriteLine(fileName + " " + argStr);
                    Run(fileName , argStr);

                    fileName = "/sbin/dhclient";
                    argStr = "-r";

                    Console.WriteLine(fileName + " " + argStr);
                    Run(fileName , argStr);

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return false;
            }
        }

        public string getWifiConnected()
        {
            try
            {
                if(RuntimeInformation.IsOSPlatform(OSPlatform.Linux))    
                {
                    string fileName = "";
                    string argStr = "";

                    fileName = "/sbin/iwconfig";
                    argStr = "wlan0";

                    Console.WriteLine(fileName + " " + argStr);
                    string wifidetails = Run(fileName , argStr);

                    string[] lines = wifidetails.Split("\n");
                    string[] essidDetail = lines[0].Trim().Split("ESSID:");
                    string apName = essidDetail[1].Replace("\"" , "");

                    if(lines[1].IndexOf("Frequency") <= 0)
                    {
                        return null;
                    }
                    else
                    {
                        return apName;
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);

                return "";
                throw;
            }
        }

        public string getMyIp()
        {
            string myIp = "";
            
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    myIp = ip.ToString();
                }
            }

            return myIp;
        }

        public ConnectionInfo MakeQrCode()
        {
            ConnectionInfo info = new ConnectionInfo();

            string ip = getMyIp();

            string apName = getWifiConnected();
                
            info.ApName = apName;
            info.Ip = ip;

            QRCodeGenerator _qrCode = new QRCodeGenerator();      
            QRCodeData _qrCodeData = _qrCode.CreateQrCode("http://" + ip , QRCodeGenerator.ECCLevel.Q);      
            SvgQRCode qrCode = new SvgQRCode(_qrCodeData);      
            string qrCodeImage = qrCode.GetGraphic(3); 

            string qrImagename = "qrCodeImage.svg";

            FileStream fs = File.Create(qrImagename);
            fs.Close();
            File.WriteAllText(qrImagename , qrCodeImage);
            
            info.image = qrCodeImage;    

            return info;
        }
    }
}