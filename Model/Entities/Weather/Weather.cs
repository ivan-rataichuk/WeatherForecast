using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherForecast.Model.Entities.Weather
{
    public class Weather
    {
        [JsonProperty("id")]
        public int ConditionId { get; set; }

        [JsonProperty("main")]
        public string ParametersGroup { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("icon")]
        public string IconId { get; set; }
    }
}
