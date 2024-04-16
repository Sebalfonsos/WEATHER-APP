using WEATHER_APP.Model;

namespace WEATHER_APP.Services
{
    public interface IWeatherService
    {

        public Task<List<WeatherResponse.Rootobject>> Get(String location);
    }
}
