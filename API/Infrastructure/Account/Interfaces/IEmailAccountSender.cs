using System.Threading.Tasks;

namespace API.Infrastructure.Account {

    public interface IEmailAccountSender {

        Task EmailForgotPassword(string username, string displayname, string email, string callbackUrl);

    }

}