namespace API.Features.Reservations.Ships {

    public class ShipBrowserVM {

        public int Id { get; set; }
        public string Description { get; set; }
        public ShipOwnerBrowserVM ShipOwner { get; set; }
        public bool IsActive { get; set; }

    }

}