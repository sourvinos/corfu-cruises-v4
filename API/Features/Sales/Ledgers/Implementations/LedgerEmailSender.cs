using API.Features.Reservations.Customers;
using API.Features.Reservations.Parameters;
using API.Infrastructure.Helpers;
using API.Infrastructure.Responses;
using AutoMapper;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MimeKit;
using RazorLight;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace API.Features.Sales.Ledgers {

    public class LedgerEmailSender : ILedgerEmailSender {

        #region variables

        private readonly EmailInvoiceSettings emailInvoiceSettings;
        private readonly ICustomerRepository customerRepo;
        private readonly IMapper mapper;
        private readonly IReservationParametersRepository parametersRepo;

        #endregion

        public LedgerEmailSender(ICustomerRepository customerRepo, IOptions<EmailInvoiceSettings> emailSettings, IMapper mapper, IReservationParametersRepository parametersRepo) {
            this.customerRepo = customerRepo;
            this.emailInvoiceSettings = emailSettings.Value;
            this.mapper = mapper;
            this.parametersRepo = parametersRepo;
        }

        #region public methods

        public async Task SendLedgerToEmail(EmailLedgerVM model) {
            using var smtp = new SmtpClient();
            smtp.Connect(emailInvoiceSettings.SmtpClient, emailInvoiceSettings.Port);
            smtp.Authenticate(emailInvoiceSettings.Username, emailInvoiceSettings.Password);
            await smtp.SendAsync(await BuildLedgerMessage(model));
            smtp.Disconnect(true);
        }

        public FileStreamResult OpenPdf(string filename) {
            var fullpathname = Path.Combine("Reports" + Path.DirectorySeparatorChar + "Ledgers" + Path.DirectorySeparatorChar + filename);
            byte[] byteArray = File.ReadAllBytes(fullpathname);
            MemoryStream memoryStream = new(byteArray);
            return new FileStreamResult(memoryStream, "application/pdf");
        }

        #endregion

        #region private methods

        private async Task<MimeMessage> BuildLedgerMessage(EmailLedgerVM model) {
            var customer = GetCustomerAsync(model.CustomerId).Result;
            var message = new MimeMessage { Sender = MailboxAddress.Parse(emailInvoiceSettings.Username) };
            message.From.Add(new MailboxAddress(emailInvoiceSettings.From, emailInvoiceSettings.Username));
            message.To.Add(MailboxAddress.Parse(customer.Email));
            message.Subject = "📧 Λογιστική καρτέλα και ανάλυση λογαριασμού";
            var builder = new BodyBuilder { HtmlBody = await BuildEmailLedgerTemplate(customer.Email) };
            foreach (var filename in model.Filenames) {
                builder.Attachments.Add(Path.Combine("Reports" + Path.DirectorySeparatorChar + "Ledgers" + Path.DirectorySeparatorChar + filename));
            }
            message.Body = builder.ToMessageBody();
            return message;
        }

        private async Task<string> BuildEmailLedgerTemplate(string email) {
            RazorLightEngine engine = new RazorLightEngineBuilder()
                .UseEmbeddedResourcesProject(Assembly.GetEntryAssembly())
                .Build();
            return await engine.CompileRenderStringAsync(
                "key",
                LoadEmailLedgerTemplateFromFile(),
                new EmailLedgerTemplateVM {
                    Email = email,
                    CompanyPhones = parametersRepo.GetAsync().Result.Phones,
                });
        }

        private static string LoadEmailLedgerTemplateFromFile() {
            string FilePath = Directory.GetCurrentDirectory() + "\\Templates\\EmailLedger.cshtml";
            StreamReader str = new(FilePath);
            string template = str.ReadToEnd();
            str.Close();
            return template;
        }

        private async Task<EmailLedgerCustomerVM> GetCustomerAsync(int id) {
            var x = await customerRepo.GetByIdAsync(id, false);
            if (x != null) {
                return mapper.Map<Customer, EmailLedgerCustomerVM>(x);
            } else {
                throw new CustomException() {
                    ResponseCode = 404
                };
            }
        }

        #endregion

    }

}