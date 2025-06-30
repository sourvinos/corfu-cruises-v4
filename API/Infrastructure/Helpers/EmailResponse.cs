namespace API.Infrastructure.Helpers {

    public class EmailResponse {

        public bool Successful => ErrorMsg == null;
        public string ErrorMsg;

    }

}