using Infrastructure;

namespace ShipCrews {

    public class TestCrew : ITestEntity {

        public int StatusCode { get; set; }

        public int Id { get; set; }
        public int GenderId { get; set; }
        public int NationalityId { get; set; }
        public int ShipId { get; set; }
        public int SpecialtyId { get; set; }
        public string Lastname { get; set; }
        public string Firstname { get; set; }
        public string Birthdate { get; set; }
        public string PutAt { get; set; }

    }

}