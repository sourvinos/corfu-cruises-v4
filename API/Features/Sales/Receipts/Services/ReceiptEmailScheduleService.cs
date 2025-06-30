using System;
using System.Threading;
using System.Threading.Tasks;
using API.Infrastructure.Classes;
using API.Infrastructure.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace API.Features.Sales.Receipts {

    public class ReceiptEmailScheduleService : BackgroundService {

        private readonly IReceiptEmailSender receiptEmailSender;
        private readonly IReceiptPdfRepository receiptPdfRepo;
        private readonly IReceiptRepository receiptRepo;
        private readonly IServiceProvider serviceProvider;

        public ReceiptEmailScheduleService(IReceiptEmailSender receiptEmailSender, IReceiptPdfRepository receiptPdfRepo, IReceiptRepository receiptRepo, IServiceProvider serviceProvider) {
            this.receiptEmailSender = receiptEmailSender;
            this.receiptPdfRepo = receiptPdfRepo;
            this.receiptRepo = receiptRepo;
            this.serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
            if (DateHelpers.GetLocalDateTime().Hour >= 5 && DateHelpers.GetLocalDateTime().Hour <= 6) {
                while (!stoppingToken.IsCancellationRequested) {
                    await Task.Delay(TimeSpan.FromSeconds(250), stoppingToken);
                    var x = receiptRepo.GetFirstWithEmailPending();
                    if (x != null) {
                        await receiptEmailSender.SendReceiptsToEmail(BuildVM(x));
                        await PatchReceiptEmailFields(x);
                    }
                }
            }
        }

        private EmailReceiptVM BuildVM(ReceiptPdfVM x) {
            string[] filenames = { receiptPdfRepo.BuildPdf(x) };
            return new EmailReceiptVM {
                CustomerId = x.Customer.Id,
                Filenames = filenames
            };
        }

        private async Task PatchReceiptEmailFields(ReceiptPdfVM receiptVM) {
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var receipt = await receiptRepo.GetByIdForPatchEmailSent(receiptVM.InvoiceId.ToString());
            receipt.IsEmailPending = false;
            receipt.IsEmailSent = true;
            dbContext.Receipts.Attach(receipt);
            dbContext.Entry(receipt).Property(x => x.IsEmailPending).IsModified = true;
            dbContext.Entry(receipt).Property(x => x.IsEmailSent).IsModified = true;
            dbContext.SaveChanges();
        }

    }

}