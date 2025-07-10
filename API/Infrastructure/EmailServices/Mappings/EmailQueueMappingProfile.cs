using API.Infrastructure.Helpers;
using AutoMapper;

namespace API.Infrastructure.EmailServices {

    public class EmailQueueMappingProfile : Profile {

        public EmailQueueMappingProfile() {
            CreateMap<EmailQueueDto, EmailQueue>()
                .ForMember(x => x.PostAt, x => x.MapFrom(x => DateHelpers.DateTimeToISOString(DateHelpers.GetLocalDateTime())));
        }

    }

}