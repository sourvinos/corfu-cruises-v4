using System.Linq;
using API.Infrastructure.Classes;
using API.Infrastructure.Helpers;
using AutoMapper;

namespace API.Features.Sales.Receipts {

    public class ReceiptPdfMappingProfile : Profile {

        public ReceiptPdfMappingProfile() {
            CreateMap<Receipt, ReceiptPdfVM>()
                .ForMember(x => x.Header, x => x.MapFrom(x => new ReceiptPdfHeaderVM {
                    Date = DateHelpers.DateToISOString(x.Date),
                    TripDate = DateHelpers.DateToISOString(x.TripDate),
                    DocumentType = new ReceiptPdfDocumentTypeVM {
                        Description = x.DocumentType.Description,
                        Batch = x.DocumentType.Batch
                    },
                    InvoiceNo = x.InvoiceNo
                }))
                .ForMember(x => x.Customer, x => x.MapFrom(x => new ReceiptPdfPartyVM {
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
                .ForMember(x => x.DocumentType, x => x.MapFrom(x => new ReceiptPdfDocumentTypeVM {
                    Description = x.DocumentType.Description,
                    Batch = x.DocumentType.Batch
                }))
                .ForMember(x => x.Issuer, x => x.MapFrom(x => new ReceiptPdfPartyVM {
                    FullDescription = x.ShipOwner.Description,
                    VatNumber = x.ShipOwner.VatNumber,
                    Branch = x.ShipOwner.Branch,
                    Profession = x.ShipOwner.Profession,
                    Street = x.ShipOwner.Street,
                    Number = x.ShipOwner.Number,
                    PostalCode = x.ShipOwner.PostalCode,
                    City = x.ShipOwner.City,
                    Phones = x.ShipOwner.Phones,
                    Email = x.ShipOwner.Email,
                    Nationality = x.ShipOwner.Nationality.Description,
                    TaxOffice = x.ShipOwner.TaxOffice.Description
                }))
                .ForMember(x => x.PaymentMethod, x => x.MapFrom(x => x.PaymentMethod.Description))
                .ForMember(x => x.Summary, x => x.MapFrom(x => new InvoicePdfSummaryVM {
                    NetAmount = x.NetAmount,
                    VatPercent = x.VatPercent,
                    VatAmount = x.VatAmount,
                    GrossAmount = x.GrossAmount
                }))
                .ForMember(x => x.BankAccounts, x => x.MapFrom(x => x.ShipOwner.BankAccounts.Select(bankAccount => new SimpleEntity {
                    Id = bankAccount.Bank.Id,
                    Description = bankAccount.Bank.Description + " " + bankAccount.Iban
                })));
            CreateMap<Receipt, ReceiptBalanceVM>()
                .ForMember(x => x.PreviousBalance, x => x.MapFrom(x => x.PreviousBalance))
                .ForMember(x => x.NewBalance, x => x.MapFrom(x => x.NewBalance));
        }

    }

}