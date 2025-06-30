using System.Collections.Generic;
using API.Infrastructure.Classes;

namespace API.Features.CheckIn {

    public class CheckInBoardingPassReservationVM {

        public string Date { get; set; }
        public string RefNo { get; set; }
        public string TicketNo { get; set; }
        public SimpleEntity Customer { get; set; }
        public SimpleEntity Destination { get; set; }
        public CheckInBoardingPassPickupPointVM PickupPoint { get; set; }
        public int TotalPax { get; set; }
        public string Email { get; set; }
        public string Phones { get; set; }
        public string Remarks { get; set; }
        public string CompanyPhones { get; set; }
        public string Barcode { get; set; }
        public List<CheckInBoardingPassPassengerVM> Passengers { get; set; }

    }

}