using System;
using API.Features.Reservations.Genders;
using API.Features.Reservations.Nationalities;
using API.Features.Reservations.Occupants;

namespace API.Features.Reservations.Reservations {

    public class Passenger {

        // PK
        public int Id { get; set; }
        // FKs
        public Guid ReservationId { get; set; }
        public int GenderId { get; set; }
        public int NationalityId { get; set; }
        public int OccupantId { get; set; }
        // Fields
        public string Lastname { get; set; }
        public string Firstname { get; set; }
        public DateTime Birthdate { get; set; }
        public string Remarks { get; set; }
        public string SpecialCare { get; set; }
        public bool IsBoarded { get; set; }
        // Navigation
        public Gender Gender { get; set; }
        public Nationality Nationality { get; set; }
        public Occupant Occupant { get; set; }
        public virtual Reservation Reservation { get; set; }

    }

}