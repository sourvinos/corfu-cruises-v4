namespace API.Infrastructure.Users {

    public class UserListVM {

        public string Id { get; set; }
        public string Username { get; set; }
        public string Displayname { get; set; }
        public string Email { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsActive { get; set; }

    }

}