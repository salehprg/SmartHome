
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
using smarthome.Model;
using smarthome.Model.Bluetooth;
using smarthome.Model.Modules.Devices;
using smarthome.Model.Modules.MainModule;
using Tmds.DBus;

#pragma warning disable 0649 // Field is never assigned to, and will always have its default value

namespace smarthome.Services
{
    public class SocketService
    {

        private readonly IHubContext<EntryHub> hubContext;
        
        public SocketService(IHubContext<EntryHub> _entryHub)
        {
            if(_entryHub != null)
                hubContext = _entryHub;
            else
                throw new Exception("hub cannot be Empty");
        }

        private async Task send(string method , object data)
        {
            await hubContext.Clients.All.SendAsync(method , data);
        }

        public async Task SendAlertSmartPlug(float totatlPrice)
        {
            await send(config.AlertEvent , totatlPrice);
        }

        public async Task SendHomeData(Home home)
        {
            await send(config.DeviceData , home);
        }
        public async Task SendDeviceInfo(BaseDevice baseDevice)
        {
            await send(config.DeviceData , baseDevice);
        }
        public async Task SendDeviceInfo(List<BaseDevice> devices)
        {
            await send(config.DeviceData , devices);
        }

        public async Task SendModuleInfo(Module module)
        {
            await send(config.ModuleData , module);
        }

        public async Task SendRoomData(Room room)
        {
            await send(config.roomsData , room);
        }

        public async Task SendPairBluetooth(string deviceName)
        {
            await send(config.PairBluetooth , deviceName);
        }

        public async Task SendTrack_StatusInfo(string status)
        {
            await send(config.Track_status , status);
        }
        public async Task SendTrack_PositionInfo(string position)
        {
            await send(config.Track_pos , position);
        }
        public async Task SendTrack_NameInfo(string trackname)
        {
            await send(config.Track_Name , trackname);
        }
    }
}
