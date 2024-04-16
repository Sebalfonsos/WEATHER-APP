using WEATHER_APP.Model;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Net;

namespace WEATHER_APP.Services
{
    public class WeatherService : IWeatherService
    {

        //private string url = "https://api.openweathermap.org/data/2.5/weather?appid=b82c7e8a1c61615bbe9ab70b3ef52f8e&lang=es&q=";
        private string serviceURL = "https://api.openweathermap.org/data/2.5/weather";
        private string appKey = "b82c7e8a1c61615bbe9ab70b3ef52f8e";
        private string lang = "en";
        
        public async Task<List<WeatherResponse.Rootobject>> Get(string location)
        {
            string url = $"{serviceURL}?q={location}&appid={appKey}&lang={lang}";
            var client = new HttpClient();
            var response = await client.GetAsync(url);

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                // La ciudad no fue encontrada, puedes manejar esto de alguna manera
                // Por ejemplo, lanzar una excepción o devolver un valor predeterminado
                // Aquí devolvemos una lista vacía
                return new List<WeatherResponse.Rootobject>();
            }

            var responseBody = await response.Content.ReadAsStringAsync();
            var weatherResponse = JsonSerializer.Deserialize<WeatherResponse.Rootobject>(responseBody);

            return new List<WeatherResponse.Rootobject> { weatherResponse };
        }
    }
}
