using API.Infrastructure.Classes;
using AutoMapper;

namespace API.Features.Reservations.ShipOwners {

    public class ShipOwnerMappingProfile : Profile {

        public ShipOwnerMappingProfile() {
            CreateMap<ShipOwner, ShipOwnerListVM>()
                .ForMember(x => x.Description, x => x.MapFrom(x => x.DescriptionEn));
            CreateMap<ShipOwner, ShipOwnerBrowserVM>()
                .ForMember(x => x.Description, x => x.MapFrom(x => x.DescriptionEn))
                .ForMember(x => x.IsMyData, x => x.MapFrom(x => x.MyDataIsActive))
                .ForMember(x => x.IsOxygen, x => x.MapFrom(x => x.OxygenIsActive));
            CreateMap<ShipOwner, ShipOwnerReadDto>()
                .ForMember(x => x.TaxOffice, x => x.MapFrom(x => new SimpleEntity { Id = x.TaxOffice.Id, Description = x.TaxOffice.Description }))
                .ForMember(x => x.Nationality, x => x.MapFrom(x => new SimpleEntity { Id = x.Nationality.Id, Description = x.Nationality.Description }));
            CreateMap<ShipOwnerWriteDto, ShipOwner>()
                .ForMember(x => x.Description, x => x.MapFrom(x => x.Description.Trim()))
                .ForMember(x => x.DescriptionEn, x => x.MapFrom(x => x.DescriptionEn.Trim()))
                .ForMember(x => x.VatNumber, x => x.MapFrom(x => x.VatNumber.Trim()))
                .ForMember(x => x.Profession, x => x.MapFrom(x => x.Profession.Trim()))
                .ForMember(x => x.Street, x => x.MapFrom(x => x.Street.Trim()))
                .ForMember(x => x.Number, x => x.MapFrom(x => x.Number.Trim()))
                .ForMember(x => x.PostalCode, x => x.MapFrom(x => x.PostalCode.Trim()))
                .ForMember(x => x.City, x => x.MapFrom(x => x.City.Trim()))
                .ForMember(x => x.PersonInCharge, x => x.MapFrom(x => x.PersonInCharge.Trim()))
                .ForMember(x => x.Phones, x => x.MapFrom(x => x.Phones.Trim()));
        }

    }

}