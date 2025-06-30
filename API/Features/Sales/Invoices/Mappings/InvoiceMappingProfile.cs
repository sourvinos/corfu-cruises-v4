using System.Linq;
using API.Infrastructure.Classes;
using API.Infrastructure.Helpers;
using AutoMapper;

namespace API.Features.Sales.Invoices {

    public class InvoiceMappingProfile : Profile {

        public InvoiceMappingProfile() {
            // List
            CreateMap<Invoice, InvoiceListVM>()
                .ForMember(x => x.Date, x => x.MapFrom(x => DateHelpers.DateToISOString(x.Date)))
                .ForMember(x => x.Customer, x => x.MapFrom(x => new SimpleEntity { Id = x.Customer.Id, Description = x.Customer.Description }))
                .ForMember(x => x.Destination, x => x.MapFrom(x => new SimpleEntity { Id = x.Destination.Id, Description = x.Destination.Description }))
                .ForMember(x => x.DocumentType, x => x.MapFrom(x => new SimpleEntity { Id = x.DocumentType.Id, Description = x.DocumentType.Abbreviation + " - ΣΕΙΡΑ " + x.DocumentType.Batch }))
                .ForMember(x => x.Ship, x => x.MapFrom(x => new SimpleEntity { Id = x.Ship.Id, Description = x.Ship.Description }))
                .ForMember(x => x.ShipOwner, x => x.MapFrom(x => new SimpleEntity { Id = x.ShipOwner.Id, Description = x.ShipOwner.DescriptionEn }))
                .ForMember(x => x.Aade, x => x.MapFrom(x => new InvoiceListAadeVM { Mark = x.Aade.Mark != "", MarkCancel = x.Aade.MarkCancel != "" }));
            // GetById
            CreateMap<Invoice, InvoiceReadDto>()
                .ForMember(x => x.Date, x => x.MapFrom(x => DateHelpers.DateToISOString(x.Date)))
                .ForMember(x => x.TripDate, x => x.MapFrom(x => DateHelpers.DateToISOString(x.TripDate)))
                .ForMember(x => x.Customer, x => x.MapFrom(x => new SimpleEntity {
                    Id = x.Customer.Id,
                    Description = x.Customer.Description
                }))
                .ForMember(x => x.Destination, x => x.MapFrom(x => new SimpleEntity {
                    Id = x.Destination.Id,
                    Description = x.Destination.Description
                }))
                .ForMember(x => x.DocumentType, x => x.MapFrom(x => new DocumentTypeVM {
                    Id = x.DocumentType.Id,
                    Abbreviation = x.DocumentType.AbbreviationEn,
                    Description = x.DocumentType.Description,
                    Batch = x.DocumentType.Batch
                }))
                .ForMember(x => x.Ship, x => x.MapFrom(x => new SimpleEntity {
                    Id = x.Ship.Id,
                    Description = x.Ship.Description
                }))
                .ForMember(x => x.PaymentMethod, x => x.MapFrom(x => new SimpleEntity {
                    Id = x.PaymentMethod.Id,
                    Description = x.PaymentMethod.DescriptionEn
                }))
                .ForMember(x => x.Aade, x => x.MapFrom(x => new InvoiceFormAadeVM {
                    InvoiceId = x.Aade.InvoiceId,
                    Id = x.Aade.Id,
                    UId = x.Aade.Uid,
                    Mark = x.Aade.Mark,
                    MarkCancel = x.Aade.MarkCancel,
                    AuthenticationCode = x.Aade.AuthenticationCode,
                    ICode = x.Aade.ICode,
                    Url = x.Aade.Url,
                    Discriminator = x.Aade.Discriminator
                }))
                .ForMember(x => x.InvoicesPorts, x => x.MapFrom(x => x.InvoicesPorts.Select(port => new InvoicePortReadDto {
                    Id = port.Id,
                    InvoiceId = port.InvoiceId,
                    Port = new SimpleEntity {
                        Id = port.Port.Id,
                        Description = port.Port.Description
                    },
                    AdultsWithTransfer = port.AdultsWithTransfer,
                    AdultsPriceWithTransfer = port.AdultsPriceWithTransfer,
                    AdultsWithoutTransfer = port.AdultsWithoutTransfer,
                    AdultsPriceWithoutTransfer = port.AdultsPriceWithoutTransfer,
                    KidsWithTransfer = port.KidsWithTransfer,
                    KidsPriceWithTransfer = port.KidsPriceWithTransfer,
                    KidsWithoutTransfer = port.KidsWithoutTransfer,
                    KidsPriceWithoutTransfer = port.KidsPriceWithoutTransfer,
                    FreeWithTransfer = port.FreeWithTransfer,
                    FreeWithoutTransfer = port.FreeWithoutTransfer,
                    TotalPax = port.TotalPax,
                    TotalAmount = port.TotalAmount
                })));
            // Create invoice
            CreateMap<InvoiceCreateDto, Invoice>()
                .ForMember(x => x.DiscriminatorId, x => x.MapFrom(x => 1))
                .ForMember(x => x.Aade, x => x.MapFrom(x => new InvoiceAade {
                    InvoiceId = x.InvoiceId,
                    Id = "",
                    Uid = "",
                    Mark = "",
                    MarkCancel = "",
                    AuthenticationCode = "",
                    ICode = "",
                    Url = "",
                    Discriminator = ""
                }))
                .ForMember(x => x.Remarks, x => x.MapFrom(x => x.Remarks.Trim()));
            // Update invoice
            CreateMap<InvoiceUpdateDto, Invoice>();
            // Update aade
            CreateMap<InvoiceUpdateDto, InvoiceAade>();
            // Write port
            CreateMap<InvoicePortWriteDto, InvoicePort>();
        }

    }

}