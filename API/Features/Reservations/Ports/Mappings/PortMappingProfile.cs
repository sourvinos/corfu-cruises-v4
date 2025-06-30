using API.Infrastructure.Classes;
using AutoMapper;

namespace API.Features.Reservations.Ports {

    public class PortMappingProfile : Profile {

        public PortMappingProfile() {
            CreateMap<Port, PortListVM>();
            CreateMap<Port, PortBrowserVM>();
            CreateMap<Port, SimpleEntity>();
            CreateMap<Port, PortReadDto>();
            CreateMap<PortWriteDto, Port>()
                .ForMember(x => x.Abbreviation, x => x.MapFrom(x => x.Abbreviation.Trim()))
                .ForMember(x => x.Description, x => x.MapFrom(x => x.Description.Trim()))
                .ForMember(x => x.Locode, x => x.MapFrom(x => x.Locode.Trim()));
        }

    }

}