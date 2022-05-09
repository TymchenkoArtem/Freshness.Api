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
    [Route("api/service")]
    [ApiController]
    public class ServiceController : BaseController
    {
        private readonly IServiceService _serviceService;

        public ServiceController(IServiceService serviceService)
        {
            _serviceService = serviceService;
        }

        /// <summary>
        /// Retrieves services
        /// </summary>
        /// <param name="paginationRequestModel">Offset and limit</param>
        /// <returns>HTTP 200 and services, or HTTP 400, or HTTP 500 with error</returns>
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaginationRequestModel paginationRequestModel)
        {
            if (!ModelState.IsValid)
            {
                return ValidationError(ModelState);
            }

            var services = await _serviceService.GetAsync(paginationRequestModel.Offset, paginationRequestModel.Limit);

            return Ok(services);
        }

        /// <summary>
        /// Retrieves service by id
        /// </summary>
        /// <param name="id">Service`s id</param>
        /// <returns>HTTP 200 and service, or HTTP 400, or HTTP 500 with error</returns>
        [AllowAnonymous]
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var service = await _serviceService.FindAsync(item => item.Id == id);

            return Ok(service);
        }

        /// <summary>
        /// Creates service
        /// </summary>
        /// <param name="serviceRequestModel">ServiceCreateRequestModel</param>
        /// <returns>HTTP 200 and inserted service, or HTTP 400, or HTTP 500 with error</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ServiceCreateRequestModel serviceRequestModel)
        {
            if (!ModelState.IsValid)
            {
                return ValidationError(ModelState);
            }

            var createdService = await _serviceService.CreateAsync(serviceRequestModel);

            return Ok(createdService);
        }

        /// <summary>
        /// Updates service
        /// </summary>
        /// <param name="serviceRequestModel">ServiceUpdateRequestModel</param>
        /// <returns>HTTP 200 and updated service, or HTTP 400, or HTTP 500 with error</returns>
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] ServiceUpdateRequestModel serviceRequestModel)
        {
            if (!ModelState.IsValid)
            {
                return ValidationError(ModelState);
            }

            var updatedservice = await _serviceService.UpdateAsync(serviceRequestModel);

            return Ok(updatedservice);
        }

        /// <summary>
        /// Deletes service
        /// </summary>
        /// <param name="id">Service`s id</param>
        /// <returns>HTTP 200 and boolean indicator of result, or HTTP 400, or HTTP 500 with error</returns>
        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            var result = await _serviceService.DeleteAsync(id);

            return Ok(result);
        }
    }
}
