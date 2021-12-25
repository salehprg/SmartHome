using System;
using System.Collections.Generic;
using System.Device.Gpio;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using smarthome;
using smarthome.Helper;
using smarthome.Model;
using smarthome.Model.Modules;
using smarthome.Model.Modules.Devices;
using smarthome.Model.Modules.MainModule;
using smarthome.Model.SmartPlug;
using smarthome.Services;

namespace smarthome.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SmartplugController : ControllerBase
    {
        DbContextModel contextModel;
        SmartPlugService smartplugService;
        
        public SmartplugController(DbContextModel _context
                                    , SmartPlugService _smartPlugService)
        {
            contextModel = _context;
            smartplugService = _smartPlugService;
        }

        [HttpPost]
        public IActionResult GetPriodDataReport([FromBody] DataInputSmartPlug dataInput)
        {
            try
            {
                if(string.IsNullOrEmpty(dataInput.fromDate) && string.IsNullOrEmpty(dataInput.format))
                    return BadRequest("بازه زمانی مشخص نشده است");

                DateTime fromDate = DateTime.Now;
                DateTime toDate = DateTime.Now;

                List<SmartplugInfoModel> smartplugDatas = new List<SmartplugInfoModel>();
                
                string type = "";
                
                if(!string.IsNullOrEmpty(dataInput.format))
                {
                    type = TimeHelper.GetFormatType(dataInput.format);

                    fromDate = TimeHelper.DecodeAsDateTime(dataInput.format);
                }

                if(!string.IsNullOrEmpty(dataInput.fromDate))
                {
                    type = "M";

                    string[] datesFrom = dataInput.fromDate.Split("/");
                    fromDate = new DateTime(int.Parse(datesFrom[0]) , int.Parse(datesFrom[1]) , int.Parse(datesFrom[2]) , 0 , 0 ,0) ;

                    string[] datesTo = dataInput.toDate.Split("/");
                    toDate = new DateTime(int.Parse(datesTo[0]) , int.Parse(datesTo[1]) , int.Parse(datesTo[2]) , 0 , 0 ,0) ;

                }

                List<SmartPlugDataReportModel> outputs = smartplugService.DataReport(fromDate , toDate , (type == "H" ? true : false));
  
                return Ok(new {
                    outputs , 
                    totalPrice = outputs.Sum(x => x.price) ,
                    total = outputs.Sum(x => x.totalWatt)
                });
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetData()
        {
            try
            {
                return Ok(await smartplugService.CollectDataAsync(true));
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> GenerateRandomData(float hour , float day)
        {
            try
            {
                List<BaseDevice> smartPlugs = contextModel.devices.Include(x => x.moduleParent).Where(x => x.deviceType == DeviceType.SmartPlug).ToList();
                
                foreach (var smartplug in smartPlugs)
                {
                    SmartplugInfoModel info = new SmartplugInfoModel();
                    info.voltage = new Random().Next();
                    info.current = (float)new Random().NextDouble();
                    info.watt = info.voltage * info.current;
                    
                    info.time = DateTime.Now;
                    info.time = info.time.AddHours(-hour);
                    info.time = info.time.AddDays(-day);
                    info.baseDeviceId = smartplug.Id;

                    contextModel.SmartplugInfoModels.Add(info);
                }

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
