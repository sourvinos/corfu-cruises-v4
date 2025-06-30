using API.Features.Reservations.Customers;
using AutoMapper;

namespace API.Features.Sales.Receipts {

    public class ReceiptEmailProfile : Profile {

        public ReceiptEmailProfile() {
            CreateMap<Customer, EmailReceiptCustomerVM>();
        }

    }

}