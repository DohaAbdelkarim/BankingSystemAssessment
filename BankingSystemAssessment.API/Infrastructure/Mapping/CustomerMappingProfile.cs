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

            CreateMap<Customer, CustomerIndexModel>()
              .ForMember(destination => destination.CustomerName, options => options.MapFrom(source => $"{source.FirstName} {source.LastName}"));
        }
    }
}