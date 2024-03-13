using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FoodDeliveryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigurationController : ControllerBase
    {
        public IConfiguration _configuration;
        public ConfigurationController(IConfiguration configuration) 
        {
            _configuration = configuration;
        }

        // GET: api/<ValuesController>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new  { 
                PayPalCientId = _configuration["PayPalSettings:ClientId"], 
                PayPalSecret = _configuration["PayPalSettings:SecretKey"], 
                PayPalUrl = _configuration["PayPalSettings:Url"]
            });
        }
        
    }
}
