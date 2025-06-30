using AutoMapper;

namespace API.Features.Reservations.Drivers{

    public class DriverMappingProfile : Profile {

        public DriverMappingProfile() {
            CreateMap<Driver, DriverListVM>();
            CreateMap<Driver, DriverBrowserVM>();
            CreateMap<Driver, DriverReadDto>();
            CreateMap<DriverWriteDto, Driver>()
                .ForMember(x => x.Description, x => x.MapFrom(x => x.Description.Trim()))
                .ForMember(x => x.Phones, x => x.MapFrom(x => x.Phones.Trim()));
        }

    }

}