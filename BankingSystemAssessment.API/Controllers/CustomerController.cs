using BankingSystemAssessment.API.Infrastructure.Enums;
using BankingSystemAssessment.API.Models;
using BankingSystemAssessment.API.UseCases.Customer;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace BankingSystemAssessment.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CustomerController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        /// <summary>
        /// This endpoint return userInfo, accounts with mini statement of transactions for each account.
        /// For more detailed transactions statement including paging, search call: GET: api/Transaction?accountId=
        /// </summary>
        /// <param name="id">customet id</param>
        /// <param name="transactionsCount">transactions count per account</param>
        /// <returns>CustomerDetailsModel</returns>
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(CustomerDetailsModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCustomerDetails([FromRoute] int id, [FromQuery] int transactionsCount = 10)
        {
            if (id <= 0)
            {
                return Problem(CustomerErrorCodes.InvalidCustomerId.ToString(), statusCode: 400);
            }

            if (transactionsCount <= 0)
            {
                return Problem(CustomerErrorCodes.InvalidTransactionsCountPerCustomerStatement.ToString(), statusCode: 400);
            }
            var customerByIdQuery = new CustomerByIdQuery(id, transactionsCount);
            var customerDetailsModel = await _mediator.Send(customerByIdQuery);
            return Ok(customerDetailsModel);
        }
    }
}