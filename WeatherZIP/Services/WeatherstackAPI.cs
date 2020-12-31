using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace WeatherZIP.Services
{
    /// <summary>
    /// Logic and services for consuming the Weatherstack API
    /// </summary>
    public class WeatherstackAPI    //TODO build & inherit an IWeatherAPI and inject singleton/static instance at startup
    {
        //TODO: move API key to secrets; config / ENV var
        internal static readonly string _key = "82727441592b5987ded09df48165bc90";   
        public static readonly string _baseUrl = "http://api.weatherstack.com/";   

        public WeatherstackAPI(IHttpClientFactory clientFactory)
        {
            _httpClient = clientFactory.CreateClient("Weatherstack");
        }
        
        private readonly HttpClient _httpClient;


        //TODO convert to async methods to avoid blocking
        //TODO wrap try/catch, network error handling, etc
        public WeatherObject QueryAPI(string query, RequestType type)
        {
            var unit = (type == RequestType.Zip) ? Units.f : Units.m;

            // configure the request    //TODO move _key into factory config?
            var url = $"current?access_key={_key}&query={query}&units={unit}";    //TODO split city/zip endpoints?
            var request = new HttpRequestMessage(HttpMethod.Get, url);

            // send
            var response = _httpClient.SendAsync(request).Result;

            if (response.IsSuccessStatusCode)
            {
                var resultStream = response.Content.ReadAsStringAsync().Result;
                var forecast = JsonSerializer.Deserialize<WeatherObject>(resultStream);

                return forecast;
            }
            else
            {
                //TODO implement error logging / handling
                throw new NotImplementedException("Everything has gone horribly wrong");
            }
        }



        public enum RequestType
        {
            City,
            Zip
        }

        //TODO add Description/Display attribute tags to convert to full names for UI?
        public enum Units   
        {
            m, //Metric,
            f, //Fahrenheit,
            s, //Scientific  //not used yet
        }
    }
}
