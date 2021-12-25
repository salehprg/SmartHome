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
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using smarthome;
using smarthome.Helper;
using smarthome.Model;
using smarthome.Model.Camera;

namespace smarthome.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CameraController : ControllerBase
    {
        DbContextModel contextModel;
        CameraHelper cameraHelper;
        
        public CameraController(DbContextModel _context)
        {
            contextModel = _context;
            cameraHelper = new CameraHelper();
        }

        [HttpGet]
        public IActionResult GetList()
        {
            try
            {
                List<CameraModel> cameras = contextModel.CameraModels.ToList();

                return Ok(cameras);
                
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch]
        public async Task<IActionResult> GetAutomaticCameraList()
        {
            try
            {
                List<string> ips = cameraHelper.FindAllCameraStreamLink();

                List<CameraModel> result = new List<CameraModel>();

                foreach (var ip in ips)
                {
                    CameraModel cameraModel = contextModel.CameraModels.Where(x => x.ip == ip).FirstOrDefault();
                    if(cameraModel == null)
                    {
                        cameraModel = new CameraModel();
                        cameraModel.ip = ip;
                        cameraModel.cameraName = ip;

                        contextModel.CameraModels.Add(cameraModel);
                        await contextModel.SaveChangesAsync();

                        result.Add(cameraModel);
                    }
                    else
                    {
                        result.Add(cameraModel);
                    }
                }

                return Ok(result);
                
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return BadRequest(ex.Message);
            }
        }
    
        [HttpPut]
        public async Task<IActionResult> AddCamera(CameraModel cameraModel)
        {
            try
            {
                if(cameraModel != null)
                {
                    contextModel.CameraModels.Add(cameraModel);
                    await contextModel.SaveChangesAsync();

                    return Ok(cameraModel);
                }

                return BadRequest("Camera model is NULL");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);

                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditCamera(CameraModel _cameraModel)
        {
            try
            {   
                CameraModel cameraModel = contextModel.CameraModels.Where(x => x.Id == _cameraModel.Id).FirstOrDefault();

                if(cameraModel != null)
                {
                    cameraModel.cameraName = _cameraModel.cameraName;
                    cameraModel.ip = _cameraModel.ip;

                    contextModel.CameraModels.Update(cameraModel);
                    await contextModel.SaveChangesAsync();

                    return Ok(cameraModel);
                }

                return BadRequest("Cannot find camera ID");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);

                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveCamera(int camId)
        {
            try
            {   
                CameraModel cameraModel = contextModel.CameraModels.Where(x => x.Id == camId).FirstOrDefault();

                if(cameraModel != null)
                {
                    contextModel.CameraModels.Remove(cameraModel);
                    await contextModel.SaveChangesAsync();

                    return Ok(cameraModel);
                }

                return BadRequest("Cannot find camera ID");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);

                return BadRequest(ex.Message);
            }
        }
    }
}
