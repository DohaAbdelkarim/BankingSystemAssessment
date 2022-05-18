using AutoMapper;
using BankingSystemAssessment.API.Infrastructure.Domain;
using BankingSystemAssessment.API.Models;

namespace BankingSystemAssessment.API.Infrastructure.Mapping
{
    public class TransactionMappingProfile : Profile
    {
        public TransactionMappingProfile()
        {
            CreateMap<Transaction, TransactionModel>()
                .ForMember(destination => destination.TransactionDate, options => options.MapFrom(source => source.TransactionDate.ToString("yyyy/MM/dd")));
        }
    }
}