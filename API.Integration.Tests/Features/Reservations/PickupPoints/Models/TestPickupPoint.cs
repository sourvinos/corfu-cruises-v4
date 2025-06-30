using Infrastructure;

namespace PickupPoints {

    public class TestPickupPoint : ITestEntity {

        public int StatusCode { get; set; }

        public int Id { get; set; }
        public int CoachRouteId { get; set; }
        public int PortId { get; set; }
        public string Description { get; set; }
        public string ExactPoint { get; set; }
        public string Time { get; set; }
        public bool IsActive { get; set; }
        public string PutAt { get; set; }

    }

}