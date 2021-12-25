using System;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using smarthome.Model;

namespace smarthome.Schedules
{
    public class JobCreator
    {
        StdSchedulerFactory factory;
        IScheduler scheduler;
        DbContextModel contextModel;
        IJobFactory _jobFactory;

        public JobCreator (DbContextModel _context , IJobFactory jobFactory)
        {
            contextModel = _context;
            _jobFactory = jobFactory;
        }
        public async Task<bool> StartNewJob(Scenario scenario)
        {
            try
            {
                factory = new StdSchedulerFactory();
                scheduler = await factory.GetScheduler();
                scheduler.JobFactory = _jobFactory;
                
                int scenarioId = scenario.ScenarioId;
                string cron = scenario.cronjob;

                IJobDetail job = JobBuilder.Create<JobHandler>()
                    .WithIdentity(scenarioId.ToString(), "group" + scenarioId.ToString())
                    .Build();

                ITrigger trigger = TriggerBuilder.Create()
                    .WithIdentity("trigger" + scenarioId.ToString(), "group" + scenarioId.ToString())
                    .StartNow()
                    .WithCronSchedule(cron)
                    .Build();


                await scheduler.ScheduleJob(job, trigger);

                await scheduler.Start();

                return true;
            }   
            catch(Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return false;
            }
        }
    }
}