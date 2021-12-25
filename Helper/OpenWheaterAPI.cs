using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using smarthome.Model.OpenWeather;

namespace smarthome.Helper
{
    public class OpenWheaterAPI
    {
        private readonly string baseURL = "http://api.openweathermap.org/data/2.5/weather?";
        private readonly string baseOneCallURL = "http://api.openweathermap.org/data/2.5/onecall?";
        private readonly string APIKey = "9a70d2691b34a8dc2b89f3d6b400bd0d";
        public HttpClient client;

        public OpenWheaterAPI()
        {
            client = new HttpClient();
        }

        public async Task<Forecast> getForecast()
        {
            HttpResponseMessage message = await client.GetAsync(baseOneCallURL + "lat=36.314633&lon=59.574048&units=metric&exclude=minutely,hourly,current,alerts" + "&appid=" + APIKey);

            string messageStr = await message.Content.ReadAsStringAsync();

            Forecast response = JsonConvert.DeserializeObject<Forecast>(messageStr);
            return response;
        }

        public async Task<Response> getWheaterInfo(Request request)
        {
            if(string.IsNullOrEmpty(request.unit))
                request.unit = "metric";

            HttpResponseMessage message = await client.GetAsync(baseURL + request.getCityData() + "&units=" + request.unit + "&appid=" + APIKey);

            string messageStr = await message.Content.ReadAsStringAsync();

            Response response = JsonConvert.DeserializeObject<Response>(messageStr);
            return response;
        }
    }
}