using BankingSystemAssessment.API.Infrastructure.Enums;
using BankingSystemAssessment.API.Models;
using BankingSystemAssessment.API.UseCases.Customer;
using BankingSystemAssessment.Core.Common.Enums;
using BankingSystemAssessment.Core.Common.Models;
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
        /// <param name="id">customet id record</param>
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

        /// <summary>
        /// get all customers to select the customer you want to show its info
        /// </summary>
        /// <param name="pageIndex">page Index</param>
        /// <param name="pageSize">page size of records</param>
        /// <param name="searchString">search string value to search for</param>
        /// <param name="sortOrder">asc or desc</param>
        /// <param name="sortProperty">column to sort accroding to</param>
        /// <returns>CustomerIndexModel</returns>
        [HttpGet]
        [ProducesResponseType(typeof(PagedResponse<CustomerIndexModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCustomers([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10, [FromQuery] string searchString = null, [FromQuery] string sortOrder = null, [FromQuery] string sortProperty = null)
        {
            if (pageIndex <= 0 || pageSize <= 0)
            {
                return Problem(CommonErrorCodes.InvalidPaging.ToString(), statusCode: 400);
            }

            var customerPagedQuery = new CustomerPagedQuery( pageIndex, pageSize, searchString, sortOrder, sortProperty);
            var pagedCustomerIndexModel = await _mediator.Send(customerPagedQuery);
            return Ok(pagedCustomerIndexModel);
        }
    }
}