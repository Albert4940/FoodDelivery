using FoodDeliveryAPI.Data;
using FoodDeliveryAPI.Models;
using FoodDeliveryAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FoodDeliveryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodController : ControllerBase
    {

        public FoodController(FoodDeliveryContext context)
        {
            FoodService.InitializeContext(context);
            CategoryService.InitializeContext(context);
        }
        // GET: api/<FoodController>
        [HttpGet]
        public async Task<ActionResult<List<Food>>> Get()
        {
            return Ok(await FoodService.GetAll());
        }

        // GET api/<FoodController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Food>> Get(int id)
        {
            var food = await FoodService.GetByID(id);

            if (food is null)
                return NotFound();

            return Ok(food);
        }

        // POST api/<FoodController>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post([FromBody] Food food)
        {
           // var category = cat;
            if (food.Title != null && food.CategoryId != 0 && await CategoryService.GetByID(food.CategoryId) != null)
            {
                bool foodExists = await FoodService.CheckIfFoodExists(food);

                if (foodExists)
                    return Conflict();

                await FoodService.Add(food);
                return Ok("Food Created");
            }
            else
            {
                return BadRequest();
            }
        }

        // PUT api/<FoodController>/5
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Put(int id, [FromBody] Food food)
        {
            if (id != food.Id)
                return BadRequest();

            if (await CategoryService.GetByID(food.CategoryId) is null)
                return BadRequest();
;
            bool foodExists = await FoodService.CheckIfFoodExistsForUpdate(food);

            if (foodExists)
                return Conflict();

            try
            {
                await FoodService.Update(food);
            }
            catch (DbUpdateConcurrencyException)
            {
                //Check If Category is in DB
                var existingFoodByID = await FoodService.GetByID(food.Id);
                if (existingFoodByID is null)
                    return NotFound();
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        // DELETE api/<FoodController>/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var food = await FoodService.GetByID(id);
            if (food is null)
                return NotFound();

            await FoodService.Delete(food);

            return NoContent();
        }
    }
}
