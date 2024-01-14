using FoodDeliveryAPI.Data;
using FoodDeliveryAPI.Models;
using FoodDeliveryAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FoodDeliveryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoryController : ControllerBase
    {
        public CategoryController(FoodDeliveryContext context)
        {            
            CategoryService.InitializeContext(context);
            UserService.InitializeContext(context);
        }
        // GET: api/<ValuesController>
        [HttpGet]
        public async Task<ActionResult<List<Category>>> Get()
        {
            return Ok(await CategoryService.GetAll());
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> Get(long id)
        {
            var cat = await CategoryService.GetByID(id);

            if (cat is null)
                return NotFound();
            return Ok(cat);
        }

        // POST api/<ValuesController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Category cat)
        {
            var category = cat;
            if(cat.Title != null && await UserService.GetByID(cat.UserId) != null)
            {
                bool categoryExists = await CategoryService.CheckIfCategoryExists(category);

                 if (categoryExists)
                     return Conflict();

                 await CategoryService.Add(cat);
                return Ok("Category Created");
            }
            else
            {
                return BadRequest();
            }
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(long id, [FromBody] Category cat)
        {
            if (id != cat.Id && await UserService.GetByID(cat.UserId) != null)
                return BadRequest();

            //To avoid to put  the name that already exists 
            //Put the title unique in database
            bool categoryExists = await CategoryService.CheckIfCategoryExistsForUpdate(cat);

            if (categoryExists)
                return Conflict();
            //_context.Entry(cat).State = EntityState.Modified;

            try
            {
                await CategoryService.Update(cat);
            }
            catch (DbUpdateConcurrencyException)
            {
                //Check If Category is in DB
                var existingCatByID = await CategoryService.GetByID(cat.Id);
                if (existingCatByID is null)
                    return NotFound();
                else
                {
                    throw;
                }
            }
            

            return NoContent();
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var cat = await CategoryService.GetByID(id);
            if (cat is null)
                return NotFound();

            await CategoryService.Delete(cat);

            return NoContent();
        }
    }
}
