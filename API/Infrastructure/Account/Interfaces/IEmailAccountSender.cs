using System.Threading.Tasks;

namespace API.Infrastructure.Account {

    public interface IEmailAccountSender {

        Task SendForgotPasswordEmail(string username, string displayname, string email, string callbackUrl, string subject);

    }

}