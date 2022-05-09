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
    [Route("api/call")]
    [ApiController]
    public class CallController : BaseController
    {
        private readonly ICallService _callService;

        public CallController(ICallService callService)
        {
            _callService = callService;
        }

        /// <summary>
        /// Retrieves calls
        /// </summary>
        /// <param name="paginationRequestModel">Offset and limit</param>
        /// <returns>HTTP 200 and calls, or HTTP 400, or HTTP 500 with error</returns>
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaginationRequestModel paginationRequestModel)
        {
            if (!ModelState.IsValid)
            {
                return ValidationError(ModelState);
            }

            var calls = await _callService.GetAsync(paginationRequestModel.Offset, paginationRequestModel.Limit);

            return Ok(calls);
        }

        /// <summary>
        /// Retrieves call by id
        /// </summary>
        /// <param name="id">Call`s id</param>
        /// <returns>HTTP 200 and call, or HTTP 400, or HTTP 500 with error</returns>
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var calls = await _callService.FindAsync(item => item.Id == id);

            return Ok(calls);
        }

        /// <summary>
        /// Creates call
        /// </summary>
        /// <param name="callCreateRequestModel">CallCreateRequestModel</param>
        /// <returns>HTTP 200 and inserted call, or HTTP 400, or HTTP 500 with error</returns>
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CallCreateRequestModel callCreateRequestModel)
        {
            if (!ModelState.IsValid)
            {
                return ValidationError(ModelState);
            }

            var createdCall = await _callService.CreateAsync(callCreateRequestModel);

            return Ok(createdCall);
        }

        /// <summary>
        /// Updates call
        /// </summary>
        /// <param name="callUpdateRequestModel">CallUpdateRequestModel</param>
        /// <returns>HTTP 200 and updated call, or HTTP 400, or HTTP 500 with error</returns>
        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] CallUpdateRequestModel callUpdateRequestModel)
        {
            if (!ModelState.IsValid)
            {
                return ValidationError(ModelState);
            }

            var updatedCall = await _callService.UpdateAsync(callUpdateRequestModel);

            return Ok(updatedCall);
        }

        /// <summary>
        /// Deletes call
        /// </summary>
        /// <param name="id">Call`s id</param>
        /// <returns>HTTP 200 and boolean indicator of result, or HTTP 400, or HTTP 500 with error</returns>
        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            var result = await _callService.DeleteAsync(id);

            return Ok(result);
        }
    }
}
