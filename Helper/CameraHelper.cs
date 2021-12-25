using System.IO;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RtspClientSharp;
using smarthome.Model.Camera;

namespace smarthome.Helper
{
    public class CameraHelper
    {
        public bool verifyIp(CameraModel cameraModel)
        {
            string[] sections = cameraModel.ip.Split(".");

            if(sections.Length != 4)
                return false;

            for (int i = 0; i < sections.Length; i++)
            {
                try
                {
                    int result = int.Parse(sections[i]);
                    if(result < 0 && result > 255)
                        return false;
                }
                catch{return false;}
            }

            return true;

        }
        public List<string> FindAllCameraStreamLink()
        {
            try
            {
                List<string> cameras = new List<string>();

                string myIp = "";
            
                var host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        myIp = ip.ToString();
                    }
                }

                string[] ips = myIp.Split('.');

                string localip = $"{ips[0]}.{ips[1]}.{ips[2]}.";
                
                for(int i = 0; i < 255 ;i++)
                {
                    string ip = localip + i.ToString();

                    string rtsp = "rtsp://" + ip;

                    var serverUri = new Uri(rtsp);
                    var connectionParameters = new ConnectionParameters(serverUri);
                    var cancellationTokenSource = new CancellationTokenSource();

                    if(ConnectAsync(connectionParameters, cancellationTokenSource.Token).Result)
                    {
                        cameras.Add(ip);
                    }
                }

                return cameras;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                
                return null;
            }
        }

        private async Task<bool> ConnectAsync(ConnectionParameters connectionParameters, CancellationToken token)
        {
            try
            {
                var rtspClient = new RtspClient(connectionParameters);
                rtspClient.ConnectionParameters.ConnectTimeout = TimeSpan.FromMilliseconds(500);

                await rtspClient.ConnectAsync(token);

                return true;
            }
            catch {return false;}
        }
    }
}