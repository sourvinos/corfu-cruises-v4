using AutoMapper;

namespace API.Features.Reservations.CrewSpecialties {

    public class CrewSpecialtyMappingProfile : Profile {

        public CrewSpecialtyMappingProfile() {
            CreateMap<CrewSpecialty, CrewSpecialtyBrowserVM>();
        }

    }

}