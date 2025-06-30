using System;
using API.Infrastructure.Classes;
using AutoMapper;

namespace API.Infrastructure.Users {

    public class UserMappingProfile : Profile {

        public UserMappingProfile() {
            CreateMap<UserExtended, UserListVM>();
            CreateMap<UserNewDto, UserExtended>()
                .ForMember(x => x.UserName, x => x.MapFrom(x => x.Username.Trim()))
                .ForMember(x => x.Displayname, x => x.MapFrom(x => x.Displayname.Trim()))
                .ForMember(x => x.EmailConfirmed, x => x.MapFrom(x => true))
                .ForMember(x => x.SecurityStamp, x => x.MapFrom(x => Guid.NewGuid().ToString()));
            CreateMap<UserExtended, UserReadDto>()
                .ForMember(x => x.Customer, x => x.MapFrom(x => x.Customer == null
                    ? new SimpleEntity { Id = 0, Description = "(EMPTY)" }
                    : new SimpleEntity { Id = x.Customer.Id, Description = x.Customer.Description }));
        }

    }

}