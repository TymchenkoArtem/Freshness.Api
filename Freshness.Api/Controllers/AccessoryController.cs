using Freshness.Models.RequestModels;
using Freshness.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Freshness.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/accessory")]
    [ApiController]
    public class AccessoryController : BaseController
    {
        private readonly IAccessoryService _accessoryService;

        public AccessoryController(IAccessoryService accessoryService)
        {
            _accessoryService = accessoryService;
        }

        /// <summary>
        /// Retrieves accessories
        /// </summary>
        /// <param name="paginationRequestModel">Offset and limit</param>
        /// <returns>HTTP 200 and accessories, or HTTP 400, or HTTP 500 with error</returns>
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaginationRequestModel paginationRequestModel)
        {
            if (!ModelState.IsValid)
            {
                return ValidationError(ModelState);
            }

            var accessories = await _accessoryService.GetAsync(paginationRequestModel.Offset, paginationRequestModel.Limit);

            return Ok(accessories);
        }

        /// <summary>
        /// Retrieves accessory by id
        /// </summary>
        /// <param name="id">Accessory`s id</param>
        /// <returns>HTTP 200 and accessory, or HTTP 400, or HTTP 500 with error</returns>
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var accessory = await _accessoryService.FindAsync(item => item.Id == id);

            return Ok(accessory);
        }

        /// <summary>
        /// Creates accessory
        /// </summary>
        /// <param name="accessoryRequestModel">AccessoryCreateRequestModel</param>
        /// <returns>HTTP 200 and inserted accessory, or HTTP 400, or HTTP 500 with error</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AccessoryCreateRequestModel accessoryRequestModel)
        {
            if (!ModelState.IsValid)
            {
                return ValidationError(ModelState);
            }

            var createdAccessory = await _accessoryService.CreateAsync(accessoryRequestModel);

            return Ok(createdAccessory);
        }

        /// <summary>
        /// Updates accessory
        /// </summary>
        /// <param name="accessoryRequestModel">AccessoryUpdateRequestModel</param>
        /// <returns>HTTP 200 and updated accessory, or HTTP 400, or HTTP 500 with error</returns>
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] AccessoryUpdateRequestModel accessoryRequestModel)
        {
            if (!ModelState.IsValid)
            {
                return ValidationError(ModelState);
            }

            var updatedAccessory = await _accessoryService.UpdateAsync(accessoryRequestModel);

            return Ok(updatedAccessory);
        }

        /// <summary>
        /// Deletes accessory
        /// </summary>
        /// <param name="id">Accessory`s id</param>
        /// <returns>HTTP 200 and boolean indicator of result, or HTTP 400, or HTTP 500 with error</returns>
        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            var result = await _accessoryService.DeleteAsync(id);

            return Ok(result);
        }
    }
}
