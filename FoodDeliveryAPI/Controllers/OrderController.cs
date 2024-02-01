using FoodDeliveryAPI.Data;
using FoodDeliveryAPI.Models;
using FoodDeliveryAPI.Services;
using FoodDeliveryWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FoodDeliveryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        public OrderController(FoodDeliveryContext context)
        {
            OrderService.InitializeContext(context);
            OrderItemService.InitializeContext(context);
            FoodService.InitializeContext(context);
        }
        // GET: api/<OrderController>
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<List<Order>>> Get()
        {
            var CurrentUser = GetCurrent();

            var Orders = await OrderService.GetAll();
            var OrdersUser = Orders.FindAll(o => o.UserId == CurrentUser.Id);

            return Ok(new {
                Order = OrdersUser,
                OrderItems = await OrderItemService.GetAll()
            });
        }

        // GET api/<OrderController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<OrderController>
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Object>> Post(OrderRequest OrderRequest)
        {
            var CurrentUser = GetCurrent();
            if(CurrentUser != null) {
                Order Order = OrderRequest.Order;
                List<OrderItem> OrderItems = OrderRequest?.OrderItems;
                ShippingAddress Address = OrderRequest.ShippingAddress;
                //check is null

                try
                {
                    Order.UserId = CurrentUser.Id;
                    Order.TotalPrice = await OrderService.GetTotalPrice(OrderItems);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
                
                var OrderCreated = await OrderService.Add(Order);
                /*Address.UserId = CurrentUser.Id;
                var AddressResult = await AddressService.Add(Address);*/

                //await OrderItemService.AddRange(OrderItems,OrderCreated.Id);

                return Ok(OrderCreated);
            }
           


            /*var AddressResult = await AddressService.Add(Address);
            var OrderItemsResult = await OrderItemService.AddRange(OrderItems);*/
            return Ok(CurrentUser);
        }

        private User GetCurrent()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity != null)
            {
                var userClaims = identity.Claims;

                return new User
                {
                    Id = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.PrimarySid)?.Value,
                    UserName = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value,
                };
            }
            return null;
        }

        // PUT api/<OrderController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<OrderController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
