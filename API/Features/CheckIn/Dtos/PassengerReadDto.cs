using System;
using API.Infrastructure.Classes;

namespace API.Features.CheckIn {

    public class PassengerReadDto {

        // PK
        public int Id { get; set; }
        // FKs
        public Guid ReservationId { get; set; }
        // Fields
        public string Lastname { get; set; }
        public string Firstname { get; set; }
        public string Birthdate { get; set; }
        public string Remarks { get; set; }
        public string SpecialCare { get; set; }
        // Navigation
        public SimpleEntity Nationality { get; set; }
        public SimpleEntity Gender { get; set; }

    }

}