using BankingSystemAssessment.API.Infrastructure.Enums;
using BankingSystemAssessment.API.Models;
using BankingSystemAssessment.API.UseCases.Transaction;
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
    public class TransactionController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TransactionController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        /// <summary>
        /// This endpoint show detailed transactions, user can search transactions in specific period 
        /// or search for value of any column in TransactionModel
        /// user can choose sortOrder, sort column
        /// The default is searching accroding to Transaction date desc
        /// </summary>
        /// <param name="accountId">customer account id</param>
        /// <param name="pageIndex">page Index</param>
        /// <param name="pageSize">page size of records</param>
        /// <param name="transactionDateFrom">search transaction Date from</param>
        /// <param name="transactionDateTo">search transaction Date to</param>
        /// <param name="searchString">search string value to search for</param>
        /// <param name="sortOrder">asc or desc</param>
        /// <param name="sortProperty">column to sort accroding to</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{accountId}")]
        [ProducesResponseType(typeof(PagedResponse<TransactionModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTransactions([FromRoute] int accountId, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10, [FromQuery] string transactionDateFrom = null, [FromQuery] string transactionDateTo = null, [FromQuery] string searchString = null, [FromQuery] string sortOrder = null, [FromQuery] string sortProperty = null)
        {
            if (accountId <= 0)
            {
                return Problem(TransactionErrorCodes.InvalidAccountId.ToString(), statusCode: 400);
            }
            if (pageIndex <= 0 || pageSize <= 0)
            { 
                return Problem(CommonErrorCodes.InvalidPaging.ToString(), statusCode: 400);
            }

            var transactionPagedQuery = new TransactionPagedQuery(accountId, pageIndex, pageSize, transactionDateFrom, transactionDateTo, searchString, sortOrder, sortProperty);
            var pagedTransactionModel = await _mediator.Send(transactionPagedQuery);
            return Ok(pagedTransactionModel);
        }
    }
}