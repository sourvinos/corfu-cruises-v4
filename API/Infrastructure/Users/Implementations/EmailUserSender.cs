using API.Features.Reservations.Parameters;
using API.Infrastructure.Helpers;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using RazorLight;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace API.Infrastructure.Users {

    public class EmailUserSender : IEmailSender {

        private readonly EmailUserSettings emailUserSettings;
        private readonly IReservationParametersRepository parametersRepo;

        public EmailUserSender(IReservationParametersRepository parametersRepo, IOptions<EmailUserSettings> emailUserSettings) {
            this.emailUserSettings = emailUserSettings.Value;
            this.parametersRepo = parametersRepo;
        }

        public async Task EmailUserDetails(UserDetailsForEmailVM model) {
            using var smtp = new SmtpClient();
            smtp.Connect(emailUserSettings.SmtpClient, emailUserSettings.Port);
            smtp.Authenticate(emailUserSettings.Username, emailUserSettings.Password);
            await smtp.SendAsync(await BuildMessage(model));
            smtp.Disconnect(true);
        }

        private async Task<MimeMessage> BuildMessage(UserDetailsForEmailVM model) {
            var message = new MimeMessage { Sender = MailboxAddress.Parse(emailUserSettings.Username) };
            message.From.Add(new MailboxAddress(emailUserSettings.From, emailUserSettings.Username));
            message.To.Add(MailboxAddress.Parse(model.Email));
            message.Subject = model.Subject;
            message.Body = new BodyBuilder { HtmlBody = await BuildTemplate(model) }.ToMessageBody();
            return message;
        }

        private async Task<string> BuildTemplate(UserDetailsForEmailVM model) {
            RazorLightEngine engine = new RazorLightEngineBuilder()
                .UseEmbeddedResourcesProject(Assembly.GetEntryAssembly())
                .Build();
            return await engine.CompileRenderStringAsync(
                "key",
                LoadNewUserEmailTemplateFromFile(),
                new UserDetailsForEmailVM {
                    Username = model.Username,
                    Displayname = model.Displayname,
                    Email = model.Email,
                    Url = model.Url,
                    CompanyPhones = parametersRepo.GetAsync().Result.Phones
                });
        }

        private static string LoadNewUserEmailTemplateFromFile() {
            string FilePath = Directory.GetCurrentDirectory() + "\\Templates\\UserDetailsForEmail.cshtml";
            StreamReader str = new(FilePath);
            string template = str.ReadToEnd();
            str.Close();
            return template;
        }

     }

}