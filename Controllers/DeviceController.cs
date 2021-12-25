using System;
using System.Collections.Generic;
using System.Device.Gpio;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using smarthome;
using smarthome.Helper;
using smarthome.Model;
using smarthome.Model.Modules.Devices;
using smarthome.Model.Modules;
using smarthome.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using smarthome.Hubs;

namespace smarthome.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeviceController : ControllerBase
    {
        DbContextModel contextModel;
        DeviceService deviceService;
        
        public DeviceController(DbContextModel _context , DeviceService _deviceService)
        {
            contextModel = _context;
            deviceService = _deviceService;
        }

        [HttpGet]
        public IActionResult GetList(DeviceType deviceType)
        {
            try
            {
                return Ok(deviceService.GetAll().response.Where(x => x.deviceType == deviceType).ToList());
                
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Add([FromBody] BaseDevice baseDevice)
        {
            try
            {
                ServiceResponse<bool> serviceResponse = await deviceService.Add(baseDevice);
                bool result = serviceResponse.response;

                if(result)
                {
                    return Ok(baseDevice);
                }

                return BadRequest(serviceResponse.message);
                
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return BadRequest("در ثبت دستگاه مشکلی پیش آمد");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromBody] BaseDevice baseDevice)
        {
            try
            {
                ServiceResponse<bool> serviceResponse = await deviceService.Edit(baseDevice);
                bool result = serviceResponse.response;

                if(result)
                {
                    return Ok(baseDevice);
                }

                return BadRequest(serviceResponse.message);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return BadRequest("در ویرایش دستگاه مشکلی پیش آمد");
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                ServiceResponse<BaseDevice> serviceResponse = await deviceService.Remove(id);
                BaseDevice result = serviceResponse.response;

                if(result != null)
                {
                    return Ok(result);
                }

                return BadRequest(serviceResponse.message);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return BadRequest("در حذف دستگاه مشکلی پیش آمد");
            }
        }
    
        [HttpPatch]
        public async Task<IActionResult> SetDeviceStatus([FromBody] DeviceSetStatusInput inputModel)
        {
            try
            {
                BaseDevice device = contextModel.devices.Include(x => x.moduleParent).Where(x => x.Id == inputModel.Id).FirstOrDefault();

                if((await deviceService.setDeviceStatus(device , inputModel.status)))
                {
                    return Ok("Succusfuly changed");
                }

                return BadRequest("Cannot change status LED");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return BadRequest(ex.Message);
            }
        }
    }
}
