using FoodDeliveryAPI.Data;
using FoodDeliveryAPI.Models;
using FoodDeliveryAPI.Services;
using FoodDeliveryAPI.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FoodDeliveryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private BaseService _baseService;
        private IConfiguration _configuration;
        public OrderController(FoodDeliveryContext context, IConfiguration configuration)
        {
            _configuration = configuration;

            OrderService.InitializeContext(context);
            OrderItemService.InitializeContext(context);
            FoodService.InitializeContext(context);
            UserService.InitializeContext(context);

            _baseService = new BaseService(context);
        }
        // GET: api/<OrderController>
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<List<Order>>> Get()
        {
            var CurrentUser = UserService.GetCurrent(HttpContext);

            //var Orders = await OrderService.GetAll();
            var Orders = await _baseService.Get<Order>();
            var OrdersUser = Orders.FindAll(o => o.UserId == CurrentUser.Id);

            if (OrdersUser != null)
                return Ok(OrdersUser);
            else
                return NotFound();
        }

        // GET api/<OrderController>/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult> Get(long id)
        {

           // var Order = await OrderService.GetByID(id);
            var Order = await _baseService.Get<Order>(id);

            if (Order is null)
                return NotFound();

            //var OrderItems = await OrderItemService.GetByOrderID(id);
            var OrderItems = await _baseService.Get<OrderItem>();
            var OrderItemsFilter = OrderItems.FindAll(oI => oI.OrderId == Order.Id);

            var Addresses = await _baseService.Get<ShippingAddress>();
            var ShippingAddress = Addresses.FirstOrDefault(a => a.UserId == Order.UserId);

            var Payments = await _baseService.Get<Payment>();
            var Payment = Payments.FirstOrDefault(p => p.OrderId == Order.Id);

             return Ok(new {Order, OrderItems = OrderItemsFilter, ShippingAddress, Payment});            
        }

        // POST api/<OrderController>
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Object>> Post(OrderRequest OrderRequest)
        {
            //return Ok(OrderRequest);
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
                    if (OrderItems is null || OrderItems.Count == 0)
                        return BadRequest("OrderItems empty");

                    if(Address is null)
                        return BadRequest("Shipping Address is empty");


                    // return Ok(Order);
                    //Add order inside a single try catch
                    try
                    {
                        Order.UserId = CurrentUser.Id;
                        var Configuration = ConfigurationUtil.GetConfiguration(_configuration);

                        int TaxPercent = Configuration.TaxPercent;
                        decimal DeliveryFee = Configuration.DeliveryFee;

                        decimal ItemsPrice = await OrderItemService.GetItemsPrice(OrderItems);
                        decimal TaxPrice = ((DeliveryFee + ItemsPrice) * TaxPercent) / 100;
                        decimal TotalPrice = TaxPrice + DeliveryFee + ItemsPrice;

                        Order.DeliveryFee = DeliveryFee;                        
                        Order.ItemsPrice = ItemsPrice;
                        Order.TaxPrice = TaxPrice;
                        Order.TotalPrice = TotalPrice;

                        //return Ok(Order);
                    }
                    catch (Exception ex)
                    {
                         return BadRequest(ex.Message);
                        //return BadRequest(new { ErrorMessage = ex.Message });
                    }

                    
                    try
                    {
                        // OrderCreated = await OrderService.Add(Order);
                        Order.Id = 0;
                        OrderCreated = await _baseService.Add<Order>(Order);
                        await OrderItemService.AddRange(OrderItems, OrderCreated.Id);

                        await FoodService.UpdateCountInStock(OrderItems);

                        /* Address.UserId = CurrentUser.Id;
                         await AddressService.Add(Address);*/
                    }
                    catch (Exception ex)
                    {
                        await OrderService.Delete(OrderCreated);
                        return BadRequest(ex.Message);
                    }

                    Address.Id = 0;
                    Address.UserId = CurrentUser.Id;

                    await _baseService.Add<ShippingAddress>(Address);

                    return Ok(OrderCreated);
                    //return Ok(await Get(OrderCreated.Id));
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
        public async Task<IActionResult> Put(long id, Order Order)
        {
            
            var OrderExisting = await _baseService.Get<Order>(id);
            if (OrderExisting is null)
                return NotFound();

            await _baseService.Update<Order>(Order);
            return NoContent();
        }

        // DELETE api/<OrderController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
