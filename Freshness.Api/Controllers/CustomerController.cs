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
    [Route("api/customer")]
    [ApiController]
    public class CustomerController : BaseController
    {
        private ICustomerService _customerService;
        private INoteService _noteService;

        public CustomerController(ICustomerService customerService, INoteService noteService)
        {
            _customerService = customerService;
            _noteService = noteService;
        }

        /// <summary>
        /// Retrieves customers
        /// </summary>
        /// <param name="paginationRequestModel">Offset and limit</param>
        /// <returns>HTTP 200 and customers, or HTTP 400, or HTTP 500 with error</returns>
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaginationRequestModel paginationRequestModel)
        {
            if (!ModelState.IsValid)
            {
                return ValidationError(ModelState);
            }

            var customers = await _customerService.GetAsync(paginationRequestModel.Offset, paginationRequestModel.Limit);

            return Ok(customers);
        }

        /// <summary>
        /// Retrieves single customer by id
        /// </summary>
        /// <param name="id">Customer`s id</param>
        /// <returns>HTTP 200 and customer, or HTTP 400, or HTTP 500 with error</returns>
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var customer = await _customerService.FindAsync(item => item.Id == id);

            return Ok(customer);
        }

        /// <summary>
        /// Creates customer
        /// </summary>
        /// <param name="customer">Customer</param>
        /// <returns>HTTP 200 and inserted customer, or HTTP 400, or HTTP 500 with error</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CustomerCreateRequestModel customer)
        {
            if (!ModelState.IsValid)
            {
                return ValidationError(ModelState);
            }

            var createdCustomer = await _customerService.CreateAsync(customer);

            return Ok(createdCustomer);
        }

        /// <summary>
        /// Updates customer
        /// </summary>
        /// <param name="customer">Customer</param>
        /// <returns>HTTP 200 and updated customer, or HTTP 400, or HTTP 500 with error</returns>
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] CustomerUpdateRequestModel customer)
        {
            if (!ModelState.IsValid)
            {
                return ValidationError(ModelState);
            }

            var updatedCustomer = await _customerService.UpdateAsync(customer);

            return Ok(updatedCustomer);
        }

        /// <summary>
        /// Deletes customer
        /// </summary>
        /// <param name="id">Customer`s id</param>
        /// <returns>HTTP 200 and boolean indicator of result, or HTTP 400, or HTTP 500 with error</returns>
        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            var result = await _customerService.DeleteAsync(id);

            return Ok(result);
        }

        /// <summary>
        /// Retrieves notes for customer
        /// </summary>
        /// <param name="customerId">Customer id</param>
        /// <returns>HTTP 200 and notes, or HTTP 400, or HTTP 500 with error</returns>
        [HttpGet]
        [Route("note")]
        public async Task<IActionResult> GetAllNotes([FromQuery] int customerId)
        {
            var notes = await _noteService.GetAsync(item => item.CustomerId == customerId);

            return Ok(notes);
        }

        /// <summary>
        /// Creates note for customer
        /// </summary>
        /// <param name="noteRequestModel">NoteRequestModel</param>
        /// <returns>HTTP 200 and created note, or HTTP 400, or HTTP 500 with error</returns>
        [HttpPost]
        [Route("note")]
        public async Task<IActionResult> CreateNote([FromBody] NoteRequestModel noteRequestModel)
        {
            if (!ModelState.IsValid)
            {
                return ValidationError(ModelState);
            }

            var createdNote = await _noteService.CreateAsync(noteRequestModel);

            return Ok(createdNote);
        }

        /// <summary>
        /// Deletes note
        /// </summary>
        /// <param name="id">Note`s id</param>
        /// <returns>HTTP 200 and boolean indicator of result, or HTTP 400, or HTTP 500 with error</returns>
        [HttpDelete]
        [Route("note")]
        public async Task<IActionResult> DeleteNote([FromQuery] int id)
        {
            var result = await _noteService.DeleteAsync(id);

            return Ok(result);
        }
    }
}
