using FoodDeliveryAPI.Data;
using FoodDeliveryAPI.Models;
using FoodDeliveryAPI.Services;
using Microsoft.AspNetCore.Mvc;

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
        }
        // GET: api/<FoodController>
        [HttpGet]
        public async Task<ActionResult<List<Food>>> Get()
        {
            return Ok(await FoodService.GetAll());
        }

        // GET api/<FoodController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<FoodController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Food food)
        {
           /* var category = cat;
            if (cat.Title != null)
            {
                bool categoryExists = await CategoryService.CheckIfCategoryExists(category);

                if (categoryExists)
                    return Conflict();*/

                await FoodService.Add(food);
                return Ok("Food Created");
           /* }
            else
            {
                return BadRequest();
            }*/
        }

        // PUT api/<FoodController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<FoodController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
