using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WeatherForecast.Helpers;

namespace WeatherForecast.Model
{
    public class ApiConnection
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ApiConnection));

        public event Action<string> OnResponce;
        public event Action OnError;

        public async void GetData(string apiUri)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(apiUri);
                    if (response.IsSuccessStatusCode)
                    {
                        string responceJson = await response.Content.ReadAsStringAsync();
                        OnResponce(responceJson);
                    }
                    else
                    {
                        OnError();
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex.Message);
                }
            }
        }
    }
}
