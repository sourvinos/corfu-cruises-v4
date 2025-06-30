using System;

namespace API.Features.CheckIn {

    public class PassengerWriteDto {

        // PK
        public int Id { get; set; }
        // FKs
        public Guid? ReservationId { get; set; }
        public int GenderId { get; set; }
        public int NationalityId { get; set; }
        // Fields
        public string Lastname { get; set; }
        public string Firstname { get; set; }
        public string Birthdate { get; set; }
        public string SpecialCare { get; set; }
        public string Remarks { get; set; }

    }

}