using API.Infrastructure.Interfaces;

namespace API.Features.Sales.Prices {

    public class PriceWriteDto : IMetadata {

        // PK
        public int Id { get; set; }
        // FKs
        public int CustomerId { get; set; }
        public int DestinationId { get; set; }
        public int PortId { get; set; }
        // Fields
        public string From { get; set; }
        public string To { get; set; }
        public decimal AdultsWithTransfer { get; set; }
        public decimal AdultsWithoutTransfer { get; set; }
        public decimal KidsWithTransfer { get; set; }
        public decimal KidsWithoutTransfer { get; set; }
        // Metadata
        public string PostAt { get; set; }
        public string PostUser { get; set; }
        public string PutAt { get; set; }
        public string PutUser { get; set; }

    }

}