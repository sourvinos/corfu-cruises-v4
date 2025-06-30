using AutoMapper;

namespace API.Features.Reservations.Parameters {

    public class ParameterMappingProfile : Profile {

        public ParameterMappingProfile() {
            CreateMap<ReservationParameter, ParameterReadDto>();
            CreateMap<ParameterWriteDto, ReservationParameter>()
                .ForMember(x => x.ClosingTime, x => x.MapFrom(x => x.ClosingTime))
                .ForMember(x => x.Phones, x => x.MapFrom(x => x.Phones));
        }

    }

}