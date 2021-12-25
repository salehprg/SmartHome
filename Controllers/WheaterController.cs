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
using smarthome.Model.OpenWeather;

namespace smarthome.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WheaterController : ControllerBase
    {
        public OpenWheaterAPI openWheaterAPI;

        public WheaterController()
        {
            openWheaterAPI = new OpenWheaterAPI();
        }

        /// <returns>Wheater of City</returns>
        /// <response code="200">Returns the Wheater for this City</response>
        /// <response code="400">If the cityname is null</response>       
        [Route("city")]
        [HttpGet]
        public async Task<IActionResult> GetByCity(string cityName)
        {
            try
            {
                Request request = new Request();
                request.setCityData(cityName);
                request.unit = Unit.metric;

                Response response = await openWheaterAPI.getWheaterInfo(request);

                return Ok(response);
                
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return BadRequest(ex.Message);
            }
        }

        [Route("cityid")]
        [HttpGet]
        public async Task<IActionResult> GetByCityId(int cityid)
        {
            try
            {
                Request request = new Request();
                request.setCityData(cityid);
                request.unit = Unit.metric;

                Response response = await openWheaterAPI.getWheaterInfo(request);

                return Ok(response);
                
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return BadRequest(ex.Message);
            }
        }

        [Route("latlong")]
        [HttpGet]
        public async Task<IActionResult> GetByLatLong(int lat , int longg)
        {
            try
            {
                Request request = new Request();
                request.setCityData(lat , longg); 
                request.unit = Unit.metric;

                Response response = await openWheaterAPI.getWheaterInfo(request);

                return Ok(response);
                
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return BadRequest(ex.Message);
            }
        }
    
        [Route("forecast")]
        [HttpGet]
        public async Task<IActionResult> GetForecastWeather(int day)
        {
            try
            {
                Forecast response = await openWheaterAPI.getForecast();

                List<Daily> result = new List<Daily>();

                for (var i = 0; i < day; i++)
                {
                    DateTimeOffset offset = DateTimeOffset.FromUnixTimeSeconds(response.daily[i].dt);
                    
                    response.daily[i].time = offset.DateTime;

                    result.Add(response.daily[i]);
                }

                return Ok(result);
                
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return BadRequest(ex.Message);
            }
        }

    }
}
