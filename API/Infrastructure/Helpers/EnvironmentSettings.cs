namespace API.Infrastructure.Helpers {

    public class EnvironmentSettings {

        public string BaseUrl { get; set; }
        public int EmailSecondsDelay { get; set; }
        public int ReservationsUpdateQueueSecondsDelay { get; set; }
        public int ReservationsProcessQueueSecondsDelay { get; set; }

    }

}