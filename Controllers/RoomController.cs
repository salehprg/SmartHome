using System;
using System.Collections.Generic;
using System.Device.Gpio;
using System.IO.Ports;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
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
    public class RoomController : ControllerBase
    {
        RoomService roomService;
           
        public RoomController(RoomService _roomService)
        {
            roomService = _roomService;
        }

        [HttpGet]
        public IActionResult GetRoomList()
        {
            try
            {
                return Ok(roomService.GetAll().response);
                
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Add([FromBody] Room _room)
        {
            try
            {
                ServiceResponse<bool> serviceResponse = await roomService.Add(_room);
                bool result = serviceResponse.response;

                if(result)
                {
                    return Ok(_room);
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
        public async Task<IActionResult> Edit([FromBody] Room _room)
        {
            try
            {
                ServiceResponse<bool> serviceResponse = await roomService.Edit(_room);
                bool result = serviceResponse.response;

                if(result)
                {
                    return Ok(_room);
                }

                return BadRequest(serviceResponse.message);
                
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return BadRequest("در ثبت دستگاه مشکلی پیش آمد");
            }
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveRoom(int id)
        {
            try
            {
                ServiceResponse<Room> serviceResponse = await roomService.Remove(id);
                Room result = serviceResponse.response;

                if(result != null)
                {
                    return Ok(result);
                }

                return BadRequest(serviceResponse.message);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return BadRequest("در حذف اتاق مشکلی پیش آمد");
            }
        }
    }
}
