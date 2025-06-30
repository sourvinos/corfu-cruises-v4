namespace API.Features.Reservations.Boarding {

    public class BoardingCriteria {

        public string Date { get; set; }
        public int[] DestinationIds { get; set; }
        public int[] PortIds { get; set; }
        public int?[] ShipIds { get; set; }

    }

}