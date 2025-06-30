using API.Infrastructure.Interfaces;

namespace API.Features.Reservations.Ships {

    public class ShipWriteDto : IBaseEntity, IMetadata {

        // PK
        public int Id { get; set; }
        // FKs
        public int ShipOwnerId { get; set; }
        // Fields
        public string Abbreviation { get; set; }
        public string Description { get; set; }
        public string RegistryNo { get; set; }
        public bool IsActive { get; set; }
        // Metadata
        public string PostAt { get; set; }
        public string PostUser { get; set; }
        public string PutAt { get; set; }
        public string PutUser { get; set; }

    }

}