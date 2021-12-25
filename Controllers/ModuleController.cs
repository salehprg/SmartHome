
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
using smarthome.Config;
using Microsoft.EntityFrameworkCore;
using smarthome.Model.Modules.Devices;
using smarthome.Model.Modules.MainModule;
using smarthome.Services;
using Microsoft.AspNetCore.SignalR;
using smarthome.Hubs;

namespace smarthome.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ModuleController : ControllerBase
    {
        ModuleService moduleService;
        
        public ModuleController(ModuleService _moduleService)
        {
            moduleService = _moduleService;
        }

        [HttpGet]
        public IActionResult GetList()
        {
            try
            {
                return Ok(moduleService.GetAll().response);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return BadRequest("مشکلی در دریافت اطلاعات بوجود آمد");
            }
        }
        
        [HttpGet]
        [Route("autodetect")]
        public IActionResult AutoDetect()
        {
            try
            {
                Module result = moduleService.ReadNewModuleInfo();

                return Ok(result);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return BadRequest("مشکلی در دریافت اطلاعات بوجود آمد");
            }
        }

        [HttpGet]
        [Route("fixmodule")]
        public IActionResult FixModule()
        {
            try
            {
                Module result = moduleService.ReadNewModuleInfo();
                if(result != null)
                {
                    ServiceResponse<Module> response = moduleService.fixModule(result.serialNumber);

                    if(response.response != null)
                    {
                        return Ok(response.response);
                    }

                    return BadRequest(response.message);
                }

                return BadRequest("ماژول معیوبی یافت نشد");

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return BadRequest("مشکلی در دریافت اطلاعات بوجود آمد");
            }
        }

        [HttpPut]
        public async Task<IActionResult> Add([FromBody] Module module)
        {
            try
            {
                ServiceResponse<bool> serviceResponse = await moduleService.Add(module);

                bool result = serviceResponse.response;

                if(result)
                {
                    return Ok(module);
                }

                return BadRequest(serviceResponse.message);
                
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return BadRequest("مشکلی در ثبت اطلاعات بوجود آمد");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromBody] Module _module)
        {
            try
            {
                ServiceResponse<bool> serviceResponse = await moduleService.Edit(_module);
                bool result = serviceResponse.response;

                if(result)
                {
                    return Ok(_module);
                }

                return BadRequest(serviceResponse.message);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return BadRequest("مشکلی در ویرایش اطلاعات بوجود آمد");
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                ServiceResponse<Module> serviceResponse = await moduleService.Remove(id);
                Module module = serviceResponse.response;

                if(module != null)
                {
                    return Ok(module);
                }

                return BadRequest(serviceResponse.message);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return BadRequest("مشکلی در حذف اطلاعات بوجودآمد");
            }
        }
    
    }
}
