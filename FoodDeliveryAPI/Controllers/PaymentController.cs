using FoodDeliveryAPI.Data;
using FoodDeliveryAPI.Models;
using FoodDeliveryAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FoodDeliveryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly BaseService _baseService;

        public PaymentController(FoodDeliveryContext context)
        {
            _baseService = new BaseService(context);
        }


         // GET: api/<PaymentController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _baseService.Get<Payment>());
        }

        // GET api/<PaymentController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            var payment = await _baseService.Get<Payment>(id);
            return payment is null ?  NotFound() : Ok(payment);
        }

        // POST api/<PaymentController>
        [HttpPost]
        public async Task<IActionResult> Post(Payment payment)
        {
            await _baseService.Add<Payment>(payment);
            return CreatedAtAction(nameof(Get), new { id = payment.Id }, payment);
        }

        // PUT api/<PaymentController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<PaymentController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var payment = await _baseService.Get<Payment>(id);

            if(payment is null)
                return NotFound();

            await _baseService.Delete<Payment>(payment);

            return NoContent();
        }
    }
}
