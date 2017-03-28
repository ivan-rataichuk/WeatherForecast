using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherForecast.DTO;
using WeatherForecast.Model.Entities.Weather;

namespace WeatherForecast.Helpers
{
    public class Mapper
    {
        public WeatherDTO MapCurrentWeather(CurrentWeather weather)
        {
            WeatherDTO weatherDTO = new WeatherDTO();

            weatherDTO.MainParameters = weather.MainParameters;

            weatherDTO.MainParameters.Temperature = Math.Round(weatherDTO.MainParameters.Temperature - 273.15); // Get Temperature in Celsius
            weatherDTO.MainParameters.Pressure = Math.Round(weatherDTO.MainParameters.Pressure * 0.750061561303); // hPa pressure to mmHg
            weatherDTO.MainParameters.Humidity = Math.Round(weatherDTO.MainParameters.Humidity);

            weatherDTO.Weather = weather.Weather[0];
            weatherDTO.Wind = weather.Wind;
            DateTimeOffset offset = DateTimeOffset.FromUnixTimeSeconds(weather.TimeOfCalculation);
            TimeZone localZone = TimeZone.CurrentTimeZone;
            weatherDTO.TimeOfCalculation = localZone.ToLocalTime(offset.UtcDateTime);

            return weatherDTO;
        }

        public ForecastDTO MapForecast(Forecast forecast)
        {
            ForecastDTO forecastDTO = new ForecastDTO();

            forecastDTO.MainParameters = forecast.MainParameters;

            forecastDTO.MainParameters.Temperature = Math.Round(forecastDTO.MainParameters.Temperature - 273.15); // Get Temperature in Celsius
            forecastDTO.MainParameters.Pressure = Math.Round(forecastDTO.MainParameters.Pressure * 0.750061561303); // hPa pressure to mmHg
            forecastDTO.MainParameters.Humidity = Math.Round(forecastDTO.MainParameters.Humidity);

            forecastDTO.Weather = forecast.Weather[0];
            forecastDTO.Wind = forecast.Wind;

            TimeZone localZone = TimeZone.CurrentTimeZone;

            DateTimeOffset offset = DateTimeOffset.FromUnixTimeSeconds(forecast.TimeOfForecast);
            forecastDTO.TimeOfForecast = localZone.ToLocalTime(offset.UtcDateTime);

            DateTime date = localZone.ToLocalTime( DateTime.ParseExact(forecast.DateOfCalculation, "yyyy-MM-dd HH:mm:ss",
                                       System.Globalization.CultureInfo.InvariantCulture ));

            forecastDTO.DateOfCalculation = date;

            return forecastDTO;
        }
    }
}
