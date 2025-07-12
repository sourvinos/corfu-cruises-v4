using API.Features.Reservations.Parameters;
using API.Infrastructure.EmailServices;
using API.Infrastructure.Helpers;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using RazorLight;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace API.Features.Sales.Invoices {

    public class InvoiceEmailSender : IEmailInvoiceSender {

        #region variables

        private readonly EmailInvoiceSettings emailInvoiceSettings;
        private readonly IReservationParametersRepository parametersRepo;

        #endregion

        public InvoiceEmailSender(IOptions<EmailInvoiceSettings> emailInvoiceSettings, IReservationParametersRepository parametersRepo) {
            this.emailInvoiceSettings = emailInvoiceSettings.Value;
            this.parametersRepo = parametersRepo;
        }

        public async Task SendInvoiceToEmail(EmailQueue emailQueue, string email) {
            using var smtp = new SmtpClient();
            smtp.Connect(emailInvoiceSettings.SmtpClient, emailInvoiceSettings.Port);
            smtp.Authenticate(emailInvoiceSettings.Username, emailInvoiceSettings.Password);
            await smtp.SendAsync(await BuildInvoiceMessage(emailQueue, email));
            smtp.Disconnect(true);
        }

        private async Task<MimeMessage> BuildInvoiceMessage(EmailQueue emailQueue, string email) {
            var message = new MimeMessage { Sender = MailboxAddress.Parse(emailInvoiceSettings.Username) };
            message.From.Add(new MailboxAddress(emailInvoiceSettings.From, emailInvoiceSettings.Username));
            message.To.AddRange(BuildReceivers(email));
            message.Subject = "✨ Αποστολή παραστατικών παροχής υπηρεσιών";
            var builder = new BodyBuilder { HtmlBody = await BuildEmailInvoiceTemplate() };
            builder.Attachments.Add(Path.Combine("Reports" + Path.DirectorySeparatorChar + "Invoices" + Path.DirectorySeparatorChar + emailQueue.EntityId.ToString() + ".pdf"));
            message.Body = builder.ToMessageBody();
            return message;
        }

        private static InternetAddressList BuildReceivers(string email) {
            InternetAddressList internetAddressList = new();
            var emails = email.Split(",");
            foreach (string address in emails) {
                internetAddressList.Add(MailboxAddress.Parse(EmailHelpers.BeValidEmailAddress(address.Trim()) ? address.Trim() : "postmaster@appcorfucruises.com"));
            }
            return internetAddressList;
        }

        private async Task<string> BuildEmailInvoiceTemplate() {
            RazorLightEngine engine = new RazorLightEngineBuilder()
                .UseEmbeddedResourcesProject(Assembly.GetEntryAssembly())
                .Build();
            return await engine.CompileRenderStringAsync("key", LoadEmailInvoiceTemplateFromFile(), new EmailInvoiceTemplateVM {
                Email = parametersRepo.GetAsync().Result.Email,
                CompanyPhones = parametersRepo.GetAsync().Result.Phones,
            });
        }

        private static string LoadEmailInvoiceTemplateFromFile() {
            string FilePath = Directory.GetCurrentDirectory() + "\\Templates\\EmailInvoice.cshtml";
            StreamReader str = new(FilePath);
            string template = str.ReadToEnd();
            str.Close();
            return template;
        }

    }

}