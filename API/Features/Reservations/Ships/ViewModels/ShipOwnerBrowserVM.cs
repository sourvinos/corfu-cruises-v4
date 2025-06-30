namespace API.Features.Reservations.Ships {

    public class ShipOwnerBrowserVM {

        public int Id { get; set; }
        public string Description { get; set; }
        public bool IsMyData { get; set; }
        public bool IsOxygen { get; set; }
        public decimal VatPercent { get; set; }

    }

}