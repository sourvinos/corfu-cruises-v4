using API.Infrastructure.Classes;
using API.Infrastructure.Helpers;
using AutoMapper;

namespace API.Features.Sales.Prices {

    public class PriceMappingProfile : Profile {

        public PriceMappingProfile() {
            // Get
            CreateMap<Price, PriceListVM>()
                .ForMember(x => x.From, x => x.MapFrom(x => DateHelpers.DateToISOString(x.From)))
                .ForMember(x => x.To, x => x.MapFrom(x => DateHelpers.DateToISOString(x.To)))
                .ForMember(x => x.Customer, x => x.MapFrom(x => new SimpleEntity { Id = x.Customer.Id, Description = x.Customer.Description }))
                .ForMember(x => x.Destination, x => x.MapFrom(x => new SimpleEntity { Id = x.Destination.Id, Description = x.Destination.Description }))
                .ForMember(x => x.Port, x => x.MapFrom(x => new SimpleEntity { Id = x.Port.Id, Description = x.Port.Description }));
            // GetById
            CreateMap<Price, PriceReadDto>()
                .ForMember(x => x.From, x => x.MapFrom(x => DateHelpers.DateToISOString(x.From)))
                .ForMember(x => x.To, x => x.MapFrom(x => DateHelpers.DateToISOString(x.To)))
                .ForMember(x => x.Customer, x => x.MapFrom(x => new SimpleEntity { Id = x.Customer.Id, Description = x.Customer.Description }))
                .ForMember(x => x.Destination, x => x.MapFrom(x => new SimpleEntity { Id = x.Destination.Id, Description = x.Destination.Description }))
                .ForMember(x => x.Port, x => x.MapFrom(x => new SimpleEntity { Id = x.Port.Id, Description = x.Port.Description }));
            // Write
            CreateMap<PriceWriteDto, Price>();
        }

    }

}