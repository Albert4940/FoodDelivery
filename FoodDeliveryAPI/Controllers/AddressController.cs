using FoodDeliveryAPI.Data;
using FoodDeliveryAPI.Models;
using FoodDeliveryAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FoodDeliveryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private BaseService _baseService;

        public AddressController(FoodDeliveryContext context)
        {
            _baseService = new BaseService(context);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _baseService.Get<ShippingAddress>());
        }

        [HttpGet("{id}")]
        // GET: AddressController
        public async Task<IActionResult> Get(long id)
        {
            var address = await _baseService.Get<ShippingAddress>(id);

            if(address is null)
                return NotFound();

            return Ok(address);
        }



        // GET: AddressController/Create
        [HttpPost]
        public async Task<IActionResult> Create(ShippingAddress address)
        {

            await _baseService.Add<ShippingAddress>(address);
            return CreatedAtAction(nameof(Get), new {id = address.Id}, address);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, ShippingAddress address)
        {
            if (id != address.Id)
                return BadRequest();

            var existingAddress = await _baseService.Get<ShippingAddress>(id);
            if (existingAddress is null)
                return NotFound();

            await _baseService.Update<ShippingAddress>(address);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var address = await _baseService.Get<ShippingAddress>(id);

            if (address is null)
                return NotFound();

            await _baseService.Delete<ShippingAddress>(address);

            return NoContent();
        }

    }
}
