using AutoMapper;

namespace API.Features.Sales.Banks {

    public class BankMappingProfile : Profile {

        public BankMappingProfile() {
            CreateMap<Bank, BankListVM>();
            CreateMap<Bank, BankBrowserVM>();
            CreateMap<Bank, BankReadDto>();
            CreateMap<BankWriteDto, Bank>()
                .ForMember(x => x.Description, x => x.MapFrom(x => x.Description.Trim()));
        }

    }

}