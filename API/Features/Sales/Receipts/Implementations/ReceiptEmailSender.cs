using API.Features.Reservations.Parameters;
using API.Infrastructure.Helpers;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using RazorLight;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using API.Infrastructure.EmailServices;

namespace API.Features.Sales.Receipts {

    public class ReceiptEmailSender : IReceiptEmailSender {

        #region variables

        private readonly EmailInvoiceSettings emailReceiptSettings;
        private readonly IReservationParametersRepository parametersRepo;

        #endregion

        public ReceiptEmailSender(IOptions<EmailInvoiceSettings> emailReceiptSettings, IReservationParametersRepository parametersRepo) {
            this.emailReceiptSettings = emailReceiptSettings.Value;
            this.parametersRepo = parametersRepo;
        }

        public async Task SendReceiptToEmail(EmailQueue emailQueue, string email) {
            using var smtp = new SmtpClient();
            smtp.Connect(emailReceiptSettings.SmtpClient, emailReceiptSettings.Port);
            smtp.Authenticate(emailReceiptSettings.Username, emailReceiptSettings.Password);
            await smtp.SendAsync(await BuildReceiptMessage(emailQueue, email));
            smtp.Disconnect(true);
        }

        private async Task<MimeMessage> BuildReceiptMessage(EmailQueue emailQueue, string email) {
            var message = new MimeMessage { Sender = MailboxAddress.Parse(emailReceiptSettings.Username) };
            message.From.Add(new MailboxAddress(emailReceiptSettings.From, emailReceiptSettings.Username));
            message.To.AddRange(BuildReceivers(email));
            message.Subject = "✨ Αποστολή αποδείξεων είσπραξης";
            var builder = new BodyBuilder { HtmlBody = await BuildEmailReceiptTemplate() };
            builder.Attachments.Add(Path.Combine("Reports" + Path.DirectorySeparatorChar + "Invoices" + Path.DirectorySeparatorChar + emailQueue.EntityId.ToString() + ".pdf"));
            message.Body = builder.ToMessageBody();
            return message;
        }

        private static InternetAddressList BuildReceivers(string email) {
            InternetAddressList x = new();
            var emails = email.Split(",");
            foreach (string address in emails) {
                x.Add(MailboxAddress.Parse(EmailHelpers.BeValidEmailAddress(address.Trim()) ? address.Trim() : "postmaster@appcorfucruises.com"));
            }
            return x;
        }

        private async Task<string> BuildEmailReceiptTemplate() {
            RazorLightEngine engine = new RazorLightEngineBuilder()
                .UseEmbeddedResourcesProject(Assembly.GetEntryAssembly())
                .Build();
            return await engine.CompileRenderStringAsync("key", LoadEmailReceiptTemplateFromFile(), new EmailReceiptTemplateVM {
                Email = parametersRepo.GetAsync().Result.Email,
                CompanyPhones = parametersRepo.GetAsync().Result.Phones,
            });
        }

        private static string LoadEmailReceiptTemplateFromFile() {
            string FilePath = Directory.GetCurrentDirectory() + "\\Templates\\EmailReceipt.cshtml";
            StreamReader str = new(FilePath);
            string template = str.ReadToEnd();
            str.Close();
            return template;
        }

    }

}