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
    [Route("api/worker")]
    [ApiController]
    public class WorkerController : BaseController
    {
        private IWorkerService _workerService;

        public WorkerController(IWorkerService workerService)
        {
            _workerService = workerService;
        }

        /// <summary>
        /// Login
        /// </summary>
        /// <param name="loginModel">Phone and password</param>
        /// <returns>HTTP 200 and token, or HTTP 400, or HTTP 500 with error</returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LogIn([FromBody] LoginRequestModel loginModel)
        {
            if (!ModelState.IsValid)
            {
                return ValidationError(ModelState);
            }

            var token = await _workerService.LogInAsync(loginModel);

            return Ok(token);
        }

        /// <summary>
        /// Refreshes access token
        /// </summary>
        /// <param name="refreshTokenRequestModel">Access token</param>
        /// <returns>HTTP 200 and token, or HTTP 400, or HTTP 500 with error</returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestModel refreshTokenRequestModel)
        {
            if (!ModelState.IsValid)
            {
                return ValidationError(ModelState);
            }

            var token = await _workerService.RefreshTokenAsync(refreshTokenRequestModel);

            return Ok(token);
        }

        /// <summary>
        /// Resets and sends new pasword to worker`s phone
        /// </summary>
        /// <param name="resetPasswordModel">Phone</param>
        /// <returns>HTTP 200, or HTTP 400, or HTTP 500 with error</returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequestModel resetPasswordModel)
        {
            if (!ModelState.IsValid)
            {
                return ValidationError(ModelState);
            }

            await _workerService.ResetPasswordAsync(resetPasswordModel);

            return Ok();
        }

        /// <summary>
        /// Retrieves workers
        /// </summary>
        /// <param name="paginationRequestModel">Offset and limit</param>
        /// <returns>HTTP 200 and workers, or HTTP 400, or HTTP 500 with error</returns>
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaginationRequestModel paginationRequestModel)
        {
            if (!ModelState.IsValid)
            {
                return ValidationError(ModelState);
            }

            var workers = await _workerService.GetAsync(paginationRequestModel.Offset, paginationRequestModel.Limit);

            return Ok(workers);
        }

        /// <summary>
        /// Retrieves worker by id
        /// </summary>
        /// <param name="id">Worker`s id</param>
        /// <returns>HTTP 200 and worker, or HTTP 400, or HTTP 500 with error</returns>
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var worker = await _workerService.FindAsync(item => item.Id == id);

            return Ok(worker);
        }

        /// <summary>
        /// Creates worker
        /// </summary>
        /// <param name="worker">WorkerCreateRequestModel</param>
        /// <returns>HTTP 200 and inserted worker, or HTTP 400, or HTTP 500 with error</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] WorkerCreateRequestModel worker)
        {
            if (!ModelState.IsValid)
            {
                return ValidationError(ModelState);
            }

            var createdWorker = await _workerService.CreateAsync(worker);

            return Ok(createdWorker);
        }

        /// <summary>
        /// Updates worker
        /// </summary>
        /// <param name="worker">WorkerUpdateRequestModel</param>
        /// <returns>HTTP 200 and updated worker, or HTTP 400, or HTTP 500 with error</returns>
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] WorkerUpdateRequestModel worker)
        {
            if (!ModelState.IsValid)
            {
                return ValidationError(ModelState);
            }

            var updatedWorker = await _workerService.UpdateAsync(worker);

            return Ok(updatedWorker);
        }

        /// <summary>
        /// Deletes worker
        /// </summary>
        /// <param name="id">Worker`s id</param>
        /// <returns>HTTP 200 and boolean indicator of result, or HTTP 400, or HTTP 500 with error</returns>
        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            var result = await _workerService.DeleteAsync(id);

            return Ok(result);
        }
    }
}
