﻿using FoodDeliveryAPI.Data;
using FoodDeliveryAPI.Models;
using FoodDeliveryAPI.Services;
using FoodDeliveryWebApp.Models;
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
        [Authorize]
        public async Task<ActionResult> Get(long id)
        {

            var Order = await OrderService.GetByID(id);
            var OrderItems = await OrderItemService.GetByOrderID(id);
             if (Order is null)
                  return NotFound();

             return Ok(new {cart = Order,OrderItems});            
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

                   // return Ok(Order);
                    try
                    {
                        Order.UserId = CurrentUser.Id;
                        Order.TotalPrice = await OrderService.GetTotalPrice(OrderItems);
                    }
                    catch (Exception ex)
                    {
                         return BadRequest(ex.Message);
                        //return BadRequest(new { ErrorMessage = ex.Message });
                    }

                    //return Ok(Order);
                    try
                    {
                        OrderCreated = await OrderService.Add(Order);
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

        [HttpPost("AddRange")]
        public async Task<ActionResult> AddRange()
        {

            return Ok("OK");
                    //try
                    //{
                        //OrderCreated = await OrderService.Add(Order);

                        //await OrderItemService.AddRange(orders, 40);
                        //return Ok(orders);

                        //Sawait FoodService.UpdateCountInStock(OrderItems);

                        /* Address.UserId = CurrentUser.Id;
                         await AddressService.Add(Address);*/
                    

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
