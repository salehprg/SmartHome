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
using System.Runtime.InteropServices;

namespace smarthome.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UpdateController : ControllerBase
    {
        DbContextModel contextModel;
        
        public UpdateController(DbContextModel _context)
        {
            contextModel = _context;
        }

        [HttpGet]
        public IActionResult GetList(string cmd)
        {
            try
            {
                if(RuntimeInformation.IsOSPlatform(OSPlatform.Linux))    
                {
                    //string result = cmd.Run();
                    return Ok(true);
                }

                return BadRequest("Run this just in linux");
                
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return BadRequest(ex.Message);
            }
        }
    }
}
