namespace Prices {

    public class TestPrice {

        public int StatusCode { get; set; }

        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int DestinationId { get; set; }
        public int PortId { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public decimal AdultsWithTransfer { get; set; }
        public decimal AdultsWithoutTransfer { get; set; }
        public decimal KidsWithTransfer { get; set; }
        public decimal KidsWithoutTransfer { get; set; }
        public string PutAt { get; set; }

    }

}