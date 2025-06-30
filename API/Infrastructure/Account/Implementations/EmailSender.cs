using API.Features.Reservations.Parameters;
using API.Infrastructure.Helpers;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using RazorLight;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace API.Infrastructure.Account {

    public class EmailAccountSender : IEmailAccountSender {

        private readonly EmailUserSettings emailUserSettings;
        private readonly IReservationParametersRepository parametersRepo;

        public EmailAccountSender(IOptions<EmailUserSettings> emailUserSettings, IReservationParametersRepository parametersRepo) {
            this.emailUserSettings = emailUserSettings.Value;
            this.parametersRepo = parametersRepo;
        }

        public async Task SendForgotPasswordEmail(string username, string displayname, string email, string returnUrl, string subject) {
            using var smtp = new SmtpClient();
            smtp.Connect(emailUserSettings.SmtpClient, emailUserSettings.Port);
            smtp.Authenticate(emailUserSettings.Username, emailUserSettings.Password);
            await smtp.SendAsync(await BuildMessage(username, displayname, email, subject, returnUrl));
            smtp.Disconnect(true);
        }

        private async Task<MimeMessage> BuildMessage(string username, string displayname, string email, string subject, string returnUrl) {
            var message = new MimeMessage { Sender = MailboxAddress.Parse(emailUserSettings.Username) };
            message.From.Add(new MailboxAddress(emailUserSettings.From, emailUserSettings.Username));
            message.To.Add(MailboxAddress.Parse(email));
            message.Subject = subject;
            message.Body = new BodyBuilder { HtmlBody = await BuildForgotPasswordTemplate(emailUserSettings.From, username, displayname, email, returnUrl) }.ToMessageBody();
            return message;
        }

        private async Task<string> BuildForgotPasswordTemplate(string logo, string username, string displayname, string email, string returnUrl) {
            RazorLightEngine engine = new RazorLightEngineBuilder()
                .UseEmbeddedResourcesProject(Assembly.GetEntryAssembly())
                .Build();
            return await engine.CompileRenderStringAsync(
                "key",
                LoadForgotPasswordTemplateFromFile(),
                new ForgotPasswordResponseVM {
                    Logo = logo,
                    Username = username,
                    Displayname = displayname,
                    Email = email,
                    ReturnUrl = returnUrl,
                    CompanyPhones = parametersRepo.GetAsync().Result.Phones,
                });
        }

        private static string LoadForgotPasswordTemplateFromFile() {
            string FilePath = Directory.GetCurrentDirectory() + "\\Templates\\ResetPassword.cshtml";
            StreamReader str = new(FilePath);
            string template = str.ReadToEnd();
            str.Close();
            return template;
        }

    }

}