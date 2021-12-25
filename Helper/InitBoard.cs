using System;
using System.IO.Ports;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using smarthome.Config;
using smarthome.Hubs;
using smarthome.Model;
using smarthome.Services;

namespace smarthome.Helper
{
    public class InitBoard
    {
        public static bool ready = false;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public InitBoard(IServiceScopeFactory serviceScopeFactory)
        { 
            _serviceScopeFactory = serviceScopeFactory;
        }
        public async Task Init()
        {
            ready = true;
            using (IServiceScope scope = _serviceScopeFactory.CreateScope())
            {
                DbContextModel dbContextModel = scope.ServiceProvider.GetRequiredService<DbContextModel>();
                DeviceService deviceService = scope.ServiceProvider.GetRequiredService<DeviceService>();

                var devices = dbContextModel.devices.Include(x => x.moduleParent).AsSplitQuery().ToList();

                foreach (var device in devices)
                {
                    try
                    {
                        await deviceService.setDeviceStatus(device , device.status , false);
                    }
                    catch {}
                }
            }

            
        }
    }
}