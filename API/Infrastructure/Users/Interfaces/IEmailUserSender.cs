using System.Threading.Tasks;

namespace API.Infrastructure.Users {

    public interface IEmailUserSender {

        Task EmailUserDetails(UserDetailsForEmailVM model);

    }

}