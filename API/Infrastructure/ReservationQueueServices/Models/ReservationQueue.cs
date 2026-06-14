namespace API.Infrastructure.ReservationQueueServices {

    public class ReservationQueue {

        public int Id { get; set; }
        public string Code { get; set; }
        public string Date { get; set; }
        public int IsImported { get; set; }
        public string PostAt { get; set; }

    }

}