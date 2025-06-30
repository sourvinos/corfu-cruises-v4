using API.Features.Reservations.Customers;
using AutoMapper;

namespace API.Features.Sales.Invoices {

    public class InvoiceEmailProfile : Profile {

        public InvoiceEmailProfile() {
            CreateMap<Customer, EmailInvoiceCustomerVM>();
        }

    }

}