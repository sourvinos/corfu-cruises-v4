using API.Infrastructure.Helpers;
using AutoMapper;

namespace API.Features.Sales.Invoices {

    public class InvoiceXmlMappingProfile : Profile {

        public InvoiceXmlMappingProfile() {
            CreateMap<Invoice, XmlBuilderVM>()
                .ForMember(x => x.Credentials, x => x.MapFrom(x => new XmlCredentialsVM {
                    Username = x.Ship.ShipOwner.MyDataIsDemo ? x.Ship.ShipOwner.MyDataDemoUsername : x.Ship.ShipOwner.MyDataLiveUsername,
                    SubscriptionKey = x.Ship.ShipOwner.MyDataIsDemo ? x.Ship.ShipOwner.MyDataDemoSubscriptionKey : x.Ship.ShipOwner.MyDataLiveSubscriptionKey,
                    Url = x.Ship.ShipOwner.MyDataIsDemo ? x.Ship.ShipOwner.MyDataDemoUrl : x.Ship.ShipOwner.MyDataLiveUrl
                }))
                .ForMember(x => x.Issuer, x => x.MapFrom(x => new XmlIssuerVM {
                    VatNumber = x.Ship.ShipOwner.VatNumber,
                    Country = x.Ship.ShipOwner.Nationality.Code,
                    Branch = x.Ship.ShipOwner.Branch,
                    Address = new XmlAddressVM {
                        Street = x.Ship.ShipOwner.Street,
                        Number = x.Ship.ShipOwner.Number,
                        PostalCode = x.Ship.ShipOwner.PostalCode,
                        City = x.Ship.ShipOwner.City
                    }
                }))
                .ForMember(x => x.CounterPart, x => x.MapFrom(x => new XmlCounterPartVM {
                    VatNumber = x.Customer.VatNumber,
                    Country = x.Customer.Nationality.Code,
                    Branch = x.Customer.Branch,
                    Name = x.Customer.FullDescription,
                    Address = new XmlAddressVM {
                        Street = x.Customer.Street,
                        Number = x.Customer.Number,
                        PostalCode = x.Customer.PostalCode,
                        City = x.Customer.City
                    }
                }))
                .ForMember(x => x.InvoiceHeader, x => x.MapFrom(x => new XmlHeaderVM {
                    Series = x.DocumentType.Batch,
                    Aa = x.InvoiceNo.ToString(),
                    IssueDate = DateHelpers.DateToISOString(x.Date),
                    InvoiceType = x.DocumentType.Table8_1,
                    Currency = "EUR"
                }))
                .ForMember(x => x.PaymentMethod, x => x.MapFrom(x => new XmlPaymentMethodVM {
                    PaymentMethodDetail = new XmlPaymentMethodDetailVM {
                        Type = x.PaymentMethod.MyDataId,
                        Amount = x.GrossAmount
                    }
                }))
                .ForMember(x => x.InvoiceDetail, x => x.MapFrom(x => new XmlRowVM {
                    LineNumber = 1,
                    NetValue = x.NetAmount,
                    VatCategory = x.Customer.VatPercentId,
                    VatAmount = x.VatAmount,
                    VatExemptionCategory = x.Customer.VatExemptionId
                }))
                .ForMember(x => x.InvoiceSummary, x => x.MapFrom(x => new XmlSummaryVM {
                    TotalNetValue = x.NetAmount,
                    TotalVatAmount = x.VatAmount,
                    TotalWithheldAmount = 0,
                    TotalFeesAmount = 0,
                    TotalStampDutyAmount = 0,
                    TotalOtherTaxesAmount = 0,
                    TotalDeductionsAmount = 0,
                    TotalGrossValue = x.GrossAmount,
                    IncomeClassification = new XmlIncomeClassificationVM {
                        ClassificationType = x.DocumentType.Table8_9,
                        ClassificationCategory = x.DocumentType.Table8_8,
                        Amount = x.NetAmount
                    }
                }))
                .ForMember(x => x.Aade, x => x.MapFrom(x => new XmlAadeVM {
                    InvoiceId = x.InvoiceId,
                    UId = x.Aade.Uid,
                    Mark = x.Aade.Mark,
                    MarkCancel = x.Aade.MarkCancel,
                    Url = x.Aade.Url
                }));
        }
    }

}