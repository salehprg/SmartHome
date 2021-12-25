using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using smarthome.Config;
using smarthome.Helper;
using smarthome.Hubs;
using smarthome.Model;
using smarthome.Services;

namespace smarthome.Schedules
{
    public class JobHandler : IJob
    {
        private readonly IServiceProvider _provider;
        public JobHandler(IServiceProvider provider)
        {
            _provider = provider;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            if(InitBoard.ready)
            {
                using(var scope = _provider.CreateScope())
                {
                    var deviceService = scope.ServiceProvider.GetService<DeviceService>();
                    var dbContext = scope.ServiceProvider.GetService<DbContextModel>();

                    ScheduleHelper scheduleHelper = new ScheduleHelper(dbContext , deviceService);

                    int scenarioId = int.Parse(context.JobDetail.Key.Name);

                    await scheduleHelper.startSchedule(scenarioId);

                }
            }
        }
    }
}