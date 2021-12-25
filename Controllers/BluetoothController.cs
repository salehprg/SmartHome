using System;
using System.Collections.Generic;
using System.Device.Gpio;
using System.IO.Ports;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using smarthome;
using smarthome.Config;
using smarthome.Hubs;
using smarthome.Model;
using smarthome.Services;

namespace smarthome.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BluetoothController : ControllerBase
    {
        BluetoothService bluetoothService;
        public BluetoothController(BluetoothService _bluetoothService)
        {
            bluetoothService = _bluetoothService;
        }

        [HttpGet]
        public IActionResult AllowPair()
        {
            try
            {
                config.AllowPair = true;

                return Ok(true);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("Media")]
        public async Task<IActionResult> GetMediaInfo()
        {
            try
            {
                return Ok(await bluetoothService.GetPlayerInfo());
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return BadRequest(ex.Message);
            }
        }
    
        [HttpGet]
        [Route("MediaPlayer")]
        public async Task<IActionResult> MediaPlayerAction(string action)
        {
            try
            {
                switch(action.ToLower())
                {
                    case "play" :
                        return Ok(await bluetoothService.Play());

                    case "pause" :
                        return Ok(await bluetoothService.Pause());

                    case "next" :
                        return Ok(await bluetoothService.NextTrack());

                    case "previous" :
                        return Ok(await bluetoothService.PreviousTrack());

                    case "volumeup" :
                        return Ok(await bluetoothService.VolumeUp());

                    case "volumedown" :
                        return Ok(await bluetoothService.VolumeDown());

                    default :
                        return Ok(false);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return BadRequest(ex.Message);
            }
        }
    }
}
