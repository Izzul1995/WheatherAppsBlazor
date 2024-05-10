

using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WeatherApp.Data
{
    public class WeatherService
    {
        private readonly HttpClient _httpClient;

        public WeatherService(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<WeatherModel> GetWeatherAsync(double latitude, double longitude)
        {
            try
            {
                string apiUrl = $"https://api.open-meteo.com/v1/forecast?latitude={latitude}&longitude={longitude}&current=temperature_2m,precipitation,wind_speed_10m&hourly=temperature_2m,precipitation,wind_speed_10m&forecast_days=1";
                var response = await _httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var weatherData = JsonConvert.DeserializeObject<OpenMeteoResponse>(content);

                    // Parse the weather data and return a WeatherModel object
                    return new WeatherModel
                    {
                        Temperature = weatherData.current.temperature_2m,
                        Precipitation = weatherData.current.precipitation,
                        WindSpeed = weatherData.current.wind_speed_10m
                    };
                }
                else
                {
                    // Handle unsuccessful API response
                    return null;
                }
            }
            catch (Exception ex)
            {
                // Handle exception
                return null;
            }
        }
    }

    // Define models to deserialize API response
    public class OpenMeteoResponse
    {
        public CurrentWeather current { get; set; }
    }

    public class CurrentWeather
    {
        public float temperature_2m { get; set; }
        public float precipitation { get; set; }
        public float wind_speed_10m { get; set; }
    }
}

