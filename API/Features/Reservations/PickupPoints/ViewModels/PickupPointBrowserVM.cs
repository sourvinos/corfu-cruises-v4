using API.Infrastructure.Classes;

namespace API.Features.Reservations.PickupPoints {

    public class PickupPointBrowserVM {

        public int Id { get; set; }
        public string Description { get; set; }
        public string ExactPoint { get; set; }
        public string Time { get; set; }
        public SimpleEntity Port { get; set; }
        public bool IsActive { get; set; }

    }

}