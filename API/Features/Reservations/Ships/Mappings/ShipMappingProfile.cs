using API.Infrastructure.Classes;
using AutoMapper;

namespace API.Features.Reservations.Ships {

    public class ShipMappingProfile : Profile {

        public ShipMappingProfile() {
            CreateMap<Ship, ShipListVM>();
            CreateMap<Ship, ShipBrowserVM>()
                .ForMember(x => x.ShipOwner, x => x.MapFrom(x => new ShipOwnerBrowserVM {
                    Id = x.ShipOwner.Id,
                    Description = x.ShipOwner.DescriptionEn,
                    IsMyData = x.ShipOwner.MyDataIsActive,
                    IsOxygen = x.ShipOwner.OxygenIsActive,
                    VatPercent = x.ShipOwner.VatPercent
                }));
            CreateMap<Ship, SimpleEntity>();
            CreateMap<Ship, ShipReadDto>()
                .ForMember(x => x.ShipOwner, x => x.MapFrom(x => new SimpleEntity {
                    Id = x.ShipOwner.Id,
                    Description = x.ShipOwner.DescriptionEn
                }));
            CreateMap<ShipWriteDto, Ship>()
                .ForMember(x => x.Description, x => x.MapFrom(x => x.Description.Trim()))
                .ForMember(x => x.Abbreviation, x => x.MapFrom(x => x.Abbreviation.Trim()))
                .ForMember(x => x.RegistryNo, x => x.MapFrom(x => x.RegistryNo.Trim()));
        }

    }

}