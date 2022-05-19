namespace BankingSystemAssessment.Web.Infrastructure
{
    public interface ICorrelationIdResolver
    {
        string GetCorrelationId();
    }
}