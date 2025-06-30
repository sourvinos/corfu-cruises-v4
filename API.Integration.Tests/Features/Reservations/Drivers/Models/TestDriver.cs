using Infrastructure;

namespace Drivers {

    public class TestDriver : ITestEntity {

        public int StatusCode { get; set; }

        public int Id { get; set; }
        public string Description { get; set; }
        public string PutAt { get; set; }

    }

}