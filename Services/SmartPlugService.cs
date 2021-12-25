using System.Linq;
using System;
using System.Collections.Generic;
using smarthome.Model.SmartPlug;
using smarthome.Model.Modules.Devices;
using smarthome.Model;
using smarthome.Model.Modules;
using smarthome.Model.Modules.MainModule;
using smarthome.Helper;
using System.IO.Ports;
using smarthome.Config;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace smarthome.Services
{
    public class SmartPlugService
    {
        DbContextModel contextModel;
        ModbusService modbusService;
        SocketService socketService;

        SmartplugHelper smartplugHelper;
        public SmartPlugService(DbContextModel _context , SerialPort serilaPort , SocketService _socketService)
        {
            modbusService = new ModbusService(serilaPort);
            contextModel = _context;
            socketService = _socketService;
            smartplugHelper = new SmartplugHelper();
        }

        public List<SmartPlugDataReportModel> DataReport(DateTime fromDate , DateTime toDate , bool hourlyReporyt = false)
        {
            List<SmartPlugDataReportModel> result = new List<SmartPlugDataReportModel>();

            var length = (int)(toDate - fromDate).TotalDays;

            var smartplugDatas = contextModel.SmartplugInfoModels.Where(x => x.time >= fromDate && x.time <= toDate).OrderBy(x => x.time).ToList();

            var smartPlugs = contextModel.devices.Where(x => x.deviceType == DeviceType.SmartPlug).ToList();

            foreach (var smartplug in smartPlugs)
            {
                List<SmartplugInfoModel> smartplugdata = smartplugDatas.Where(x => x.baseDeviceId == smartplug.Id).ToList();

                SmartPlugDataReportModel model = new SmartPlugDataReportModel();

                model.infoModels = new List<SmartplugInfoModel> ();
                model.baseDevice = smartplug;
                model.totalWatt = smartplugdata.Sum(x => x.watt);

                DateTime newTime = fromDate;

                for (int i = 0; i < length; i++)
                {
                    List<SmartplugInfoModel> infoModels = new List<SmartplugInfoModel>();

                    if(hourlyReporyt)
                    {
                        infoModels = smartplugdata.Where(x => x.time >= newTime && x.time < newTime.AddHours(1)).ToList();
                    }
                    else
                    {
                        infoModels = smartplugdata.Where(x => x.time >= newTime && x.time <= newTime.AddDays(1)).ToList();
                    }

                    SmartplugInfoModel infoModel = new SmartplugInfoModel();
                    
                    if(infoModels.Count <= 0)
                    {
                        infoModel.current = 0;
                        infoModel.voltage = 0;
                        infoModel.watt = 0;
                    }
                    else
                    {
                        infoModel.current = infoModels.Average(x => x.current);
                        infoModel.voltage = infoModels.Average(x => x.voltage);
                        infoModel.watt = infoModels.Sum(x => x.watt);
                    }                                   

                    infoModel.baseDevice = null;
                    infoModel.baseDeviceId = smartplug.Id;
                    infoModel.time = newTime; 

                    model.infoModels.Add(infoModel);
                
                    if(hourlyReporyt)
                    {
                        newTime = newTime.AddHours(1);
                    }
                    else
                    {
                        newTime = newTime.AddDays(1);
                    }
                }

                result.Add(model); 
            }

            smartplugHelper.CalculatePrice(result);

            return result;
        }
        public List<int> ReadModuleData(Module module)
        {
            try
            {
                return modbusService.readRegisters(module , 22);
            }
            catch {return null;}
        }

        public async Task<List<SmartplugInfoModel>> CollectDataAsync(bool resetSmartPlug = true)
        {
            List<Module> modules = contextModel.modules.Where(x => x.moduleType == ModuleType.Measurement).ToList();

            List<SmartplugInfoModel> result = new List<SmartplugInfoModel>();
            List<BaseDevice> smartPlugs = contextModel.devices.Include(x => x.moduleParent).Where(x => x.deviceType == DeviceType.SmartPlug).ToList();

            foreach (var module in modules)
            {
                List<int> data = ReadModuleData(module);

                foreach (var smartplug in smartPlugs.Where(x => x.moduleParentId == module.Id))
                {
                    SmartplugInfoModel info = getACInfo(smartplug , data);
                    info.time = DateTime.Now;
                    info.baseDeviceId = smartplug.Id;

                    result.Add(info);
                }

                if(resetSmartPlug)
                    ResetSmartPlugData(module);
            }

            contextModel.SmartplugInfoModels.AddRange(result);
            await contextModel.SaveChangesAsync();

            return result;
        }
        public async Task CheckAlert()
        {
            var datas = DataReport(DateTime.Now.AddMonths(-1) , DateTime.Now);
            var prices = datas.Sum(x => x.price);

            if(prices >= config.MaxPriceSmartPlug)
            {
                await socketService.SendAlertSmartPlug(prices);
            }
        }

        public void ResetSmartPlugData(Module module)
        {
            modbusService.writeRegister(module.slave_id , config.reg_reset_measure , 1);
        }
        public SmartplugInfoModel getACInfo(BaseDevice device , List<int> registeryData)
        {
            float voltage = smartplugHelper.getVoltage(device , registeryData);
            float current = smartplugHelper.getCurrent(device , registeryData);
            float watt = voltage * current;

            SmartplugInfoModel model = new SmartplugInfoModel();
            model.voltage = voltage;
            model.current = current;
            model.watt = watt;

            return model;
        }

    
    }
}