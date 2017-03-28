﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherForecast.Model.Entities.Weather
{
    public class Forecast
    {
        [JsonProperty("weather")]
        public Weather[] Weather { get; set; }

        [JsonProperty("main")]
        public Main MainParameters { get; set; }

        [JsonProperty("wind")]
        public Wind Wind { get; set; }

        [JsonProperty("dt")]
        public long TimeOfForecast { get; set; }

        [JsonProperty("dt_txt")]
        public string DateOfCalculation { get; set; }
    }
}
