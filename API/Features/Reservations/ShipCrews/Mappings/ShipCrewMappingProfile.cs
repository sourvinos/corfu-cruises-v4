using API.Infrastructure.Classes;
using API.Infrastructure.Helpers;
using AutoMapper;

namespace API.Features.Reservations.ShipCrews {

    public class ShipCrewMappingProfile : Profile {

        public ShipCrewMappingProfile() {
            CreateMap<ShipCrew, ShipCrewListVM>()
                .ForMember(x => x.Birthdate, x => x.MapFrom(x => DateHelpers.DateToISOString(x.Birthdate)))
                .ForMember(x => x.Ship, x => x.MapFrom(x => new SimpleEntity { Id = x.Ship.Id, Description = x.Ship.Description }))
                .ForMember(x => x.Specialty, x => x.MapFrom(x => new SimpleEntity { Id = x.Specialty.Id, Description = x.Specialty.Description }))
                .ForMember(x => x.Birthdate, x => x.MapFrom(x => DateHelpers.DateToISOString(x.Birthdate)));
            CreateMap<ShipCrew, ShipCrewReadDto>()
                .ForMember(x => x.Birthdate, x => x.MapFrom(x => DateHelpers.DateToISOString(x.Birthdate)))
                .ForMember(x => x.Gender, x => x.MapFrom(x => new SimpleEntity { Id = x.Gender.Id, Description = x.Gender.Description }))
                .ForMember(x => x.Nationality, x => x.MapFrom(x => new SimpleEntity { Id = x.Nationality.Id, Description = x.Nationality.Description }))
                .ForMember(x => x.Ship, x => x.MapFrom(x => new SimpleEntity { Id = x.Ship.Id, Description = x.Ship.Description }))
                .ForMember(x => x.Specialty, x => x.MapFrom(x => new SimpleEntity { Id = x.Specialty.Id, Description = x.Specialty.Description }));
            CreateMap<ShipCrewWriteDto, ShipCrew>()
                .ForMember(x => x.Lastname, x => x.MapFrom(x => x.Lastname.Trim()))
                .ForMember(x => x.Firstname, x => x.MapFrom(x => x.Firstname.Trim()))
                .ForMember(x => x.OccupantId, x => x.MapFrom(x => 1));
        }

    }

}