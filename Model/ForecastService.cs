using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using WeatherForecast.DTO;
using WeatherForecast.Helpers;
using WeatherForecast.Model.Entities;
using WeatherForecast.Model.Entities.Weather;

namespace WeatherForecast.Model
{
    public class ForecastService
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ForecastService));

        private IList<City> Cities;
        private CurrentWeather currentWeather;
        private ForecastList forecastList;
        private Mapper mapper;

        public event Action<WeatherDTO> OnWeatherReady;
        public event Action<IDictionary<DayOfWeek, List<ForecastDTO>>> OnForecastsReady;

        public ForecastService()
        {
            mapper = new Mapper();
        }

        public IEnumerable<City> GetCities()
        {
            if (Cities != null && Cities.Count != 0)
            {
                return Cities;
            }

            string json = string.Empty;
            Cities = new List<City>();

            try
            {
                FileInfo fileInf = new FileInfo(Strings.CitiesListLocation);
                if (fileInf.Exists)
                {
                    using (StreamReader sr = fileInf.OpenText())
                    {
                        while (!sr.EndOfStream)
                        {
                            json = sr.ReadLine();
                            City city = JsonConvert.DeserializeObject<City>(json);
                            Cities.Add(city);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }

            return Cities;
        }

        public void GetWeather(int id)
        {
            Log.Info("Test");
            ApiConnection connection = new ApiConnection();
            connection.OnError += ReceiveError;
            connection.OnResponce += ReceiveWeather;
            connection.GetData(Strings.GetWeatherUri(id));

            connection = new ApiConnection();
            connection.OnError += ReceiveError;
            connection.OnResponce += ReceiveForecast;
            connection.GetData(Strings.GetForecastUri(id));
        }

        public IEnumerable<City> GetFavoriteCities()
        {
            IList<City> cities = new List<City>();

            try
            {
                XmlDocument xDoc = new XmlDocument();
                xDoc.Load(Strings.FavoredListLocation);
                XmlElement xNodes = xDoc.DocumentElement;

                foreach (XmlNode xNode in xNodes)
                {
                    int id = int.Parse(xNode.InnerText);
                    City city = Cities.FirstOrDefault(c => c._id == id);
                    if (city != null)
                        cities.Add(city);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }

            return cities;
        }

        public void AddFavoriteCity(City city)
        {
            try
            {
                XmlDocument xDoc = new XmlDocument();
                xDoc.Load(Strings.FavoredListLocation);

                XmlElement xCity = xDoc.CreateElement("city");
                xCity.InnerText = city._id.ToString();
                xDoc.DocumentElement.AppendChild(xCity);
                xDoc.Save(Strings.FavoredListLocation);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
        }

        public void DeleteFavoriteCity(City city)
        {
            try
            {
                XmlDocument xDoc = new XmlDocument();
                xDoc.Load(Strings.FavoredListLocation);

                XmlElement xNodes = xDoc.DocumentElement;

                foreach (XmlNode xNode in xNodes)
                {
                    int id = int.Parse(xNode.InnerText);
                    if (id == city._id)
                        xNode.ParentNode.RemoveChild(xNode);
                }

                xDoc.Save(Strings.FavoredListLocation);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
        }

        private void ReceiveWeather(string json)
        {
            currentWeather = JsonConvert.DeserializeObject<CurrentWeather>(json);
            WeatherDTO weatherDTO = mapper.MapCurrentWeather(currentWeather);
            OnWeatherReady(weatherDTO);
        }

        private void ReceiveForecast(string json)
        {
            try
            {
                forecastList = JsonConvert.DeserializeObject<ForecastList>(json);
                IList<ForecastDTO> forecasts = new List<ForecastDTO>();
                foreach (Forecast forecast in forecastList.Forecasts)
                {
                    forecasts.Add(mapper.MapForecast(forecast));
                }

                IDictionary<DayOfWeek, List<ForecastDTO>> sortedForecasts = forecasts.GroupBy(f => f.DateOfCalculation.DayOfWeek)
                                                                                     .ToDictionary(g => g.Key, g => g.ToList());
                OnForecastsReady(sortedForecasts);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
        }

        private void ReceiveError()
        {
            Log.Error("Connection Lost");
        }
    }
}
