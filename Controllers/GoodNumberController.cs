using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace WeatherApp.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class GoodNumberController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;

        public GoodNumberController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        /* 
            Obviously there are multiple ways to fulfil this task, many of which
           are much more easily readable. I thought this would be a fun approach
           favoring instead performance and brevity of code. 
           
           I liked the idea of applying some basic mathematical reasoning and 
           averaging out the values to detect how many '0's there have been. 
           Plus, it's not very often you get to use the c - '0' approach to 
           convert a char to an int.
        
           In order to most accurately fulfil the brief of the input being a binary 
           string I decided to perform the validation on the input itself by way of
           the RegularExpressionAttribute validator
        */
        [HttpGet()]
        public string Get([RegularExpression(@"^[01]+$", ErrorMessage = "Not a binary string")] string binaryString)
        {
            int total = 0;
            int pos = 0;
            int numLen = binaryString.Length;

            foreach (char c in binaryString) {
                total += c - '0';
                var avg = (double)total/++pos;
                if (avg < .5 || (pos == numLen && avg != .5)) {
                    return "Invalid Number";
                }
            }
            
            return "Valid Number";
        }
    }
}
