using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using System.Device.Gpio;
using System.Runtime.InteropServices;
using System.Threading;
using smarthome.Config;

namespace smarthome.Helper.Modbus
{
    class ModbusHelper
    {
        private SerialPort serialPort = new SerialPort();
        private GpioController gpioController;
        public string modbusStatus;
        public int txControl = 18;

        public int read_timeout = 1000;
        public int write_timeout = 1000;

        private string port = "";


        #region Constructor / Deconstructor
        public ModbusHelper(SerialPort _serialPort , string UARTPort, int TxControl)
        {
            port = UARTPort;
            serialPort = _serialPort;
            
            txControl = TxControl;
            
            if(RuntimeInformation.IsOSPlatform(OSPlatform.Linux))    
            {
                gpioController = new GpioController();
            }
        }
        #endregion

        #region Open / Close Procedures
        private bool Open(int databits, Parity parity, StopBits stopBits)
        {
            string portName = port;
            int baudRate = config.baudrate;

            if (!serialPort.IsOpen)
            {

                serialPort.PortName = portName;
                serialPort.BaudRate = baudRate;
                serialPort.DataBits = databits;
                serialPort.Parity = parity;
                serialPort.StopBits = stopBits;
                serialPort.ReadTimeout = read_timeout;
                serialPort.WriteTimeout = write_timeout;

                try
                {
                    if(RuntimeInformation.IsOSPlatform(OSPlatform.Linux))    
                    {
                        gpioController.OpenPin(txControl , PinMode.Output);
                        gpioController.Write(txControl , PinValue.Low);
                    }
                    serialPort.Open();
                }
                catch (Exception err)
                {
                    modbusStatus = "Error opening " + portName + ": " + err.Message;
                    return false;
                }
                modbusStatus = portName + " opened successfully";
                return true;
            }
            else
            {
                modbusStatus = portName + " already opened";
                return true;
            }
        }
        public bool Close()
        {
            //Ensure port is opened before attempting to close:
            if (serialPort.IsOpen)
            {
                try
                {
                    serialPort.Close();
                    if(RuntimeInformation.IsOSPlatform(OSPlatform.Linux))    
                    {
                        gpioController.ClosePin(txControl);
                    }

                    modbusStatus = serialPort.PortName + " closed successfully";
                    return true;
                }
                catch (Exception err)
                {
                    modbusStatus = "Error closing " + serialPort.PortName + ": " + err.Message;
                    return false;
                }
            }
            else
            {
                modbusStatus = serialPort.PortName + " is not open";
                return false;
            }
        }
        #endregion

        #region CRC Computation
        private void GetCRC(byte[] message, ref byte[] CRC)
        {
            //Function expects a modbus message of any length as well as a 2 byte CRC array in which to 
            //return the CRC values:

            ushort CRCFull = 0xFFFF;
            byte CRCHigh = 0xFF, CRCLow = 0xFF;
            char CRCLSB;

            for (int i = 0; i < (message.Length) - 2; i++)
            {
                CRCFull = (ushort)(CRCFull ^ message[i]);

                for (int j = 0; j < 8; j++)
                {
                    CRCLSB = (char)(CRCFull & 0x0001);
                    CRCFull = (ushort)((CRCFull >> 1) & 0x7FFF);

                    if (CRCLSB == 1)
                        CRCFull = (ushort)(CRCFull ^ 0xA001);
                }
            }
            CRC[1] = CRCHigh = (byte)((CRCFull >> 8) & 0xFF);
            CRC[0] = CRCLow = (byte)(CRCFull & 0xFF);
        }
        #endregion

        #region Build Message
        private void BuildMessage(byte address, byte type, ushort start, ushort registers, ref byte[] message)
        {
            //Array to receive CRC bytes:
            byte[] CRC = new byte[2];

            message[0] = address;
            message[1] = type;
            message[2] = (byte)(start >> 8);
            message[3] = (byte)start;
            message[4] = (byte)(registers >> 8);
            message[5] = (byte)registers;

            GetCRC(message, ref CRC);
            message[message.Length - 2] = CRC[0];
            message[message.Length - 1] = CRC[1];
        }
        #endregion

        #region Check Response
        private bool CheckResponse(byte[] response)
        {
            //Perform a basic CRC check:
            byte[] CRC = new byte[2];
            GetCRC(response, ref CRC);
            if (CRC[0] == response[response.Length - 2] && CRC[1] == response[response.Length - 1])
                return true;
            else
                return false;
        }
        #endregion

        #region Get Response
        private void GetResponse(ref byte[] response)
        {
            try
            {
                Thread.Sleep(config.GPIOSet_timeout);
                for (int i = 0; i < response.Length; i++)
                {
                    byte temp = (byte)(serialPort.ReadByte());
                    response[i] = temp;
                    //Console.Write(temp);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Get respponse : " + ex.Message);
            }
        }

        private void write(byte[] message)
        {
            try
            {
                serialPort.DiscardOutBuffer();
                serialPort.DiscardInBuffer();

                if(RuntimeInformation.IsOSPlatform(OSPlatform.Linux))    
                {
                    if(!gpioController.IsPinOpen(txControl))
                        gpioController.OpenPin(txControl , PinMode.Output);

                    gpioController.Write(txControl , PinValue.High);  
                }
                Thread.Sleep(config.GPIOSet_timeout);

                serialPort.Write(message, 0, message.Length);

                Thread.Sleep(config.GPIOSet_timeout);
                if(RuntimeInformation.IsOSPlatform(OSPlatform.Linux))    
                {
                    if(!gpioController.IsPinOpen(txControl))
                        gpioController.OpenPin(txControl , PinMode.Output);

                    gpioController.Write(txControl , PinValue.Low);
                }
            }
            catch (Exception err)
            {
                if(RuntimeInformation.IsOSPlatform(OSPlatform.Linux))    
                {
                    if(!gpioController.IsPinOpen(txControl))
                        gpioController.OpenPin(txControl , PinMode.Output);

                    gpioController.Write(txControl , PinValue.Low);
                    Thread.Sleep(config.GPIOSet_timeout);
                }

                modbusStatus = "Error in write event: " + err.Message;
            }
        }

        #endregion

        #region Function 16 - Write Multiple Registers
        public bool WriteMulipleRegister(byte address, ushort start, ushort registers, short[] values)
        {
            //Ensure port is open:
            if (!serialPort.IsOpen)
            {
                Open(8 , Parity.None , StopBits.One);
            }
            
            if (serialPort.IsOpen)
            {
                //Message is 1 addr + 1 fcn + 2 start + 2 reg + 1 count + 2 * reg vals + 2 CRC
                byte[] message = new byte[9 + 2 * registers];
                //Function 16 response is fixed at 8 bytes
                byte[] response = new byte[8];

                //Add bytecount to message:
                message[6] = (byte)(registers * 2);
                //Put write values into message prior to sending:
                for (int i = 0; i < registers; i++)
                {
                    message[7 + 2 * i] = (byte)(values[i] >> 8);
                    message[8 + 2 * i] = (byte)(values[i]);
                }
                //Build outgoing message:
                BuildMessage(address, (byte)16, start, registers, ref message);
                
                //Send Modbus message to Serial Port:
                try
                {
                    write(message);
                    GetResponse(ref response);
                }
                catch (Exception err)
                {
                    modbusStatus = "Error in write event: " + err.Message;
                    return false;
                }

                Close();

                //Evaluate message:
                if (CheckResponse(response))
                {
                    modbusStatus = "Write successful";
                    return true;
                }
                else
                {
                    modbusStatus = "CRC error";
                    return false;
                }
            }
            else
            {
                modbusStatus = "Serial port not open";
                return false;
            }
        }
        #endregion

        #region Function 3 - Read Registers
        public bool ReadHoldingRegister(byte address, ushort start, ushort registers, ref short[] values)
        {
            if (!serialPort.IsOpen)
            {
                Open(8 , Parity.None , StopBits.One);
            }

            //Ensure port is open:
            if (serialPort.IsOpen)
            {
                //Function 3 request is always 8 bytes:
                byte[] message = new byte[8];
                //Function 3 response buffer:
                byte[] response = new byte[5 + 2 * registers];
                //Build outgoing modbus message:
                BuildMessage(address, (byte)3, start, registers, ref message);
                //Send modbus message to Serial Port:
                try
                {
                    write(message);
                    GetResponse(ref response);
                }
                catch (Exception err)
                {
                    modbusStatus = "Error in read event: " + err.Message;
                    return false;
                }

                Close();
                
                //Evaluate message:
                if (CheckResponse(response))
                {
                    //Return requested register values:
                    for (int i = 0; i < (response.Length - 5) / 2; i++)
                    {
                        values[i] = response[2 * i + 3];
                        values[i] <<= 8;
                        values[i] += response[2 * i + 4];
                    }
                    modbusStatus = "Read successful";
                    return true;
                }
                else
                {
                    modbusStatus = "CRC error";
                    return false;
                }
            }
            else
            {
                modbusStatus = "Serial port not open";
                return false;
            }

        }
        #endregion

    }
}
