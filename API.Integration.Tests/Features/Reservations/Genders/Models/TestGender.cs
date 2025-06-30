using Infrastructure;

namespace Genders {

    public class TestGender : ITestEntity {

        public int StatusCode { get; set; }
        
        public int Id { get; set; }
        public string Description { get; set; }
        public string PutAt { get; set; }

    }

}