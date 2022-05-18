using BankingSystemAssessment.API.Models;
using BankingSystemAssessment.Core.Common.Models;
using MediatR;

namespace BankingSystemAssessment.API.UseCases.Customer
{
    public class CustomerPagedQuery : IRequest<PagedResponse<CustomerIndexModel>>
    {
        public FilterModel FilterModel { get; }

        public CustomerPagedQuery( int pageIndex, int pageSize, string searchString, string sortOrder, string sortProperty)
        {
            FilterModel = new FilterModel()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                SearchString = searchString,
                SortOrder = sortOrder,
                SortProperty = sortProperty
            };
        }
    }
}