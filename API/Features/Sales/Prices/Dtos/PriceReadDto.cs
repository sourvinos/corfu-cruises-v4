using API.Infrastructure.Classes;
using API.Infrastructure.Interfaces;

namespace API.Features.Sales.Prices {

    public class PriceReadDto : IMetadata {

        // PK
        public int Id { get; set; }
        // Fields
        public string From { get; set; }
        public string To { get; set; }
        public decimal AdultsWithTransfer { get; set; }
        public decimal AdultsWithoutTransfer { get; set; }
        public decimal KidsWithTransfer { get; set; }
        public decimal KidsWithoutTransfer { get; set; }
        // Navigation
        public SimpleEntity Customer { get; set; }
        public SimpleEntity Destination { get; set; }
        public SimpleEntity Port { get; set; }
        // Metadata
        public string PostAt { get; set; }
        public string PostUser { get; set; }
        public string PutAt { get; set; }
        public string PutUser { get; set; }

    }

}