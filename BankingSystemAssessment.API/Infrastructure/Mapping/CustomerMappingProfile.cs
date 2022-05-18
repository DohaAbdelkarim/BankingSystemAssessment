using AutoMapper;
using BankingSystemAssessment.API.Infrastructure.Domain;
using BankingSystemAssessment.API.Models;

namespace BankingSystemAssessment.API.Infrastructure.Mapping
{
    public class CustomerMappingProfile : Profile
    {
        public CustomerMappingProfile()
        {
            CreateMap<Customer, CustomerDetailsModel>()
                .ForPath(destination => destination.Accounts, options => options.MapFrom(source => source.Account));
        }
    }
}