using API.Infrastructure.Classes;
using AutoMapper;

namespace API.Features.Reservations.IdentityDocuments {

    public class IdentityDocumentMappingProfile : Profile {

        public IdentityDocumentMappingProfile() {
            CreateMap<IdentityDocument, IdentityDocumentListVM>();
            CreateMap<IdentityDocument, IdentityDocumentBrowserVM>();
            CreateMap<IdentityDocument, SimpleEntity>();
            CreateMap<IdentityDocument, IdentityDocumentReadDto>();
            CreateMap<IdentityDocumentWriteDto, IdentityDocument>()
                .ForMember(x => x.Description, x => x.MapFrom(x => x.Description.Trim()));
        }

    }

}