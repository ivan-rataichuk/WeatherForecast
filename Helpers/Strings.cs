using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherForecast.Helpers
{
    public static class Strings
    {
        public static readonly string CitiesListLocation = "data/city.list.json";
        public static readonly string FavoredListLocation = "data/favored.xml";

        private static readonly string apiKey = $"&APPID=461022929b42975098cdba6b1b1b9ede";
        private static readonly string weatherApiUri = $"http://api.openweathermap.org/data/2.5/weather?id=";
        private static readonly string forecastApiUri = $"http://api.openweathermap.org/data/2.5/forecast?id=";

        public static string GetWeatherUri(int id)
        {
            return weatherApiUri + id + apiKey;
        }

        public static string GetForecastUri(int id)
        {
            return forecastApiUri + id + apiKey;
        }

    }
}
