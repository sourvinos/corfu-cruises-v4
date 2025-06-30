using API.Infrastructure.Classes;

namespace API.Features.Sales.Revenues {

    public class RevenuesVM {

        public string Date { get; set; }
        public SimpleEntity Customer { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public decimal PeriodBalance { get; set; }
        public decimal Total { get; set; }

    }

}