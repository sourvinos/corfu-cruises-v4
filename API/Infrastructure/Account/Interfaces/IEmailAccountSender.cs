using System.Threading.Tasks;
using API.Infrastructure.Users;

namespace API.Infrastructure.Account {

    public interface IEmailAccountSender {

        Task SendForgotPasswordEmail(string username, string displayname, string email, string callbackUrl, string subject);

    }

}