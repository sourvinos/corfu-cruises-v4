namespace API.Infrastructure.Responses {

    public class ResponseWithBody : IResponse {

        public int Code { get; set; }
        public string Icon { get; set; }
        public object Body { get; set; }
        public string Message { get; set; }
 
    }

}