using API.Features.Sales.Invoices;
using API.Features.Reservations.Customers;
using API.Features.Reservations.Parameters;
using API.Infrastructure.Helpers;
using API.Infrastructure.Responses;
using AutoMapper;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using RazorLight;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace API.Features.Sales.Receipts {

    public class ReceiptEmailSender : IReceiptEmailSender {

        #region variables

        private readonly EmailInvoiceSettings emailInvoiceSettings;
        private readonly ICustomerRepository customerRepo;
        private readonly IMapper mapper;
        private readonly IReservationParametersRepository parametersRepo;

        #endregion

        public ReceiptEmailSender(ICustomerRepository customerRepo, IOptions<EmailInvoiceSettings> emailSettings, IMapper mapper, IReservationParametersRepository parametersRepo) {
            this.customerRepo = customerRepo;
            this.emailInvoiceSettings = emailSettings.Value;
            this.mapper = mapper;
            this.parametersRepo = parametersRepo;
        }

        #region public methods

        public async Task SendReceiptsToEmail(EmailReceiptVM model) {
            using var smtp = new SmtpClient();
            smtp.Connect(emailInvoiceSettings.SmtpClient, emailInvoiceSettings.Port);
            smtp.Authenticate(emailInvoiceSettings.Username, emailInvoiceSettings.Password);
            await smtp.SendAsync(await BuildReceiptMessage(model));
            smtp.Disconnect(true);
        }

        #endregion

        #region private methods

        private async Task<MimeMessage> BuildReceiptMessage(EmailReceiptVM model) {
            var customer = GetCustomerAsync(model.CustomerId).Result;
            var message = new MimeMessage { Sender = MailboxAddress.Parse(emailInvoiceSettings.Username) };
            message.From.Add(new MailboxAddress(emailInvoiceSettings.From, emailInvoiceSettings.Username));
            message.To.AddRange(BuildReceivers(customer.Email));
            message.Subject = "📧 Ηλεκτρονική αποστολή παραστατικών";
            var builder = new BodyBuilder { HtmlBody = await BuildEmailReceiptTemplate(customer.Email) };
            foreach (var filename in model.Filenames) {
                builder.Attachments.Add(Path.Combine("Reports" + Path.DirectorySeparatorChar + "Invoices" + Path.DirectorySeparatorChar + filename));
            }
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

        private async Task<string> BuildEmailReceiptTemplate(string email) {
            RazorLightEngine engine = new RazorLightEngineBuilder()
                .UseEmbeddedResourcesProject(Assembly.GetEntryAssembly())
                .Build();
            return await engine.CompileRenderStringAsync(
                "key",
                LoadEmailRreceiptTemplateFromFile(),
                new EmailInvoiceTemplateVM {
                    Email = email,
                    CompanyPhones = parametersRepo.GetAsync().Result.Phones,
                });
        }

        private static string LoadEmailRreceiptTemplateFromFile() {
            string FilePath = Directory.GetCurrentDirectory() + "\\Templates\\EmailInvoice.cshtml";
            StreamReader str = new(FilePath);
            string template = str.ReadToEnd();
            str.Close();
            return template;
        }

        private async Task<EmailReceiptCustomerVM> GetCustomerAsync(int id) {
            var x = await customerRepo.GetByIdAsync(id, false);
            if (x != null) {
                return mapper.Map<Customer, EmailReceiptCustomerVM>(x);
            } else {
                throw new CustomException() {
                    ResponseCode = 404
                };
            }
        }

        #endregion

    }

}