using System;
using API.Features.Reservations.CrewSpecialties;
using API.Features.Reservations.Genders;
using API.Features.Reservations.Nationalities;
using API.Features.Reservations.Occupants;
using API.Features.Reservations.Ships;
using API.Infrastructure.Interfaces;

namespace API.Features.Reservations.ShipCrews {

    public class ShipCrew : IMetadata {

        // PK
        public int Id { get; set; }
        // FKs
        public int GenderId { get; set; }
        public int NationalityId { get; set; }
        public int OccupantId { get; set; }
        public int ShipId { get; set; }
        public int SpecialtyId { get; set; }
        // Fields
        public string Lastname { get; set; }
        public string Firstname { get; set; }
        public DateTime Birthdate { get; set; }
        public bool IsActive { get; set; }
        // Metadata
        public string PostAt { get; set; }
        public string PostUser { get; set; }
        public string PutAt { get; set; }
        public string PutUser { get; set; }
        // Navigation
        public Gender Gender { get; set; }
        public Nationality Nationality { get; set; }
        public Occupant Occupant { get; set; }
        public Ship Ship { get; set; }
        public CrewSpecialty Specialty { get; set; }

    }

}