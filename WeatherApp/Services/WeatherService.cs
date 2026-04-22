using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WeatherApp.Extensions;
using WeatherApp.Models;

namespace WeatherApp.Services
{
    /// <summary>
    /// Сервис для получения информации о погоде
    /// </summary>
    public class WeatherService
    {
        /// <summary>
        /// Ключ для запросов к погодному API
        /// </summary>
        private readonly string _apiKey;
        private readonly HttpClient _httpClient;

        public WeatherService()
        {
            var key = App.Configuration.GetSection("ApiKeys")["WeatherApi"];
            if (string.IsNullOrEmpty(key))
            {
                throw new Exception("Weather API key not found in appsettings.json");
            }
            _apiKey = key;

            _httpClient = new HttpClient();
        }

        /// <summary>
        /// Получение погоды в городе по наименованию
        /// </summary>
        /// <param name="cityName">название города</param>
        /// <returns>информация о погоде в городе</returns>
        public async Task<Weather> GetWeatherByCityNameAsync(string cityName)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            options.Converters.Add(new WeatherJsonConverter());

            return await _httpClient.GetFromJsonAsync<Weather>($"http://api.openweathermap.org/data/2.5/weather?q={cityName}&appid={_apiKey}&lang=ru", options);
        }

        /// <summary>
        /// Получение погоды в городе по координатам
        /// </summary>
        /// <param name="latitude">Широта</param>
        /// <param name="longitude">Долгота</param>
        /// <returns>информация о погоде в городе</returns>
        public async Task<Weather> GetWeatherByGeoAsync(double latitude, double longitude)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            options.Converters.Add(new WeatherJsonConverter());
            
            return await _httpClient.GetFromJsonAsync<Weather>($"http://api.openweathermap.org/data/2.5/weather?lat={latitude}&lon={longitude}&appid={_apiKey}&lang=ru", options);
        }

        /// <summary>
        /// Получение городов по названию
        /// </summary>
        /// <param name="cityName">название города</param>
        /// <param name="limit">максимум городов, который может вернуть метод</param>
        /// <returns>список городов с координатами</returns>
        public async Task<List<City>> GetGeoByCityNameAsync(string cityName, int limit=1)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            options.Converters.Add(new CityJsonConverter());

            return await _httpClient.GetFromJsonAsync<List<City>>($"http://api.openweathermap.org/geo/1.0/direct?q={cityName}&limit={limit}&appid={_apiKey}&lang=ru", options);
        }
    }
}

