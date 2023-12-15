using AutoMapper;
using BankSystemManagement.CommonInterface.DTOs;
using BankSystemManagement.Core.DTOs;
using BankSystemManagement.Domain.Models;


namespace BankSystemManagement.Core.AutoMapper
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            //mapper.Map<DestinationType>(sourceObject)

            // Model to DTO mappings
            //<source,destination>




            CreateMap<CustomerCredential, CustomerRegistrationDto>()
           .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
           .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.PasswordHash));

            CreateMap<Admin, AdminRegistrationDto>()
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
            .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.PasswordHash));

            CreateMap<Account, CustomerAccountOverviewDto>()
            .ForMember(dest => dest.AccountType, opt => opt.MapFrom(src => src.AccountTypes))
            .ForMember(dest => dest.Balance, opt => opt.MapFrom(src => src.Balance));


            CreateMap<Transaction, AccountTransactionDto>()
           .ForMember(dest => dest.TransactionDate, opt => opt.MapFrom(src => src.TransactionDate))
           .ForMember(dest => dest.TransactionTypeName, opt => opt.MapFrom(src => src.TransactionTypes.TypeName))
           .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
           .ForMember(dest => dest.Balance, opt => opt.MapFrom(src => src.Balance));


        }
    }
}
