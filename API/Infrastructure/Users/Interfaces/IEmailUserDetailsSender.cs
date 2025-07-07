using System.Threading.Tasks;

namespace API.Infrastructure.Users {

    public interface IEmailUserDetailsSender {

        Task EmailUserDetails(UserExtended user);

    }

}