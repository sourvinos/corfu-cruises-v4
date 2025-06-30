using System;
using API.Features.Reservations.Customers;
using API.Features.Reservations.Destinations;
using API.Features.Reservations.Ports;
using API.Infrastructure.Interfaces;

namespace API.Features.Sales.Prices {

    public class Price : IMetadata {

        // PK
        public int Id { get; set; }
        // FKs
        public int CustomerId { get; set; }
        public int DestinationId { get; set; }
        public int PortId { get; set; }
        // Fields
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public decimal AdultsWithTransfer { get; set; }
        public decimal AdultsWithoutTransfer { get; set; }
        public decimal KidsWithTransfer { get; set; }
        public decimal KidsWithoutTransfer { get; set; }
        // Metadata
        public string PostAt { get; set; }
        public string PostUser { get; set; }
        public string PutAt { get; set; }
        public string PutUser { get; set; }
        // Navigation
        public Customer Customer { get; set; }
        public Destination Destination { get; set; }
        public Port Port { get; set; }

    }

}