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
    public class HomeController : ControllerBase
    {
        DbContextModel contextModel;
        
        public HomeController(DbContextModel _context)
        {
            contextModel = _context;
        }

        [HttpGet]
        public IActionResult GetHomeInfo()
        {
            try
            {
                Console.WriteLine(Request.HttpContext.Connection.RemoteIpAddress);

                Home home = contextModel.Homes
                                        .Include(x => x.rooms)
                                        .ThenInclude(x => x.roomDevices)
                                        .Include(x => x.strokedAreas).FirstOrDefault();

                return Ok(home);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return BadRequest(ex.Message);
            }
        }
    }
}
