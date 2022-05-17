namespace BankingSystemAssessment.Core.Common.Models
{
    public class FilterModel
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string SearchString { get; set; }
        public string SortOrder { get; set; }
        public string SortProperty { get; set; }
    }
}