using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using smarthome.Config;
using smarthome.Helper;
using smarthome.Hubs;
using smarthome.Model;
using smarthome.Model.Modules.Devices;
using smarthome.Model.Modules.MainModule;
using smarthome.Interface;
using System.IO.Ports;
using Newtonsoft.Json;

namespace smarthome.Services
{
    public class ModuleService : IServiceCRUD<Module>
    {
        public SerialService serialService;
        public DbContextModel dbContextModel;

        ModbusService myModbus;
        SocketService socketService;

        public ModuleService(DbContextModel _context 
                                , SocketService _socketService
                                , SerialPort _port)
        {
            socketService = _socketService;
            
            dbContextModel = _context;
            serialService = new SerialService();
            myModbus = new ModbusService(_port);
        }

        public ServiceResponse<List<Module>> GetAll()
        {
            return new ServiceResponse<List<Module>>{
                response = dbContextModel.modules.ToList(),
                message = ""
            };
        }
        public async Task<ServiceResponse<bool>> Add(Module module)
        {
            Module temp = dbContextModel.modules.Where(x => x.serialNumber == module.serialNumber).FirstOrDefault();

            if(temp == null)
            {
                temp = dbContextModel.modules.OrderByDescending(x => x.slave_id).FirstOrDefault();

                Module checkIdentity = ReadNewModuleInfo();

                if(checkIdentity != null)
                {
                    if(checkIdentity.serialNumber == module.serialNumber)
                    {
                        int _slaveid = 0;

                        if(temp != null)
                        {
                            _slaveid = temp.slave_id;
                        }
                        else
                        {
                            _slaveid = 1;
                        }
                        
                        module.slave_id = _slaveid + 1;

                        module = serialService.decodeModule(module);

                        if(myModbus.writeRegister(config.default_slaveId , config.reg_function , module.slave_id))
                        {
                            dbContextModel.modules.Add(module);
                            await dbContextModel.SaveChangesAsync();

                            //await socketService.SendModuleInfo(module);

                            return new ServiceResponse<bool>{
                                message = "ماژول با موفقیت اضافه شد",
                                response = true
                            };
                        }
                        return new ServiceResponse<bool>{
                            message = "لطفا ماژول را تا انتهای نصب متصل نگه دارید",
                            response = false
                        };
                    }
                    else
                    {
                        return new ServiceResponse<bool>{
                            message = "سریال نامبر دستگاه تطابق ندارد",
                            response = false
                        };
                    }
                }

                return new ServiceResponse<bool>{
                    message = "دستگاه جدیدی متصل نیست",
                    response = false
                };
            }
            
            return new ServiceResponse<bool>{
                message = "دستگاهی با این شماره سریال موجود است",
                response = false
            };
        }

        public async Task<ServiceResponse<bool>> Edit(Module _module)
        {
            try
            {
                int id = _module.Id;
                Module module = dbContextModel.modules.Where(x => x.Id == id).FirstOrDefault();

                if(module != null)
                {
                    module.serialNumber = _module.serialNumber;
                    module.slave_id = _module.slave_id;
                    
                    module = serialService.decodeModule(module);

                    dbContextModel.modules.Update(module);
                    await dbContextModel.SaveChangesAsync();

                    //await socketService.SendModuleInfo(module);

                    return new ServiceResponse<bool>{
                        message = "ماژول با موفقیت ویرایش شد",
                        response = true
                    };
                }

                return new ServiceResponse<bool> {
                    response = false,
                    message = "ماژول مورد نظر وجود ندارد"
                };
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return new ServiceResponse<bool> {
                    response = false,
                    message = "مشکلی در ویرایش بوجود آمد"
                };
            }
        }

        public async Task<ServiceResponse<Module>> Remove(int moduleId)
        {
            Module module = dbContextModel.modules.Where(x => x.Id == moduleId).FirstOrDefault();

            if(module != null)
            {
                dbContextModel.modules.Remove(module);
                await dbContextModel.SaveChangesAsync();

                //await socketService.SendModuleInfo(module);

                return new ServiceResponse<Module>{
                    response = module,
                    message = ""
                };
            }

            return new ServiceResponse<Module>{
                response = null,
                message = "ماژول مورد نظر وجود ندارد"
            };
        }
    
        public Module ReadNewModuleInfo()
        {
            try
            {
                List<int> datas = myModbus.readRegisters(1 , config.reg_last_info);

                if(datas.Count > 0)
                {
                    string hex_macaddress = datas[config.reg_mac_1].ToString() 
                                        + datas[config.reg_mac_2].ToString();

                    if(hex_macaddress.Length < config.serial_length)
                    {
                        while(hex_macaddress.Length < config.serial_length)
                        {
                            hex_macaddress = "0" + hex_macaddress;
                        }
                    }

                    int type = datas[config.reg_type];
                    string type_str = "";

                    if(type < 10)
                        type_str = "0" + type.ToString();
                    else
                        type_str = type.ToString();

                    Module result = new Module();
                    result.moduleType = (ModuleType)type;
                    result.serialNumber = $"{type_str} {hex_macaddress} 0000" ;
                    result.slave_id = config.default_slaveId;

                    return result;
                }
                return null;
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.StackTrace);

                return null;
            }
        }
    
        public ServiceResponse<Module> fixModule(string serialNumber)
        {
            try
            {
                Module module = dbContextModel.modules.Where(x => x.serialNumber == serialNumber).FirstOrDefault();
                if(module != null)
                {
                    if(myModbus.writeRegister(config.default_slaveId , config.reg_function , module.slave_id))
                    {
                        return new ServiceResponse<Module>{
                            response = module,
                            message = "ماژول با موفقیت اصلاح شد"
                        };
                    }

                    return new ServiceResponse<Module>{
                        response = null,
                        message = "لطفا ماژول را به سیستم متصل نگه دارید"
                    };
                }

                return new ServiceResponse<Module>{
                    response = null,
                    message = "چنین ماژولی وجود ندارد"
                };
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                
                return new ServiceResponse<Module>{
                    response = null,
                    message = "مشکلی در اصلاح ماژول بوجود آمد"
                };
            }
        }
    }
}