using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherForecast.Model.Entities.Weather;

namespace WeatherForecast.DTO
{
    public class WeatherDTO
    {
        public Weather Weather { get; set; }

        public Main MainParameters { get; set; }

        public Wind Wind { get; set; }

        public DateTime TimeOfCalculation { get; set; }
    }
}
