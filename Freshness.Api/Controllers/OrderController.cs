using Freshness.Models.RequestModels;
using Freshness.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Freshness.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/order")]
    [ApiController]
    public class OrderController : BaseController
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        /// <summary>
        /// Retrieves orders
        /// </summary>
        /// <param name="paginationRequestModel">Offset and limit</param>
        /// <returns>HTTP 200 and orders, or HTTP 400, or HTTP 500 with error</returns>
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaginationRequestModel paginationRequestModel)
        {
            if (!ModelState.IsValid)
            {
                return ValidationError(ModelState);
            }

            var orders = await _orderService.GetAsync(paginationRequestModel.Offset, paginationRequestModel.Limit);

            return Ok(orders);
        }

        /// <summary>
        /// Retrieves single order by id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>HTTP 200 and order, or HTTP 400, or HTTP 500 with error</returns>
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var order = await _orderService.FindAsync(item => item.Id == id);

            return Ok(order);
        }

        /// <summary>
        /// Creates order
        /// </summary>
        /// <param name="order">Order</param>
        /// <returns>HTTP 200 and inserted order, or HTTP 400, or HTTP 500 with error</returns>
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] OrderCreateRequestModel order)
        {
            if (!ModelState.IsValid)
            {
                return ValidationError(ModelState);

            }
            var createdOrder = await _orderService.CreateAsync(order);

            return Ok(createdOrder);
        }

        /// <summary>
        /// Updates order
        /// </summary>
        /// <param name="order">Order</param>
        /// <returns>HTTP 200 and updated order, or HTTP 400, or HTTP 500 with error</returns>
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] OrderUpdateRequestModel order)
        {
            if (!ModelState.IsValid)
            {
                return ValidationError(ModelState);
            }

            var updatedOrder = await _orderService.UpdateAsync(order);

            return Ok(updatedOrder);
        }

        /// <summary>
        /// Deletes order
        /// </summary>
        /// <param name="id">Order`s id</param>
        /// <returns>HTTP 200 and boolean indicator of result, or HTTP 400, or HTTP 500 with error</returns>
        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            var result = await _orderService.DeleteAsync(id);

            return Ok(result);
        }
    }
}
