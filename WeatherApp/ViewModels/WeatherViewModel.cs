using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Extensions;
using WeatherApp.Models;
using WeatherApp.Services;

namespace WeatherApp.ViewModels
{
    public partial class WeatherViewModel : ViewModelBase
    {
        private readonly WeatherService _weatherService;
        private const string WeatherCacheKey = "SavedWeatherCities";

        [ObservableProperty]
        private string _city = string.Empty;

        public ObservableCollection<City> FoundCities { get; } = new();

        [ObservableProperty]
        private City? _selectedCity;

        public ObservableCollection<Weather> WeatherList { get; } = new();

        public WeatherViewModel(WeatherService weatherService)
        {
            _weatherService = weatherService;
            
            InitializationTask = Task.Run(async () =>
            {
                var savedCityNames = await Preferences.Load<List<string>>(WeatherCacheKey, new List<string>());

                foreach (var cityName in savedCityNames)
                {
                    var weather = await _weatherService.GetWeatherByCityNameAsync(cityName);
                    if (weather != null)
                    {
                        WeatherList.Add(weather);
                    }
                }
            });
        }

        /// <summary>
        /// Задача инициализации, необходима для 
        /// запуска асинхронной операции в конструкторе 
        /// </summary>
        TaskNotifier? _initializationTask;
        public Task? InitializationTask
        {
            get => _initializationTask;
            set => SetPropertyAndNotifyOnCompletion(ref _initializationTask, value);
        }

        [RelayCommand]
        async Task GetWeather() 
        {
            if (string.IsNullOrWhiteSpace(City)) return;

            var cities = await _weatherService.GetGeoByCityNameAsync(City, limit: 5);

            FoundCities.Clear();
            if (cities != null)
            {
                foreach (var city in cities)
                {
                    FoundCities.Add(city);
                }
            }
        }

        [RelayCommand]
        public async Task RemoveWeather(Weather weather)
        {
            if (weather != null && WeatherList.Contains(weather))
            {
                WeatherList.Remove(weather);
                var names = WeatherList.Select(w => w.City.Name).ToList();
                await Preferences.Save(WeatherCacheKey, names);
            }
        }
        [RelayCommand]
        async Task RefreshWeatherAsync(Weather oldWeather)
        {
            if (oldWeather == null) return;

            int index = WeatherList.IndexOf(oldWeather);
            if (index == -1) return;

            var updatedWeather = await _weatherService.GetWeatherByCityNameAsync(oldWeather.City.Name);

            if (updatedWeather != null)
            {
                WeatherList[index] = updatedWeather;
            }
        }


        partial void OnSelectedCityChanged(City? value)
        {
            if (value == null) return;

            _ = AddWeatherToList(value);
        }

        private async Task AddWeatherToList(City city)
        {
            var weather = await _weatherService.GetWeatherByCityNameAsync(city.Name);

            if (weather != null)
            {
                if (true/*!WeatherList.Any(w => w.City.Name == weather.City.Name)*/)
                {
                    WeatherList.Add(weather);
                    var cityNames = WeatherList.Select(w => w.City.Name).ToList();
                    await Preferences.Save(WeatherCacheKey, cityNames);
                }
            }

            SelectedCity = null;
        }
    }
}

