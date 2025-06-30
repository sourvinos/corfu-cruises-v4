namespace API.Features.Reservations.Boarding {

    public class BoardingFinalPassengerVM {

        public int Id { get; set; }
        public string Lastname { get; set; }
        public string Firstname { get; set; }
        public bool IsBoarded { get; set; }

        public BoardingFinalPassengerNationalityVM Nationality { get; set; }

    }

}