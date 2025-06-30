using API.Infrastructure.Classes;

namespace API.Features.Sales.Prices {

    public class PriceListVM {

        public int Id { get; set; }
        public SimpleEntity Customer { get; set; }
        public SimpleEntity Destination { get; set; }
        public SimpleEntity Port { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public decimal AdultsWithTransfer { get; set; }
        public decimal AdultsWithoutTransfer { get; set; }
        public decimal KidsWithTransfer { get; set; }
        public decimal KidsWithoutTransfer { get; set; }

    }

}