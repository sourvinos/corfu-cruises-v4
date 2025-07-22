using System;

namespace API.Infrastructure.EmailServices {

    public class EmailQueue {

        public int Id { get; set; }
        public string Initiator { get; set; }
        public Guid EntityId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int? CustomerId { get; set; }
        public byte Priority { get; set; }
        public bool IsSent { get; set; }
        public string PostAt { get; set; }

    }

}