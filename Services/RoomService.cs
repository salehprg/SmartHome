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
    public class RoomService : IServiceCRUD<Room>
    {
        public SerialService serialService;
        public DbContextModel dbContextModel;

        SocketService socketService;
        ModbusService myModbus;

        public RoomService(DbContextModel _context 
                            , SerialPort _port
                            , SocketService _socketService)
        {
            socketService = _socketService;

            dbContextModel = _context;
            myModbus = new ModbusService(_port);
            serialService = new SerialService();
        }

        public ServiceResponse<List<Room>> GetAll()
        {
            return new ServiceResponse<List<Room>>{
                response = dbContextModel.rooms.Include(x => x.roomDevices).ToList(),
                message = ""
            };
        }

        public async Task<ServiceResponse<bool>> Add(Room room)
        {
            try
            {
                Room temp = dbContextModel.rooms.Where(x => x.roomName == room.roomName).FirstOrDefault();

                if(temp == null)
                {

                    dbContextModel.rooms.Add(room);
                    await dbContextModel.SaveChangesAsync();

                    return new ServiceResponse<bool>{
                        message = "اتاق با موفقیت اضافه شد",
                        response = true
                    };
                }
                
                return new ServiceResponse<bool>{
                    message = "نام اتاق تکراری میباشد",
                    response = false
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);

                return new ServiceResponse<bool>{
                    message = "مشکلی در ایجاد اتاق بوجود آمد",
                    response = false
                };
            }

        }

        public async Task<ServiceResponse<bool>> Edit(Room _room)
        {
            try
            {
                Room room = _room;

                if(room.RoomId != 0)
                {
                    Room temp = dbContextModel.rooms.Where(x => x.RoomId == room.RoomId).FirstOrDefault();

                    if(temp != null)
                    {
                        temp.name = room.name;
                        temp.area = room.area;
                        temp.border = room.border;
                        temp.X = room.X;
                        temp.Y = room.Y;

                        dbContextModel.rooms.Update(temp);
                        await dbContextModel.SaveChangesAsync();

                        return new ServiceResponse<bool>{
                            message = "اتاق با موفقیت ویرایش شد",
                            response = true
                        };
                    }

                    return new ServiceResponse<bool>{
                        message = "چنین اتاقی وجود ندارد",
                        response = false
                    };
                }

                return new ServiceResponse<bool>{
                    message = "اتاق به درستی مشخص نشده است",
                    response = false
                };
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return new ServiceResponse<bool>{
                    message = "مشکلی در ویرایش اتاق بوجود آمد",
                    response = false
                };
            }
        }

        public async Task<ServiceResponse<Room>> Remove(int id)
        {
            try
            {
                if(id != 0)
                {
                    Room temp = dbContextModel.rooms.Where(x => x.RoomId == id).FirstOrDefault();

                    dbContextModel.rooms.Remove(temp);
                    await dbContextModel.SaveChangesAsync();

                    return new ServiceResponse<Room>{
                        message = "اتاق با موفقیت حذف شد",
                        response = temp
                    };
                }

                return new ServiceResponse<Room>{
                    message = "اتاق به درستی مشخص نشده است",
                    response = null
                };
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return new ServiceResponse<Room>{
                    message = "مشکلی در حذف اتاق بوجود آمد",
                    response = null
                };
            }
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