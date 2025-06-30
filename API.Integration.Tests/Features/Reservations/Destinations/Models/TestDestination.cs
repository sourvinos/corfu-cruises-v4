using Infrastructure;

namespace Destinations {

    public class TestDestination : ITestEntity {

        public int StatusCode { get; set; }

        public int Id { get; set; }
        public string Description { get; set; }
        public string Abbreviation { get; set; }
        public string PutAt { get; set; }

    }

}