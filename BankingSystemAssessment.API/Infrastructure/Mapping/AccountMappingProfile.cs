using AutoMapper;
using BankingSystemAssessment.API.Infrastructure.Domain;
using BankingSystemAssessment.API.Infrastructure.Enums;
using BankingSystemAssessment.API.Models;
using System;

namespace BankingSystemAssessment.API.Infrastructure.Mapping
{
    public class AccountMappingProfile : Profile
    {
        public AccountMappingProfile()
        {
            CreateMap<CreateAccountRequestModel, Account>()
                .ForMember(destination => destination.CustomerId, options => options.MapFrom(source => source.CustomerId))
                .ForMember(destination => destination.Balance, options => options.MapFrom(source => 0))
                .ForMember(destination => destination.Status, options => options.MapFrom(source => AccountStatus.Active.ToString()))
                .ForMember(destination => destination.CreatedDate, options => options.MapFrom(source => DateTimeOffset.Now.Date))
                //Assume the bank in Egypt and the default account currency will be EGP
                .ForMember(destination => destination.Currency, options => options.MapFrom(source => Currency.EGP.ToString()));
            
            CreateMap<Account, AccountModel>()
                .ForPath(destination => destination.Transactions, options => options.MapFrom(source => source.Transaction));
        }
    }
}
