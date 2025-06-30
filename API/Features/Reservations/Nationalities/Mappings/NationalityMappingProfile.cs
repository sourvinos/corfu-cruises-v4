using AutoMapper;

namespace API.Features.Reservations.Nationalities {

    public class NationalityMappingProfile : Profile {

        public NationalityMappingProfile() {
            CreateMap<Nationality, NationalityBrowserVM>();
        }

    }

}