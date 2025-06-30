using System;
using System.Collections.Generic;

namespace API.Features.CheckIn {

    public class ReservationWriteDto {

        public Guid ReservationId { get; set; }
        public string PutAt { get; set; }
        public List<PassengerWriteDto> Passengers { get; set; }

    }

}