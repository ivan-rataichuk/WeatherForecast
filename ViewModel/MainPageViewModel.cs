using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WeatherForecast.DTO;
using WeatherForecast.Helpers;
using WeatherForecast.Model;
using WeatherForecast.Model.Entities;

namespace WeatherForecast.ViewModel
{
    class MainPageViewModel : INotifyPropertyChanged
    {
        public WeatherDTO Weather { get; set; }
        public ObservableCollection<ForecastDTO> Forecast { get; set; }
        public ObservableCollection<City> SearchResults { get; set; }
        public ObservableCollection<City> FavoredCities { get; set; }
        public ObservableCollection<DayOfWeek> Days { get; set; }
        private ForecastService forecastService;
        private IEnumerable<City> cities;
        
        private string searchText;
        private City selectedResult;
        private IDictionary<DayOfWeek, List<ForecastDTO>> forecasts;

        public RelayCommand GetWeatherCommand { get; set; }
        public RelayCommand AddCityCommand { get; set; }
        public RelayCommand DeleteCityCommand { get; set; }
        public RelayCommand SelectForecastCommand { get; set; }

        public City SelectedResult
        {
            get
            {
                return this.selectedResult;
            }
            set
            {
                this.selectedResult = value;
                this.searchText = this.selectedResult.name;
                PropertyChanged(this, new PropertyChangedEventArgs("SearchText"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        

        public string SearchText
        {
            get
            {
                return this.searchText;
            }
            set
            {
                this.searchText = value;
                if (this.searchText.Length <= 1) return;

                SearchResults.Clear();
                foreach (City city in cities)
                {
                    if (city.name.ToUpperInvariant().StartsWith(this.searchText.ToUpperInvariant()))
                    {
                        SearchResults.Add(city);
                    }
                }
            }
        }

        public MainPageViewModel()
        {
            this.forecastService = new ForecastService();
            this.forecastService.OnWeatherReady += UpdateWeather;
            this.forecastService.OnForecastsReady += UpdateForecasts;
            this.cities = forecastService.GetCities();
            FavoredCities = new ObservableCollection<City>(forecastService.GetFavoriteCities());
            Forecast = new ObservableCollection<ForecastDTO>();

            GetWeatherCommand = new RelayCommand(GetWeather);
            AddCityCommand = new RelayCommand(AddCity);
            DeleteCityCommand = new RelayCommand(DeleteCity);
            SelectForecastCommand = new RelayCommand(SelectForecast);

            SearchResults = new ObservableCollection<City>();
            SearchText = "";
        }

        private void AddCity(object parameter)
        {
            if (SelectedResult == null)
                return;

            forecastService.AddFavoriteCity(SelectedResult);
            FavoredCities = new ObservableCollection<City>(forecastService.GetFavoriteCities());
            PropertyChanged(this, new PropertyChangedEventArgs("FavoredCities"));
        }

        private void DeleteCity(object parameter)
        {
            if (parameter == null)
                return;

            City selectedCity = parameter as City;
            forecastService.DeleteFavoriteCity(selectedCity);
            FavoredCities = new ObservableCollection<City>(forecastService.GetFavoriteCities());
            PropertyChanged(this, new PropertyChangedEventArgs("FavoredCities"));
        }

        private void SelectForecast(object parameter)
        {
            if (parameter == null)
                return;

            DayOfWeek day = (DayOfWeek)parameter;
            Forecast = new ObservableCollection<ForecastDTO>(forecasts[day]);
            PropertyChanged(this, new PropertyChangedEventArgs("Forecast"));
        }

        private void GetWeather(object parameter)
        {
            if (parameter == null)
                return;

            City selectedCity = parameter as City;
            forecastService.GetWeather(selectedCity._id);
        }

        private void UpdateWeather(WeatherDTO weather)
        {
            Weather = weather;
            PropertyChanged(this, new PropertyChangedEventArgs("Weather"));
        }

        private void UpdateForecasts(IDictionary<DayOfWeek, List<ForecastDTO>> forecasts)
        {
            this.forecasts = SortForecasts(forecasts);
            PropertyChanged(this, new PropertyChangedEventArgs("Forecasts"));
        }

        private IDictionary<DayOfWeek, List<ForecastDTO>> SortForecasts(IDictionary<DayOfWeek, List<ForecastDTO>> forecasts)
        {
            IDictionary<DayOfWeek, List<ForecastDTO>> sortedForecasts = new Dictionary<DayOfWeek, List<ForecastDTO>>();

            DayOfWeek currentDay = DateTime.Today.DayOfWeek;

            IList<DayOfWeek> days = new List<DayOfWeek>();
            days.Add(currentDay);

            for (int i = 1; i < 7; i++)
            {
                int k = ((int)currentDay + i);
                int d = (k >= 7) ? k % 7 : k;

                days.Add((DayOfWeek)d);
            }

            Days = new ObservableCollection<DayOfWeek>(days);
            PropertyChanged(this, new PropertyChangedEventArgs("Days"));

            IList<DayOfWeek> sortedDays = forecasts.OrderByDescending(kp => days.IndexOf(kp.Key)).Select(kp => kp.Key).ToList();

            foreach (DayOfWeek day in sortedDays)
            {
                List<ForecastDTO> sortedList = forecasts[day].OrderBy(f => f.DateOfCalculation.TimeOfDay).ToList();

                sortedForecasts.Add(day, sortedList);
            }

            Forecast = new ObservableCollection<ForecastDTO>(forecasts[days[0]]);
            PropertyChanged(this, new PropertyChangedEventArgs("Forecast"));

            return sortedForecasts;
        }


    }
}
