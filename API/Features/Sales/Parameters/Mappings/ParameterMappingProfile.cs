using AutoMapper;

namespace API.Features.Sales.Parameters {

    public class ParameterMappingProfile : Profile {

        public ParameterMappingProfile() {
            CreateMap<SaleParameter, ParameterReadDto>();
            CreateMap<ParameterWriteDto, SaleParameter>();
        }

    }

}