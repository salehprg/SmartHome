using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using Microsoft.AspNetCore.SignalR;
using smarthome.Config;
using smarthome.Helper.Modbus;
using smarthome.Hubs;
using smarthome.Model;
using smarthome.Model.Modules.Devices;
using smarthome.Model.Modules.MainModule;
using smarthome.Model.SmartPlug;
using smarthome.Services;

namespace smarthome.Helper
{
    public class SmartplugHelper
    {
        int Measure_Accuracy = 100;

        
        float[] step_prices = {400 , 477 , 1023 , 1841 , 2114 , 2660 , 2933};

        
        public SmartplugHelper(){}


        public float CalculatePrice(List<SmartPlugDataReportModel> datas)
        {
            float totalPrice = 0;

            if (datas is null)
            {
                return 0;
            }

            foreach (var data in datas)
            {   
                List<SmartplugInfoModel> sortedDate = data.infoModels.OrderBy(x => x.time).ToList();
                
                if(sortedDate.Count > 0)
                {
                    DateTime startTime = sortedDate.FirstOrDefault().time;
                    DateTime endTime = sortedDate.LastOrDefault().time;

                    int days = (int)(endTime - startTime).TotalDays;

                    float avg = (data.totalWatt / days);

                    data.price = calculateStepPrice(avg);

                    totalPrice += data.price;
                }
            }
            
            return totalPrice;
        }

        public float calculateStepPrice(float avg)
        {
            float result = 0;
            float remain = avg;

            List<float> prices = new List<float>();

            for(int i = 0; i < 6; i++)
            {
                if(remain > 0)
                {
                    float avgWatt = 0;
                    if(remain >= 100)
                    {
                        avgWatt = 100;
                    }
                    else
                    {
                        avgWatt = remain;
                    }

                    remain -= avgWatt;

                    result += avgWatt * step_prices[i];
                }
            }

            return result;

        }
    
        public float getCurrent(BaseDevice device , List<int> registeryData)
        {
            try
            {
                if(registeryData.Count > 0)
                    return registeryData[device.registerid + config.reg_last_info] / (Measure_Accuracy * (registeryData[config.reg_last_info] / config.data_interval));

                return 0;
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return 0;
            }
        }

        public float getVoltage(BaseDevice device , List<int> registeryData)
        {
            try
            {
                if(registeryData.Count > 0)
                    return registeryData[config.reg_volt_measure + config.reg_last_info] / Measure_Accuracy;

                return 0;
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return 0;
            }
        }
    
    
    }
}