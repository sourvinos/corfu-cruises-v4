using API.Infrastructure.Classes;
using AutoMapper;

namespace API.Features.Sales.BankAccounts {

    public class BankAccountMappingProfile : Profile {

        public BankAccountMappingProfile() {
            CreateMap<BankAccount, BankAccountListVM>()
                .ForMember(x => x.Bank, x => x.MapFrom(x => new SimpleEntity { Id = x.Bank.Id, Description = x.Bank.Description }))
                .ForMember(x => x.ShipOwner, x => x.MapFrom(x => new SimpleEntity { Id = x.ShipOwner.Id, Description = x.ShipOwner.Description }));
            CreateMap<BankAccount, BankAccountReadDto>()
                .ForMember(x => x.Bank, x => x.MapFrom(x => new SimpleEntity { Id = x.Bank.Id, Description = x.Bank.Description }))
                .ForMember(x => x.ShipOwner, x => x.MapFrom(x => new SimpleEntity { Id = x.ShipOwner.Id, Description = x.ShipOwner.Description }));
            CreateMap<BankAccountWriteDto, BankAccount>()
                .ForMember(x => x.Iban, x => x.MapFrom(x => x.Iban.Trim()));
        }

    }

}