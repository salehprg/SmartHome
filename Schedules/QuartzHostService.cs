using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Spi;
using smarthome.Model;

namespace smarthome.Schedules
{
    public class QuartzHostedService : IHostedService
    {
        private readonly ISchedulerFactory _schedulerFactory;
        private readonly IServiceProvider _provider;
        private readonly IJobFactory _jobFactory;

        public QuartzHostedService(
            ISchedulerFactory schedulerFactory,
            IJobFactory jobFactory,
            IServiceProvider provider)
        {
            _schedulerFactory = schedulerFactory;
            _provider = provider;
            _jobFactory = jobFactory;
        }
        public IScheduler Scheduler { get; set; }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Scheduler = await _schedulerFactory.GetScheduler(cancellationToken);
            Scheduler.JobFactory = _jobFactory;

            string smartplugKey = "SP_1";
            IJobDetail job_sp = JobBuilder.Create<SmartPlugData_Schedule>()
                .WithIdentity(smartplugKey , "group" + smartplugKey)
                .Build();

            ITrigger trigger_sp = TriggerBuilder.Create()
                .WithIdentity("trigger" + smartplugKey, "group" + smartplugKey)
                .StartNow()
                .WithCronSchedule("0 0/15 * ? * * *")
                .Build();

            await Scheduler.ScheduleJob(job_sp, trigger_sp, cancellationToken);

            using(var scope = _provider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetService<DbContextModel>();

                // foreach (var scenario in dbContext.Scenarios)
                // {
                //     // Console.WriteLine(scenario.cronjob);
                //     string key = scenario.ScenarioId.ToString();

                //     IJobDetail job = JobBuilder.Create<JobHandler>()
                //         .WithIdentity(key, "group" + key)
                //         .Build();

                //     ITrigger trigger = TriggerBuilder.Create()
                //         .WithIdentity("trigger" + key, "group" + key)
                //         .StartNow()
                //         .WithCronSchedule(scenario.cronjob)
                //         .Build();

                //     await Scheduler.ScheduleJob(job, trigger, cancellationToken);
                // }

            }

            await Scheduler.Start(cancellationToken);
        }

        private static IJobDetail CreateJob()
        {
            return JobBuilder
                .Create(typeof(SmartPlugData_Schedule))
                .WithIdentity("powerDatas 1")
                .WithDescription("smartplug_schedule")
                .Build();
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Scheduler?.Shutdown(cancellationToken);
        }
    }
}