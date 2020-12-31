using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WeatherZIP.Services;


namespace WeatherZIP.Controllers
{
    [Route("api")]
    [ApiController]
    public class APIController : ControllerBase
    {
        private WeatherstackAPI _weatherService;   //TODO: build IWeatherService and inject from Startup
        public APIController(IHttpClientFactory httpClientFactory)
        {
            _weatherService = new WeatherstackAPI(httpClientFactory);
        }

        [HttpGet("zip/{zip}")]
        public int GetByZIP(string zip)
        {
            var forecast = _weatherService.QueryAPI(zip, WeatherstackAPI.RequestType.Zip);
            return forecast.current.temperature;
        }

        [HttpGet("city/{cityName}")]
        public int GetByCityName(string cityName)
        {
            var forecast = _weatherService.QueryAPI(cityName, WeatherstackAPI.RequestType.City);
            return forecast.current.temperature;

        }
    }
}
