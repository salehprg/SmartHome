using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace smarthome.Model.OpenWeather
{
    public class Temp
    {
        public double day { get; set; }
        public double min { get; set; }
        public double max { get; set; }
        public double night { get; set; }
        public double eve { get; set; }
        public double morn { get; set; }
    }

    public class FeelsLike
    {
        public double day { get; set; }
        public double night { get; set; }
        public double eve { get; set; }
        public double morn { get; set; }
    }

    public class Daily
    {
        [JsonIgnore]
        public DateTime time { get; set; }
        public int dt { get; set; }
        // public int sunrise { get; set; }
        // public int sunset { get; set; }
        // public int moonrise { get; set; }
        // public int moonset { get; set; }
        public double moon_phase { get; set; }
        public Temp temp { get; set; }
        public FeelsLike feels_like { get; set; }
        public int pressure { get; set; }
        public int humidity { get; set; }
        public double dew_point { get; set; }
        public double wind_speed { get; set; }
        public int wind_deg { get; set; }
        public double wind_gust { get; set; }
        public List<Weather> weather { get; set; }
        public float clouds { get; set; }
        public float pop { get; set; }
        public double uvi { get; set; }
    }

    public class Forecast
    {
        // public double lat { get; set; }
        // public double lon { get; set; }
        // public string timezone { get; set; }
        // public int timezone_offset { get; set; }
        public List<Daily> daily { get; set; }
    }
}