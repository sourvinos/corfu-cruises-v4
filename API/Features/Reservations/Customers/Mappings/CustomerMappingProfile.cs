using API.Infrastructure.Classes;
using AutoMapper;

namespace API.Features.Reservations.Customers {

    public class CustomerMappingProfile : Profile {

        public CustomerMappingProfile() {
            CreateMap<Customer, CustomerListVM>();
            CreateMap<Customer, CustomerBrowserVM>();
            CreateMap<Customer, SimpleEntity>();
            CreateMap<Customer, CustomerReadDto>()
                .ForMember(x => x.TaxOffice, x => x.MapFrom(x => new SimpleEntity { Id = x.TaxOffice.Id, Description = x.TaxOffice.Description }))
                .ForMember(x => x.Nationality, x => x.MapFrom(x => new SimpleEntity { Id = x.Nationality.Id, Description = x.Nationality.Description }));
            CreateMap<CustomerWriteDto, Customer>()
                .ForMember(x => x.Description, x => x.MapFrom(x => x.Description.Trim()))
                .ForMember(x => x.FullDescription, x => x.MapFrom(x => x.FullDescription.Trim()))
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