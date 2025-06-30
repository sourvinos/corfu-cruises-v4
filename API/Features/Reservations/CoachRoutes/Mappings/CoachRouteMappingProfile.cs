using AutoMapper;

namespace API.Features.Reservations.CoachRoutes {

    public class CoachRouteMappingProfile : Profile {

        public CoachRouteMappingProfile() {
            CreateMap<CoachRoute, CoachRouteListVM>();
            CreateMap<CoachRoute, CoachRouteBrowserVM>();
            CreateMap<CoachRoute, CoachRouteReadDto>();
            CreateMap<CoachRouteWriteDto, CoachRoute>()
                .ForMember(x => x.Abbreviation, x => x.MapFrom(x => x.Abbreviation.Trim()))
                .ForMember(x => x.Description, x => x.MapFrom(x => x.Description.Trim()));
        }

    }

}