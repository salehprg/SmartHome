using System;
namespace smarthome.Helper
{
    public class TimeHelper
    {
        public static int GetLength(string format)
        {
            return int.Parse(format.Substring(0 , format.Length - 1));
        }
        public static string GetFormatType(string format)
        {
            return format[format.Length - 1].ToString().ToUpper();
        }
        public static DateTime DecodeAsDateTime(string format)
        {
            DateTime resultDate = DateTime.Now;

            int length = GetLength(format);
            string type = GetFormatType(format);

            switch(type)
            {
                case "H" :
                    resultDate = resultDate.AddHours(-1 * length);
                    break;

                case "D" :
                    resultDate = resultDate.AddDays(-1 * length);
                    break;

                case "M" :
                    length *= 30;
                    resultDate = resultDate.AddDays(-1 * length);
                    break;

                case "W" :
                    length *= 7;
                    resultDate = resultDate.AddDays(-1 * length);
                    break;
            }

            return resultDate;
        }
    }
}