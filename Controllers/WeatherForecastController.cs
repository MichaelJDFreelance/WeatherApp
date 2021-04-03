using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WeatherApp.Models;

namespace WeatherApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet()]
        public IEnumerable<WeatherForecast> Get()
        {
            try {
                var forecasts = WeatherForecast.GetAll();
                return forecasts;
            }
            catch(Exception e) {
                // In a real world scenario I would include a more fleshed out error handling process
                _logger.LogError(e.Message);
                return new List<WeatherForecast>();
            }
            
        }

        [HttpGet("averagetemps")]
        public IEnumerable<SeasonalTemps> AverageTemps(string region)
        {
            try {
                return WeatherForecast.GetAverageSeasonalTemps(region);
            }
            catch(Exception e) {
                _logger.LogError(e.Message);
                return new List<SeasonalTemps>();
            }
            
        }

        [HttpGet("{forecastId}")]
        public WeatherForecast Get(string forecastId)
        {
            try {
                var forecast = WeatherForecast.GetById(forecastId);
                return forecast;
            }
            catch(Exception e) {
                _logger.LogError(e.Message);
                return null;
            }
            
        }

        [HttpDelete]
        public string Delete(string forecastId)
        {
            try {
                var forecast = WeatherForecast.GetById(forecastId);
                forecast.Delete();
                return "Success"; 
            }
            catch(Exception e) {
                _logger.LogError(e.Message);
                return "Failed";
            }          
        }

        [HttpPost]
        public string Post(string forecastId, string fieldname, string fieldvalue)
        {
            try {
                var forecast = WeatherForecast.GetById(forecastId);
                forecast.Update(fieldname, fieldvalue);
                return "Success";
            }
             catch(Exception e) {
                 _logger.LogError(e.Message);
                 return "Failed";
             }
        }

        [HttpPut]
        public string Put([FromQuery] WeatherForecast forecast)
        {
            try {
                forecast.Save();
                return "Success"; 
            }
            catch(Exception e) {
                _logger.LogError(e.Message);
                return "Failed";
            }           
        }
    }
}
