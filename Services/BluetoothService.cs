using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using smarthome.Config;
using smarthome.Helper.Bluetooth;
using smarthome.Hubs;
using smarthome.Interface;
using smarthome.Model.Bluetooth;
using Tmds.DBus;

#pragma warning disable 0649 // Field is never assigned to, and will always have its default value

namespace smarthome.Services
{
    public class BluetoothService : IDisposable
    {
        Connection connection;
        private string s_mediaPlayerService = "org.bluez";
        string bluetoothInterface = "hci0";

        bool shutdown = false;

        SocketService socketService;

        public BluetoothService(SocketService _socketService , string btInterface = "hci0")
        {
            socketService = _socketService;
            bluetoothInterface = btInterface;
        }

        public void PairingMode()
        {
            if(RuntimeInformation.IsOSPlatform(OSPlatform.Linux))    
            {
                new Thread(async () => 
                {
                    Thread.CurrentThread.IsBackground = false; 

                    Process process = new Process();
                    ProcessStartInfo startInfo = new ProcessStartInfo();
                    startInfo.FileName = "/bin/bluetoothctl";
                    startInfo.RedirectStandardOutput = true;
                    startInfo.RedirectStandardInput = true;
                    process.StartInfo = startInfo;
                    
                    process.Start();

                    process.StandardInput.WriteLine("default-agent");

                    string line = "";
                    string deviceName = "";


                    while (!process.StandardOutput.EndOfStream)
                    {
                        if(shutdown)
                        {
                            process.StandardInput.WriteLine("exit");
                        }

                        char inch = (char)process.StandardOutput.Read();

                        //Console.Write(inch);

                        if(inch != '\n')
                        {
                            line += inch;
                        }
                        else
                        {
                            Console.WriteLine(line);
                            
                            if(line.Contains("NEW") && line.Contains("Device"))
                            {
                                deviceName = "";
                                
                                string[] splitted = line.Split(" ");

                                int index = 0;

                                for(int i = 0; i < splitted.Length; i++)
                                {
                                    if(splitted[i] == "Device")
                                    {
                                        index = i + 2;
                                        break;
                                    }
                                }

                                for (var i = index; i < splitted.Length ; i++)
                                {
                                    deviceName += splitted[i] + " ";
                                }

                                Console.WriteLine(deviceName);
                            }
                            else if(line.Contains("CHG") && line.Contains("Connected: yes"))
                            {
                                deviceName = "";

                                string[] splitted = line.Split("[");
                                string[] deviceNames = splitted[5].Split("]");

                                deviceName = deviceNames[0];

                                Console.WriteLine(deviceName);
                            }

                            if(line == $"\u001b[0;94m[{deviceName}]\u001b[0m# yes")
                            {
                                Console.WriteLine("Finished");
                                config.AllowPair = false;

                                deviceName = "";
                            }

                            if((line.Contains("Request") && line.Contains("confirmation")) || (line.Contains("Authorize") && line.Contains("service")))
                            {
                                if(!config.AllowPair)
                                {
                                    await socketService.SendPairBluetooth(deviceName);                            
                                }

                                DateTime expire = DateTime.Now.AddSeconds(60);

                                while(expire >= DateTime.Now)
                                {
                                    if(config.AllowPair)
                                    {
                                        Console.WriteLine("Pairing ...");
                                        process.StandardInput.WriteLine("yes");
                                        
                                        expire = DateTime.Now;
                                    }
                                }
                            }

                            line = "";
                        }
                    }

                }).Start();
            }
        }

        string GetDeviceId()
        {
            return DeviceHelper.GetConnectedDeviceId();
        }
        async Task<T> GetInterfaceInfo<T>(ObjectPath path)
        {
            connection = new Connection(Address.System);
            
            await connection.ConnectAsync();

            string[] services = await connection.ListServicesAsync();
            string playerService = services.Where(service => service.StartsWith(s_mediaPlayerService, StringComparison.Ordinal)).First();

            var controlPlayer = connection.CreateProxy<T>(playerService , path);

            return (T)controlPlayer;
            
        }

        async Task<T> GetData<T>(IPlayer player , string property)
        {
            try
            {
                return await player.GetAsync<T>(property);
            }
            catch { return default(T) ;}
        }

        public async Task<IPlayer> GetMediaPlayerControl()
        {
            string deviceId = GetDeviceId();

            Console.WriteLine(deviceId);

            if(string.IsNullOrEmpty(deviceId))
                return null;

            ObjectPath s_mediaPlayerPath = new ObjectPath($"/org/bluez/{bluetoothInterface}/dev_{deviceId.Replace(":" , "_")}");

            Console.WriteLine(s_mediaPlayerService.ToString());

            IPlayerInfo controlInfo = await GetInterfaceInfo<IPlayerInfo>(s_mediaPlayerPath);

            ObjectPath playerpath = new ObjectPath();

            try
            {
                playerpath = await controlInfo.GetAsync<ObjectPath>(ControlPlayerInfo.Player);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }

            if(playerpath != new ObjectPath())
            {
                IPlayer player = await GetInterfaceInfo<IPlayer>(playerpath.ToString());

                await player.WatchPropertiesAsync(OnPropertiesChanged);

                return player;
            }

            return null;
        }

        public void MonitorSignal()
        {
            if(RuntimeInformation.IsOSPlatform(OSPlatform.Linux))    
            {
                new Thread(async () => {
                Thread.CurrentThread.IsBackground = false; 

                Process process = new Process();
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = "/bin/dbus-monitor";
                startInfo.Arguments = "--system sender='org.bluez',interface='org.freedesktop.DBus.Properties'";
                startInfo.RedirectStandardOutput = true;
                startInfo.RedirectStandardInput = true;
                process.StartInfo = startInfo;
                
                process.Start();

                string line = "";
                List<string> lines = new List<string>();

                bool statusChanged = false;

                while (!process.StandardOutput.EndOfStream)
                {
                    char inch = (char)process.StandardOutput.Read();

                    if(inch != '\n')
                    {
                        line += inch;

                        if(line == "signal ")
                        {
                            lines.Clear();
                        }
                    }
                    else
                    {
                        //Console.WriteLine(line);
                        lines.Add(line);

                        if(lines.Count > 9)
                        {
                            if(lines[1].Contains("org.bluez.MediaPlayer1"))
                            {
                                string key = lines[4].Split("\"")[1].Trim();
                                string value = lines[5].Split("variant")[1].Trim().Split(" ")[1];

                                if(key == "Status")
                                {
                                    statusChanged = true;
                                    
                                    await socketService.SendTrack_StatusInfo(value);     
                                }
                                if(key == "Position" && statusChanged)
                                {
                                    statusChanged = false;
                                    
                                    await socketService.SendTrack_PositionInfo(value);     
                                }

                            }
                        }

                        line = "";
                    }
                }

            }).Start();
            }
        }

        public async Task<PlayerProperties> GetPlayerInfo()
        {
            PlayerProperties result = new PlayerProperties();

            IPlayer player = await GetMediaPlayerControl();
            
            if(player != null)
            {
                var metadata_dic = await GetData<IDictionary<string, object>>(player , nameof(PlayerProperties.Track));
                if(metadata_dic != null)
                {
                    string title = GetTitle(metadata_dic);
                    UInt32 duration = GetDuration(metadata_dic) / 1000;
                    
                    UInt32 position = await GetData<UInt32>(player , nameof(PlayerProperties.Position))  / 1000;
                    string status = await GetData<string>(player , nameof(PlayerProperties.Status));

                    result.metadatas = metadata_dic;
                    result.Track = title;
                    result.Position = position;
                    result.Status = status;
                    result.PlaybackStatus = (status.Contains("playing"));
                    result.Duration = duration;
                }
            }

            return result;
        }

        public async Task<bool> Play()
        {
            try
            {
                IPlayer player = await GetMediaPlayerControl();

                await player.PlayAsync();

                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.StackTrace);

                return false;
            }
        }

        public async Task<bool> Pause()
        {
            try
            {
                IPlayer player = await GetMediaPlayerControl();

                await player.PauseAsync();

                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.StackTrace);

                return false;
            }
        }

        public async Task<bool> NextTrack()
        {
            try
            {
                IPlayer player = await GetMediaPlayerControl();

                await player.NextAsync();

                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.StackTrace);

                return false;
            }
        }
        
        public async Task<bool> PreviousTrack()
        {
            try
            {
                IPlayer player = await GetMediaPlayerControl();

                await player.PreviousAsync();

                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.StackTrace);

                return false;
            }
        }

        public async Task<bool> Stop()
        {
            try
            {
                IPlayer player = await GetMediaPlayerControl();

                await player.StopAsync();

                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.StackTrace);

                return false;
            }
        }

        public async Task<bool> VolumeUp()
        {
            try
            {
                IPlayer player = await GetMediaPlayerControl();

                await player.VolumeUpAsync();

                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.StackTrace);

                return false;
            }
        }

        public async Task<bool> VolumeDown()
        {
            try
            {
                IPlayer player = await GetMediaPlayerControl();

                await player.VolumeDownAsync();

                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.StackTrace);

                return false;
            }
        }

        private void OnPropertiesChanged(PropertyChanges changes)
        {
            var metadata = changes.Get<IDictionary<string, object>>(nameof(PlayerProperties.Track));
            if (metadata != null)
            {
                Console.WriteLine(metadata.Values);
            }
        }

        private string GetTitle(IDictionary<string, object> metadata)
        {
            if (metadata.ContainsKey("Title"))
            {
                return metadata["Title"] as string;
            }
            else
            {
                return "نامشخص";
            }
        }
        private UInt32 GetDuration(IDictionary<string, object> metadata)
        {
            if (metadata.ContainsKey("Duration"))
            {
                return (UInt32)metadata["Duration"];
            }
            else
            {
                return 0;
            }
        }

        public void Dispose()
        {
            Console.WriteLine("Bluetooth Off");
            
            shutdown = true;
        }
    }
}
