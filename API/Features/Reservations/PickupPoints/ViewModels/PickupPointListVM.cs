namespace API.Features.Reservations.PickupPoints {

    public class PickupPointListVM {

        public int Id { get; set; }
        public string Description { get; set; }
        public string LinkTwistAlias { get; set; }
        public PickupPointListCoachRouteVM CoachRoute { get; set; }
        public PickupPointListPortVM Port { get; set; }
        public string ExactPoint { get; set; }
        public string Time { get; set; }
        public string Remarks { get; set; }
        public string PutAt { get; set; }
        public bool IsActive { get; set; }

    }

}