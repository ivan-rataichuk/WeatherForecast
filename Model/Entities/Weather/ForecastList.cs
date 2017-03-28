using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherForecast.Model.Entities.Weather
{
    public class ForecastList
    {
        [JsonProperty("list")]
        public IEnumerable<Forecast> Forecasts { get; set; }
    }
}
