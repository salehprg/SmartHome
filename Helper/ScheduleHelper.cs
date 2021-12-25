using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using smarthome.Hubs;
using smarthome.Model;
using smarthome.Model.Modules.Devices;
using smarthome.Services;

namespace smarthome.Helper
{
    public class ScheduleHelper
    {
        DbContextModel dbContext;
        DeviceService deviceService;

        public ScheduleHelper(DbContextModel _dbContext ,
                                DeviceService _deviceService)
        {
            deviceService = _deviceService;

            dbContext = _dbContext;
        }
        public async Task<bool> startSchedule(int scenarioId)
        {
            try
            {
                List<ScenarioAction> actions = dbContext.ScenarioActions.Where(x => x.ScenarioId == scenarioId).ToList();

                foreach (var action in actions)
                {
                    BaseDevice device = dbContext.devices.Include(x => x.moduleParent).Where(x => x.Id == action.deviceId).FirstOrDefault();

                    bool result = await deviceService.setDeviceStatus(device , action.status);
                    Thread.Sleep(100);

                    if(result)
                    {
                        Thread.Sleep((int)action.delay * 1000);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return false;
                throw;
            }
        }
    }
}