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
            UserService.InitializeContext(context);
            
        }
        // GET: api/<OrderController>
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<List<Order>>> Get()
        {
            var CurrentUser = UserService.GetCurrent(HttpContext);

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
            try
            {
                var CurrentUser = UserService.GetCurrent(HttpContext);
                if (CurrentUser != null)
                {
                    Order Order = OrderRequest.Order;
                    List<OrderItem> OrderItems = OrderRequest?.OrderItems;
                    ShippingAddress Address = OrderRequest.ShippingAddress;
                    Order OrderCreated = null;
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

                    try
                    {
                        OrderCreated = await OrderService.Add(Order);
                        await OrderItemService.AddRange(OrderItems, OrderCreated.Id);
                        /* Address.UserId = CurrentUser.Id;
                         await AddressService.Add(Address);*/
                    }
                    catch (Exception ex)
                    {
                        await OrderService.Delete(OrderCreated);
                        return BadRequest(ex.Message);
                    }

                    return Ok(OrderCreated);
                }
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
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
