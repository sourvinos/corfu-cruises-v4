using System;

namespace API.Infrastructure.EmailServices {

    public class EmailQueue {

        public int Id { get; set; }
        public string Initiator { get; set; }
        public Guid EntityId { get; set; }
        public byte Priority { get; set; }
        public bool IsCompleted { get; set; }
        public string PostAt { get; set; }

    }

}