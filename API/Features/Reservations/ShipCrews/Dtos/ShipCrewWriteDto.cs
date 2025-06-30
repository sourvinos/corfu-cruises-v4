using API.Infrastructure.Interfaces;

namespace API.Features.Reservations.ShipCrews {

    public class ShipCrewWriteDto : IMetadata {

        // PK
        public int Id { get; set; }
        // FKs
        public int GenderId { get; set; }
        public int NationalityId { get; set; }
        public int ShipId { get; set; }
        public int SpecialtyId { get; set; }
        // Fields
        public string Lastname { get; set; }
        public string Firstname { get; set; }
        public string Birthdate { get; set; }
        public bool IsActive { get; set; }
        // Metadata
        public string PostAt { get; set; }
        public string PostUser { get; set; }
        public string PutAt { get; set; }
        public string PutUser { get; set; }

    }

}