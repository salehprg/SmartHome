using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using smarthome.Config;
using smarthome.Helper;
using smarthome.Hubs;
using smarthome.Interface;
using smarthome.Model;
using smarthome.Model.Modules;
using smarthome.Model.Modules.Devices;

namespace smarthome.Services
{
    public class DeviceService : IServiceCRUD<BaseDevice>
    {
        public SerialService serialService;
        public DbContextModel dbContextModel;

        SocketService socketService;
        ModbusService myModbus;

        public DeviceService(DbContextModel _context 
                            , SerialPort _port
                            , SocketService _socketService)
        {
            socketService = _socketService;

            dbContextModel = _context;
            myModbus = new ModbusService(_port);
            serialService = new SerialService();
        }

        public ServiceResponse<List<BaseDevice>> GetAll()
        {
            return new ServiceResponse<List<BaseDevice>>{
                response = dbContextModel.devices.ToList(),
                message = ""
            };
        }

        public async Task<ServiceResponse<bool>> Add(BaseDevice baseDevice)
        {
            try
            {
                BaseDevice temp = dbContextModel.devices.Where(x => x.serialNumber == baseDevice.serialNumber).FirstOrDefault();

                if(temp != null)
                    return new ServiceResponse<bool>{
                        message = "دستگاه با این سریال نامبر وجود دارد",
                        response = false
                    };
                 
                temp = dbContextModel.devices.Where(x => x.registerid == baseDevice.registerid).FirstOrDefault();

                if(temp != null)
                    return new ServiceResponse<bool>{
                        message = "دستگاه دیگری روی این پین قرار دارد",
                        response = false
                    };

                if(baseDevice.deviceType == 0)
                    baseDevice.deviceType = serialService.decoderDevice(baseDevice);

                if(baseDevice.deviceType == 0)
                    return new ServiceResponse<bool>{
                        message = "نوع دستگاه نامشخص است",
                        response = false
                    };

                dbContextModel.devices.Add(baseDevice);
                await dbContextModel.SaveChangesAsync();

                return new ServiceResponse<bool>{
                    message = "دستگاه با موفقیت اضافه شد",
                    response = true
                };
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);

                return new ServiceResponse<bool>{
                    message = "مشکلی در ایجاد دستگاه بوجود آمد",
                    response = false
                };
            }

        }

        public async Task<ServiceResponse<bool>> Edit(BaseDevice _device)
        {
            BaseDevice baseDevice = dbContextModel.devices.Where(x => x.Id == _device.Id).FirstOrDefault();

            if(baseDevice != null)
            {
                baseDevice.serialNumber =  (string.IsNullOrEmpty(_device.serialNumber) ? baseDevice.serialNumber : _device.serialNumber);
                baseDevice.name = (string.IsNullOrEmpty(_device.name) ? baseDevice.name : _device.name);;
                baseDevice.registerid = _device.registerid;
                baseDevice.moduleParentId = _device.moduleParentId;

                if(baseDevice.deviceType == 0)
                    baseDevice.deviceType = serialService.decoderDevice(baseDevice);
                    
                if(baseDevice.deviceType == 0)
                    return new ServiceResponse<bool>{
                        message = "نوع دستگاه مشخص نشده است",
                        response = false
                    };

                dbContextModel.devices.Update(baseDevice);
                await dbContextModel.SaveChangesAsync();


                return new ServiceResponse<bool>{
                    message = "دستگاه با موفقیت ویرایش شد",
                    response = true
                };
            }

            return new ServiceResponse<bool>{
                message = "چنین دستگاهی وجود ندارد",
                response = true
            };
        }

        public async Task<ServiceResponse<BaseDevice>> Remove(int moduleId)
        {
            BaseDevice baseDevice = dbContextModel.devices.Where(x => x.Id == moduleId).FirstOrDefault();

            if(baseDevice != null)
            {
                dbContextModel.devices.Remove(baseDevice);
                await dbContextModel.SaveChangesAsync();

                return new ServiceResponse<BaseDevice>{
                    message = "دستگاه با موفقیت حذف شد",
                    response = baseDevice
                };
            }

            return new ServiceResponse<BaseDevice>{
                message = "چنین دستگاهی وجود ندارد",
                response = null
            };
        }


        public async Task SendSocketData(BaseDevice baseDevice = null)
        {
            if(baseDevice != null)
                await socketService.SendDeviceInfo(baseDevice);
            else
            {
                Home home = dbContextModel.Homes
                                        .Include(x => x.rooms)
                                        .ThenInclude(x => x.roomDevices)
                                        .Include(x => x.strokedAreas).FirstOrDefault();

                await socketService.SendHomeData(home);
            }
        }

        public async Task<bool> setDeviceStatus(BaseDevice baseDevice , string newStatus , bool saveToDb = true)
        {
            bool result = myModbus.setDeviceState(baseDevice , newStatus);

            if(saveToDb && result)
            {
                baseDevice.status = newStatus;
                dbContextModel.devices.Update(baseDevice);
                await dbContextModel.SaveChangesAsync();
            }

            if(result)
            {
                await SendSocketData();
            }

            return result;
        }
    }
}