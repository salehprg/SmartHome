using System;
using System.Collections.Generic;
using System.Device.Gpio;
using System.IO.Ports;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using smarthome;
using smarthome.Model;
using smarthome.Helper;
using Quartz.Impl;
using Quartz;
using smarthome.Schedules;
using Quartz.Spi;
using smarthome.Config;
using Microsoft.EntityFrameworkCore;
using smarthome.Model.Modules.Devices;
using smarthome.Model.Modules;
using Microsoft.AspNetCore.SignalR;
using smarthome.Hubs;
using smarthome.Services;

namespace smarthome.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ScheduleController : ControllerBase
    {
        ScheduleHelper scheduleHelper;
        DbContextModel contextModel;
        IJobFactory _jobfactory;
        
        public ScheduleController(DbContextModel _context , IJobFactory jobFactory , DeviceService deviceService)
        {
            scheduleHelper = new ScheduleHelper(_context , deviceService);
            contextModel = _context;
            _jobfactory = jobFactory;
        }

        [HttpGet]
        public IActionResult GetScheules()
        {
            try
            {
                List<Scenario> scenarios = contextModel.Scenarios.Include(x => x.actions).ToList();

                return Ok(scenarios);

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return BadRequest(ex.Message);
            }
        }


        [HttpPatch]
        public async Task<IActionResult> EnableSchedule([FromBody] Scenario _scenario)
        {
            try
            {
                Scenario scenario = contextModel.Scenarios.Where(x => x.ScenarioId == _scenario.ScenarioId).FirstOrDefault();

                if(scenario != null)
                {
                    await scheduleHelper.startSchedule(scenario.ScenarioId);

                    Home home = contextModel.Homes
                                        .Include(x => x.rooms)
                                        .ThenInclude(x => x.roomDevices)
                                        .Include(x => x.strokedAreas) .FirstOrDefault();

                    return Ok(home);

                }

                return BadRequest("Cannot find scenario ID");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> AddSchedule([FromBody] Scenario scenario)
        {
            try
            {
                scenario.ScenarioId = 0;
                scenario.running = false;

                await contextModel.Scenarios.AddAsync(scenario);
                await contextModel.SaveChangesAsync();

                foreach (var action in scenario.actions)
                {
                    action.ScenarioId = scenario.ScenarioId;

                    BaseDevice baseDevice = contextModel.devices.Where(x => x.Id == action.deviceId).FirstOrDefault();
                    if(baseDevice.deviceType == DeviceType.LED || baseDevice.deviceType == DeviceType.Windoor)
                    {
                        bool result = bool.Parse(action.status);

                        if(result)
                            action.status = config.On.ToString();
                        else
                            action.status = config.Off.ToString();
                    }
                }

                contextModel.ScenarioActions.UpdateRange(scenario.actions);
                await contextModel.SaveChangesAsync();

                return Ok(scenario);
                
            }
            catch(Exception ex)
            {
                contextModel.Scenarios.Remove(scenario);
                await contextModel.SaveChangesAsync();

                Console.WriteLine(ex.StackTrace);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditSchedule([FromBody] ScenariosInput _scenario)
        {
            try
            {
                contextModel.ChangeTracker.Clear();

                Scenario scenario = contextModel.Scenarios.Where(x => x.ScenarioId == _scenario.ScenarioId).FirstOrDefault();

                scenario.actions = null;
                scenario.ScenarioName = _scenario.ScenarioName;
                scenario.cronjob = _scenario.cronjob;

                _scenario.actions.ForEach(x => x.ScenarioId = scenario.ScenarioId);

                List<ScenarioAction> garbage = new List<ScenarioAction>();
                garbage = contextModel.ScenarioActions.Where(x => x.ScenarioId == scenario.ScenarioId).ToList();

                foreach (var action in _scenario.actions)
                {
                    if(action.Id == 0)
                    {
                        contextModel.ScenarioActions.Add(action);
                    }
                    else
                    {
                        garbage.Remove(garbage.Where(x => x.Id == action.Id).FirstOrDefault()); 
                        ScenarioAction scenarioAction = contextModel.ScenarioActions.Where(x => x.Id == action.Id).FirstOrDefault();

                        scenarioAction.status = action.status;
                        scenarioAction.deviceId = action.deviceId;
                        scenarioAction.delay = action.delay;
                        
                        contextModel.ScenarioActions.Update(scenarioAction);
                    }
                }

                
                contextModel.ScenarioActions.RemoveRange(garbage);
                contextModel.Scenarios.Update(scenario);

                await contextModel.SaveChangesAsync();

                return Ok(_scenario);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveSchedule(int id)
        {
            try
            {
                Scenario scenario = contextModel.Scenarios.Where(x => x.ScenarioId == id).FirstOrDefault();

                contextModel.Scenarios.Remove(scenario);
                await contextModel.SaveChangesAsync();
                
                return Ok(true);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return BadRequest(ex.Message);
            }
        }
    }
}
