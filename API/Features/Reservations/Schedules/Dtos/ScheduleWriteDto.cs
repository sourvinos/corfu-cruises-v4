using API.Infrastructure.Interfaces;

namespace API.Features.Reservations.Schedules {

    public class ScheduleWriteDto : IMetadata {

        // PK
        public int Id { get; set; }
        // FKs
        public int DestinationId { get; set; }
        public int PortId { get; set; }
        // Fields
        public string Date { get; set; }
        public int MaxPax { get; set; }
        public string Time { get; set; }
        public bool IsActive { get; set; }
        // Metadata
        public string PostAt { get; set; }
        public string PostUser { get; set; }
        public string PutAt { get; set; }
        public string PutUser { get; set; }

    }

}