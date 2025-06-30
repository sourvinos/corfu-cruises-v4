using System.Threading.Tasks;

namespace API.Infrastructure.Users {

    public interface IEmailSender {

        Task EmailUserDetails(UserDetailsForEmailVM model);

    }

}