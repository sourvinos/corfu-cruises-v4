using API.Infrastructure.Classes;
using AutoMapper;

namespace API.Features.Reservations.Destinations {

    public class DestinationMappingProfile : Profile {

        public DestinationMappingProfile() {
            CreateMap<Destination, DestinationListVM>();
            CreateMap<Destination, DestinationBrowserVM>();
            CreateMap<Destination, SimpleEntity>();
            CreateMap<Destination, DestinationReadDto>();
            CreateMap<DestinationWriteDto, Destination>()
                .ForMember(x => x.Description, x => x.MapFrom(x => x.Description.Trim()))
                .ForMember(x => x.Abbreviation, x => x.MapFrom(x => x.Abbreviation.Trim()));
        }

    }

}