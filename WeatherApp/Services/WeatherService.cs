using System;
using System.Collections.Generic;
using System.Linq;
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

        public WeatherService()
        {
            var key = App.Configuration.GetSection("ApiKeys")["WeatherApi"];
            if (string.IsNullOrEmpty(key))
            {
                throw new Exception("Weather API key not found in appsettings.json");
            }
            _apiKey = key;
        }

        /// <summary>
        /// Получение погоды в городе по наименованию
        /// </summary>
        /// <param name="cityName">название города</param>
        /// <returns>информация о погоде в городе</returns>
        public async Task<Weather> GetWeatherByCityNameAsync(string cityName)
        {
            // Опция, необходимая для корректной десериализации данных JSON из openweathermap
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            options.Converters.Add(new WeatherJsonConverter());

            // Вызовите API погоды, используя HttpClient. Подробнее: https://openweathermap.org/current#name

            // Десериализуйте ответ в объект Weather, используйте в методе опцию, указанную выше

            // Верните объект Weather

            throw new NotImplementedException(); //Уберите после реализации кода
        }

        /// <summary>
        /// Получение погоды в городе по координатам
        /// </summary>
        /// <param name="latitude">Широта</param>
        /// <param name="longitude">Долгота</param>
        /// <returns>информация о погоде в городе</returns>
        public async Task<Weather> GetWeatherByGeoAsync(double latitude, double longitude)
        {
            // Опция, необходимая для корректной десериализации данных JSON из openweathermap
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            options.Converters.Add(new WeatherJsonConverter());

            // Вызовите API погоды, используя HttpClient. Подробнее: https://openweathermap.org/current

            // Десериализуйте ответ в объект Weather, используйте в методе опцию, указанную выше

            // Верните объект Weather

            throw new NotImplementedException(); //Уберите после реализации кода
        }

        /// <summary>
        /// Получение городов по названию
        /// </summary>
        /// <param name="cityName">название города</param>
        /// <param name="limit">максимум городов, который может вернуть метод</param>
        /// <returns>список городов с координатами</returns>
        public async Task<List<City>> GetGeoByCityNameAsync(string cityName, int limit=1)
        {
            // Опция, необходимая для корректной десериализации данных JSON из openweathermap 
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            options.Converters.Add(new CityJsonConverter());

            // Вызовите географическое API, используя HttpClient. Подробнее: https://openweathermap.org/api/geocoding-api 

            // Десериализуйте ответ в список объектов City, используйте в методе опцию, указанную выше

            // Верните список City

            throw new NotImplementedException(); //Уберите после реализации кода
        }
    }
}

