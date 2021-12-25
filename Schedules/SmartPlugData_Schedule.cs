using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using smarthome.Config;
using smarthome.Helper;
using smarthome.Hubs;
using smarthome.Model;
using smarthome.Model.Modules;
using smarthome.Model.Modules.Devices;
using smarthome.Model.Modules.MainModule;
using smarthome.Services;

namespace smarthome.Schedules
{
    public class SmartPlugData_Schedule : IJob
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public SmartPlugData_Schedule(IServiceScopeFactory serviceScopeFactory)
        { 
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            if(InitBoard.ready)
            {
                using (IServiceScope scope = _serviceScopeFactory.CreateScope())
                {  
                    SmartPlugService smartPlugService = scope.ServiceProvider.GetRequiredService<SmartPlugService>();

                    await smartPlugService.CollectDataAsync();

                    await smartPlugService.CheckAlert();
                    
                }
            }
        }
    }
}