using System;
using System.Collections.Generic;
using System.Device.Gpio;
using System.IO.Ports;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using smarthome;
using smarthome.Model;

namespace smarthome.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        
        public AuthController()
        {
            
        }

        [HttpGet]
        public IActionResult CheckAuthDevice()
        {
            try
            {
                string requestIP = Request.HttpContext.Connection.RemoteIpAddress.ToString();

                Console.WriteLine(requestIP);
                if(requestIP == "127.0.0.1" || requestIP == "localhost" || requestIP == "smart.home")
                    return Ok(true);

                return Ok(false);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return BadRequest(ex.Message);
            }
        }
    }
}
