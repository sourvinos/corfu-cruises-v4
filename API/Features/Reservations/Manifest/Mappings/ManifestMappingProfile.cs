using API.Features.Reservations.Reservations;
using API.Features.Reservations.ShipCrews;
using API.Infrastructure.Classes;
using API.Infrastructure.Helpers;
using AutoMapper;

namespace API.Features.Reservations.Manifest {

    public class ManifestMappingProfile : Profile {

        public ManifestMappingProfile() {
            CreateMap<Passenger, ManifestPassengerVM>()
                .ForMember(x => x.RefNo, x => x.MapFrom(x => x.Reservation.RefNo))
                .ForMember(x => x.Birthdate, x => x.MapFrom(x => DateHelpers.DateToISOString(x.Birthdate)))
                .ForMember(x => x.Gender, x => x.MapFrom(x => new SimpleEntity { Id = x.Gender.Id, Description = x.Gender.Description }))
                .ForMember(x => x.Nationality, x => x.MapFrom(x => new ManifestNationalityVM { Id = x.Nationality.Id, Code = x.Nationality.Code, Description = x.Nationality.Description, }))
                .ForMember(x => x.Port, x => x.MapFrom(x => new ManifestPortVM {
                    Id = x.Reservation.PortAlternate.Id,
                    Description = x.Reservation.PortAlternate.Description,
                    Locode = x.Reservation.PortAlternate.Locode
                }));
            CreateMap<ShipCrew, ManifestCrewVM>()
                .ForMember(x => x.Birthdate, x => x.MapFrom(x => DateHelpers.DateToISOString(x.Birthdate)))
                .ForMember(x => x.Gender, x => x.MapFrom(x => new SimpleEntity { Id = x.Gender.Id, Description = x.Gender.Description }))
                .ForMember(x => x.Specialty, x => x.MapFrom(x => new SimpleEntity { Id = x.Specialty.Id, Description = x.Specialty.Description }))
                .ForMember(x => x.Nationality, x => x.MapFrom(x => new ManifestNationalityVM { Id = x.Nationality.Id, Code = x.Nationality.Code, Description = x.Nationality.Description, }));
        }

    }

}