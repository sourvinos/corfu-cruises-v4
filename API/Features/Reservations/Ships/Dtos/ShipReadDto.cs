using API.Infrastructure.Classes;
using API.Infrastructure.Interfaces;

namespace API.Features.Reservations.Ships {

    public class ShipReadDto : IBaseEntity, IMetadata {

        public int Id { get; set; }
        public string Abbreviation { get; set; }
        public string Description { get; set; }
        public string RegistryNo { get; set; }
        public bool IsShownInCriteria { get; set; }
        public bool IsActive { get; set; }
        public string PostAt { get; set; }
        public string PostUser { get; set; }
        public string PutAt { get; set; }
        public string PutUser { get; set; }
        public SimpleEntity ShipOwner { get; set; }

    }

}