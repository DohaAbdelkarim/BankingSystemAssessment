using System.Collections.Generic;
using System.Linq;

namespace BankingSystemAssessment.Core.Common.Models
{
    public class PagedResponse<T> where T : class
    {
        public int Total { get; }
        public IEnumerable<T> Data { get; }

        public PagedResponse(IEnumerable<T> data, int total)
        {
            Data = data;
            Total = total;
        }

        public static PagedResponse<T> Empty()
        {
            return new PagedResponse<T>(Enumerable.Empty<T>(), 0);
        }
    }
}