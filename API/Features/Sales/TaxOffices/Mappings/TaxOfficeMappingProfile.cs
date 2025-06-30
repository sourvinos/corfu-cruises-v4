using AutoMapper;

namespace API.Features.Sales.TaxOffices {

    public class TaxOfficeMappingProfile : Profile {

        public TaxOfficeMappingProfile() {
            CreateMap<TaxOffice, TaxOfficeListVM>();
            CreateMap<TaxOffice, TaxOfficeBrowserVM>();
            CreateMap<TaxOffice, TaxOfficeReadDto>();
            CreateMap<TaxOfficeWriteDto, TaxOffice>()
                .ForMember(x => x.Description, x => x.MapFrom(x => x.Description.Trim()));
        }

    }

}