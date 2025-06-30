using API.Infrastructure.Classes;

namespace API.Features.Sales.Invoices {

    public class InvoiceValidateBalanceVM {

        public SimpleEntity Customer { get; set; }
        public decimal BalanceLimit { get; set; }
        public decimal ActualBalance { get; set; }
        public decimal MaxAllowed { get; set; }

    }

}