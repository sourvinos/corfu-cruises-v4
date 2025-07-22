using System;

namespace API.Infrastructure.EmailServices {

    public class EmailQueueDto {

        public string Initiator { get; set; }
        public Guid EntityId { get; set; }
        public string? FromDate { get; set; }
        public string? ToDate { get; set; }
        public int? CustomerId { get; set; }
        public byte Priority { get; set; }
        public bool IsSent { get; set; }
        public string PostAt { get; set; }

    }

}