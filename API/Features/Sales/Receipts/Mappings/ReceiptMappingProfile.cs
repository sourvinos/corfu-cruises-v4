using API.Infrastructure.Classes;
using API.Infrastructure.Helpers;
using AutoMapper;

namespace API.Features.Sales.Receipts {

    public class ReservationMappingProfile : Profile {

        public ReservationMappingProfile() {
            // List
            CreateMap<Receipt, ReceiptListVM>()
                .ForMember(x => x.Date, x => x.MapFrom(x => DateHelpers.DateToISOString(x.Date)))
                .ForMember(x => x.Customer, x => x.MapFrom(x => new SimpleEntity { Id = x.Customer.Id, Description = x.Customer.Description }))
                .ForMember(x => x.DocumentType, x => x.MapFrom(x => new SimpleEntity { Id = x.DocumentType.Id, Description = x.DocumentType.Abbreviation + " - ΣΕΙΡΑ " + x.DocumentType.Batch }))
                .ForMember(x => x.ShipOwner, x => x.MapFrom(x => new SimpleEntity { Id = x.ShipOwner.Id, Description = x.ShipOwner.DescriptionEn }))
                .ForMember(x => x.PaymentMethod, x => x.MapFrom(x => new SimpleEntity { Id = x.PaymentMethod.Id, Description = x.PaymentMethod.Description }));
            // GetById
            CreateMap<Receipt, ReceiptReadDto>()
                .ForMember(x => x.Date, x => x.MapFrom(x => DateHelpers.DateToISOString(x.Date)))
                .ForMember(x => x.TripDate, x => x.MapFrom(x => DateHelpers.DateToISOString(x.TripDate)))
                .ForMember(x => x.Customer, x => x.MapFrom(x => new SimpleEntity {
                    Id = x.Customer.Id,
                    Description = x.Customer.Description
                }))
                .ForMember(x => x.DocumentType, x => x.MapFrom(x => new DocumentTypeVM {
                    Id = x.DocumentType.Id,
                    Abbreviation = x.DocumentType.Abbreviation,
                    Description = x.DocumentType.Description,
                    Batch = x.DocumentType.Batch
                }))
                .ForMember(x => x.PaymentMethod, x => x.MapFrom(x => new SimpleEntity {
                    Id = x.PaymentMethod.Id,
                    Description = x.PaymentMethod.Description
                }))
                .ForMember(x => x.ShipOwner, x => x.MapFrom(x => new SimpleEntity {
                    Id = x.ShipOwner.Id,
                    Description = x.ShipOwner.DescriptionEn
                }));
            CreateMap<ReceiptWriteDto, Receipt>()
                .ForMember(x => x.DiscriminatorId, x => x.MapFrom(x => 2))
                .ForMember(x => x.Remarks, x => x.MapFrom(x => x.Remarks.Trim()));
        }

    }

}