using System.Linq;
using API.Infrastructure.Classes;
using API.Infrastructure.Helpers;
using AutoMapper;

namespace API.Features.Sales.Invoices {

    public class InvoicePdfMappingProfile : Profile {

        public InvoicePdfMappingProfile() {
            CreateMap<Invoice, InvoicePdfVM>()
                .ForMember(x => x.Header, x => x.MapFrom(x => new InvoicePdfHeaderVM {
                    Date = DateHelpers.DateToISOString(x.Date),
                    TripDate = DateHelpers.DateToISOString(x.TripDate),
                    DocumentType = new InvoicePdfDocumentTypeVM {
                        Description = x.DocumentType.Description,
                        Batch = x.DocumentType.Batch
                    },
                    InvoiceNo = x.InvoiceNo
                }))
                .ForMember(x => x.Customer, x => x.MapFrom(x => new InvoicePdfPartyVM {
                    Id = x.Customer.Id,
                    FullDescription = x.Customer.FullDescription,
                    VatNumber = x.Customer.VatNumber,
                    Branch = x.Customer.Branch,
                    Profession = x.Customer.Profession,
                    Street = x.Customer.Street,
                    Number = x.Customer.Number,
                    PostalCode = x.Customer.PostalCode,
                    City = x.Customer.City,
                    Phones = x.Customer.Phones,
                    Email = x.Customer.Email,
                    Nationality = x.Customer.Nationality.Description,
                    TaxOffice = x.Customer.TaxOffice.Description
                }))
                .ForMember(x => x.Destination, x => x.MapFrom(x => x.Destination.Description))
                .ForMember(x => x.Ship, x => x.MapFrom(x => new InvoicePdfShipVM {
                    Description = x.Ship.Description,
                    RegistryNo = x.Ship.RegistryNo
                }))
                .ForMember(x => x.DocumentType, x => x.MapFrom(x => new InvoicePdfDocumentTypeVM {
                    Description = x.DocumentType.Description,
                    Batch = x.DocumentType.Batch
                }))
                .ForMember(x => x.Issuer, x => x.MapFrom(x => new InvoicePdfPartyVM {
                    FullDescription = x.Ship.ShipOwner.Description,
                    VatNumber = x.Ship.ShipOwner.VatNumber,
                    Branch = x.Ship.ShipOwner.Branch,
                    Profession = x.Ship.ShipOwner.Profession,
                    Street = x.Ship.ShipOwner.Street,
                    Number = x.Ship.ShipOwner.Number,
                    PostalCode = x.Ship.ShipOwner.PostalCode,
                    City = x.Ship.ShipOwner.City,
                    Phones = x.Ship.ShipOwner.Phones,
                    Email = x.Ship.ShipOwner.Email,
                    Nationality = x.Ship.ShipOwner.Nationality.Description,
                    TaxOffice = x.Ship.ShipOwner.TaxOffice.Description
                }))
                .ForMember(x => x.PaymentMethod, x => x.MapFrom(x => x.PaymentMethod.Description))
                .ForMember(x => x.Aade, x => x.MapFrom(x => new InvoicePdfAadeVM {
                    UId = x.Aade.Uid,
                    Mark = x.Aade.Mark,
                    MarkCancel = x.Aade.MarkCancel,
                    Url = x.Aade.Url,
                    AuthenticationCode = x.Aade.AuthenticationCode,
                    ICode = x.Aade.ICode,
                    Discriminator = x.Aade.Discriminator,
                    ProviderLabel = "Πάροχος Ηλ.Τιμολόγησης: Cloud Services Ι.Κ.Ε. - https://www.oxygen.gr",
                    FailureLabel = "Απώλεια Διασύνδεσης Επιχείρησης – Παρόχου - Transmission Failure_1"
                }))
                .ForMember(x => x.Ports, x => x.MapFrom(x => x.InvoicesPorts.Select(port => new InvoicePdfPortVM {
                    Port = port.Port.Description,
                    AdultsWithTransfer = port.AdultsWithTransfer,
                    AdultsPriceWithTransfer = port.AdultsPriceWithTransfer,
                    AdultsTotalAmountWithTransfer = port.AdultsWithTransfer * port.AdultsPriceWithTransfer,
                    AdultsWithoutTransfer = port.AdultsWithoutTransfer,
                    AdultsPriceWithoutTransfer = port.AdultsPriceWithoutTransfer,
                    AdultsTotalAmountWithoutTransfer = port.AdultsWithoutTransfer * port.AdultsPriceWithoutTransfer,
                    KidsWithTransfer = port.KidsWithTransfer,
                    KidsPriceWithTransfer = port.KidsPriceWithTransfer,
                    KidsTotalAmountWithTransfer = port.KidsWithTransfer * port.KidsPriceWithTransfer,
                    KidsWithoutTransfer = port.KidsWithoutTransfer,
                    KidsPriceWithoutTransfer = port.KidsPriceWithoutTransfer,
                    KidsTotalAmountWithoutTransfer = port.KidsWithoutTransfer * port.KidsPriceWithoutTransfer,
                    FreeWithTransfer = port.FreeWithTransfer,
                    FreeWithoutTransfer = port.FreeWithoutTransfer,
                    TotalPax = port.TotalPax,
                    TotalAmount = port.TotalAmount
                })))
                .ForMember(x => x.Summary, x => x.MapFrom(x => new InvoicePdfSummaryVM {
                    NetAmount = x.NetAmount,
                    VatPercent = x.VatPercent,
                    VatAmount = x.VatAmount,
                    GrossAmount = x.GrossAmount
                }))
                .ForMember(x => x.BankAccounts, x => x.MapFrom(x => x.Ship.ShipOwner.BankAccounts.Select(bankAccount => new SimpleEntity {
                    Id = bankAccount.Bank.Id,
                    Description = bankAccount.Bank.Description + " " + bankAccount.Iban
                })));
            CreateMap<Invoice, InvoiceBalanceVM>()
                .ForMember(x => x.PreviousBalance, x => x.MapFrom(x => x.PreviousBalance))
                .ForMember(x => x.NewBalance, x => x.MapFrom(x => x.NewBalance));
        }

    }

}