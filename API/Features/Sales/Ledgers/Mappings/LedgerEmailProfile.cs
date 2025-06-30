using API.Features.Reservations.Customers;
using AutoMapper;

namespace API.Features.Sales.Ledgers {

    public class LedgerEmailProfile : Profile {

        public LedgerEmailProfile() {
            CreateMap<Customer, EmailLedgerCustomerVM>();
        }

    }

}