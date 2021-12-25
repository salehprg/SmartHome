using System.Text.Json.Serialization;

namespace smarthome.Model.OpenWeather
{
    public static class Unit
    {
        public static readonly string metric = "metric";
        public static readonly string imperial = "imperial";

    }
    public class Request
    {
        private string cityData {get;set;}
        public string unit {get;set;}

        public void setCityData(string cityName) => cityData = "q=" + cityName;
        public void setCityData(int cityId) => cityData = "id=" + cityId;
        public void setCityData(int lat , int lang) => cityData = string.Format("lat={0}&lon={1}" , lat , lang);

        public string getCityData() => cityData;

    }
}