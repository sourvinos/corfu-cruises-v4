using API.Infrastructure.Classes;
using API.Infrastructure.Helpers;
using AutoMapper;

namespace API.Features.Reservations.Schedules {

    public class ScheduleMappingProfile : Profile {

        public ScheduleMappingProfile() {
            // GetAll
            CreateMap<Schedule, ScheduleListVM>()
                .ForMember(x => x.Date, x => x.MapFrom(x => DateHelpers.DateToISOString(x.Date)))
                .ForMember(x => x.Year, x => x.MapFrom(x => new SimpleEntity { Id = x.Date.Year, Description = x.Date.Year.ToString() }))
                .ForMember(x => x.Destination, x => x.MapFrom(x => new SimpleEntity { Id = x.Destination.Id, Description = x.Destination.Description }))
                .ForMember(x => x.Port, x => x.MapFrom(x => new SimpleEntity { Id = x.Port.Id, Description = x.Port.Description }));
            // GetById
            CreateMap<Schedule, ScheduleReadDto>()
                .ForMember(x => x.Date, x => x.MapFrom(x => DateHelpers.DateToISOString(x.Date)))
                .ForMember(x => x.Destination, x => x.MapFrom(x => new SimpleEntity { Id = x.Destination.Id, Description = x.Destination.Description }))
                .ForMember(x => x.Port, x => x.MapFrom(x => new SimpleEntity { Id = x.Port.Id, Description = x.Port.Description }));
            // Write
            CreateMap<ScheduleWriteDto, Schedule>();
        }

    }

}