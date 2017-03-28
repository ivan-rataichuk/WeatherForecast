using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherForecast.Model.Entities.Weather;

namespace WeatherForecast.DTO
{
    public class ForecastDTO
    {
        public Weather Weather { get; set; }

        public Main MainParameters { get; set; }

        public Wind Wind { get; set; }

        public DateTime TimeOfForecast { get; set; }

        public DateTime DateOfCalculation { get; set; }
    }
}
