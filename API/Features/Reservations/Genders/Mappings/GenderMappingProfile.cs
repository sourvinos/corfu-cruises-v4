using AutoMapper;

namespace API.Features.Reservations.Genders {

    public class GenderMappingProfile : Profile {

        public GenderMappingProfile() {
            CreateMap<Gender, GenderBrowserVM>();
        }

    }

}