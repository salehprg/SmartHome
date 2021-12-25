
using System;
using System.Collections.Generic;
using System.Device.Gpio;
using System.IO.Ports;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using smarthome.Config;
using smarthome.Helper.Modbus;
using smarthome.Model;
using smarthome.Model.Modules;
using smarthome.Model.Modules.Devices;
using smarthome.Model.Modules.MainModule;

namespace smarthome.Services
{
    public class ModbusService
    {
        ModbusHelper modbusHelper;

        public ModbusService(SerialPort serialPort)
        {
            string port = "";

            if(RuntimeInformation.IsOSPlatform(OSPlatform.Linux))    
            {
                port = config.portLinux;
            }
            else if(RuntimeInformation.IsOSPlatform(OSPlatform.Windows))    
            {
                port = config.portWindows;
            }

            modbusHelper = new ModbusHelper(serialPort , port , config.txControllPin);
        }

        ///<summary>Set module status to its new state and update its status in database</summary>
        public bool setDeviceState(BaseDevice device , string newStatus)
        {
            bool result = false;

            int value = -1;

            device.status = newStatus;

            switch (device.deviceType)
            {
                case DeviceType.LED :
                    bool result_led = (device.Cast<DeviceLED>()).isOn;

                    if(result_led)
                        value = config.On;
                    else
                        value = config.Off;

                break;

                case DeviceType.Windoor :
                    bool result_win = (device.Cast<DeviceWindoors>()).isOpen;

                    if(result_win)
                        value = config.On;
                    else
                        value = config.Off;

                break;

                case DeviceType.Curtain :
                    int range = (device.Cast<DeviceCurtain>()).range;

                    if(range >= 50)
                        value = config.On;
                    else
                        value = config.Off;

                break;
            }

            result = writeRegister(device , value);

            Thread.Sleep(50);
            return result;
        }
        public bool writeRegister(BaseDevice device , int newValue)
        {
            try
            {
                Module module = device.moduleParent;
    
                return writeRegister(module.slave_id , (ushort)device.registerid + config.reg_last_info , newValue);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return false;
            }
        }
        public bool writeRegister(int slave_id , int reg , int newValue)
        {
            try
            {
                short[] data = new short[1];
                data[0] = (short)newValue;

                bool result = modbusHelper.WriteMulipleRegister(Convert.ToByte(slave_id) , (ushort)reg , 1 , data);

                Console.WriteLine(modbusHelper.modbusStatus);
            
                return result;
            }
            catch(Exception ex)
            {
                modbusHelper.Close();

                Console.WriteLine(ex.StackTrace);
                return false;
            }
        }

        public List<int> readRegisters(Module module , int registerCount)
        {
            try
            {
                List<int> result = new List<int>(registerCount);

                if(module.moduleType == ModuleType.Measurement)
                {
                    modbusHelper.read_timeout = 15000;
                    modbusHelper.write_timeout = 15000;
                }
                else
                {
                    modbusHelper.read_timeout = 1000;
                    modbusHelper.write_timeout = 1000;
                }

                return readRegisters(module.slave_id , registerCount);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return null;
            }
        }
        public List<int> readRegisters(BaseDevice device , int registerCount)
        {
            try
            {
                List<int> result = new List<int>(registerCount);
                Module module = device.moduleParent;

                if(device.moduleParent.moduleType == ModuleType.Measurement)
                {
                    modbusHelper.read_timeout = 15000;
                    modbusHelper.write_timeout = 15000;
                }
                else
                {
                    modbusHelper.read_timeout = 1000;
                    modbusHelper.write_timeout = 1000;
                }

                return readRegisters(module.slave_id , registerCount);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return null;
            }
        }

        public List<int> readRegisters(int slave_id , int registerCount)
        {
            try
            {
                List<int> result = new List<int>(registerCount);

                short[] datas = new short[registerCount];

                bool readResult = modbusHelper.ReadHoldingRegister(Convert.ToByte(slave_id) , 0 , (ushort)registerCount , ref datas);

                if(readResult)
                {
                    foreach(var data in datas)
                    {
                        result.Add(data);
                    }
                }

                return result;
            }
            catch(Exception ex)
            {
                modbusHelper.Close();

                Console.WriteLine(ex.StackTrace);
                return null;
            }
        }

    }
}