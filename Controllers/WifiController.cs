using System.Net;
using System;
using System.Collections.Generic;
using System.Device.Gpio;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using QRCoder;
using smarthome;
using smarthome.Helper;
using smarthome.Model;
using smarthome.Model.ConnectionInfo;
using smarthome.Model.WifiHelper;

namespace smarthome.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WifiController : ControllerBase
    {
        WifiHelper wifiHelper;
        
        public WifiController()
        {
            wifiHelper = new WifiHelper();
        }

        [HttpGet]
        public IActionResult GetList()
        {
            try
            {
                return Ok(wifiHelper.GetWifiAPs());
                
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult ConnectToWifi([FromBody] WifiInput wifiInput)
        {
            try
            {
                string apname = wifiInput.apname;
                string password = wifiInput.password;

                Console.WriteLine($"ssid : {apname}");
                Console.WriteLine($"pass : {password}");
                
                return Ok(wifiHelper.ConnectToWifi(apname , password));
                
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return BadRequest(ex.Message);
            }
        }
    
        [HttpPatch]
        public IActionResult GetConnectInfo()
        {
            try
            {

                ConnectionInfo info = new ConnectionInfo();
                info = wifiHelper.MakeQrCode();

                return Ok(info);
                
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return BadRequest(ex.Message);
            }
        }
    }
}
