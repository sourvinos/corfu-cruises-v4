using System;
using System.Collections.Generic;
using System.Linq;
using API.Infrastructure.Users;
using API.Infrastructure.Classes;
using API.Infrastructure.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace API.Features.Sales.Invoices {

    public class InvoiceUpdateRepository : Repository<Invoice>, IInvoiceUpdateRepository {

        private readonly TestingEnvironment testingEnvironment;

        public InvoiceUpdateRepository(AppDbContext context, IHttpContextAccessor httpContext, IOptions<TestingEnvironment> testingEnvironment, UserManager<UserExtended> userManager) : base(context, httpContext, testingEnvironment, userManager) {
            this.testingEnvironment = testingEnvironment.Value;
        }

        public Invoice Update(Guid invoiceId, Invoice invoice) {
            using var transaction = context.Database.BeginTransaction();
            UpdateInvoice(invoice);
            DeletePorts(invoiceId, invoice.InvoicesPorts);
            context.SaveChanges();
            DisposeOrCommit(transaction);
            return invoice;
        }

        public InvoiceAade UpdateInvoiceAade(InvoiceAade invoiceAade) {
            using var transaction = context.Database.BeginTransaction();
            context.InvoicesAade.Update(invoiceAade);
            context.SaveChanges();
            DisposeOrCommit(transaction);
            return invoiceAade;
        }

        public InvoiceAade UpdateInvoiceOxygen(InvoiceAade invoiceAade) {
            using var transaction = context.Database.BeginTransaction();
            context.InvoicesAade.Update(invoiceAade);
            context.SaveChanges();
            DisposeOrCommit(transaction);
            return invoiceAade;
        }

        public async Task<int> AttachShipOwnerIdToInvoiceAsync(InvoiceCreateDto invoice) {
            var ship = await context.Ships
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == invoice.ShipId);
            return invoice.ShipOwnerId = ship.ShipOwnerId;
        }

        public void UpdateIsEmailSent(Invoice invoice, string invoiceId) {
            using var transaction = context.Database.BeginTransaction();
            invoice.IsEmailSent = true;
            context.Invoices.Attach(invoice);
            context.Entry(invoice).Property(x => x.IsEmailSent).IsModified = true;
            context.SaveChanges();
            DisposeOrCommit(transaction);
        }

        public void UpdateIsEmailPending(Invoice invoice, string invoiceId) {
            using var transaction = context.Database.BeginTransaction();
            invoice.IsEmailPending = true;
            invoice.IsEmailSent = false;
            context.Invoices.Attach(invoice);
            context.Entry(invoice).Property(x => x.IsEmailPending).IsModified = true;
            context.Entry(invoice).Property(x => x.IsEmailSent).IsModified = true;
            context.SaveChanges();
            DisposeOrCommit(transaction);
        }

        public void UpdateIsCancelled(Invoice invoice, string invoiceId) {
            using var transaction = context.Database.BeginTransaction();
            invoice.IsCancelled = true;
            context.Invoices.Attach(invoice);
            context.Entry(invoice).Property(x => x.IsCancelled).IsModified = true;
            context.SaveChanges();
            DisposeOrCommit(transaction);
        }

        public async Task<int> IncreaseInvoiceNoAsync(InvoiceCreateDto invoice) {
            var lastInvoiceNo = await context.Transactions
                .AsNoTracking()
                .Where(x => x.DocumentTypeId == invoice.DocumentTypeId)
                .OrderBy(x => x.InvoiceNo)
                .Select(x => x.InvoiceNo)
                .LastOrDefaultAsync();
            return lastInvoiceNo += 1;
        }

        private void DisposeOrCommit(IDbContextTransaction transaction) {
            if (testingEnvironment.IsTesting) {
                transaction.Dispose();
            } else {
                transaction.Commit();
            }
        }

        private void UpdateInvoice(Invoice invoice) {
            context.Invoices.Update(invoice);
        }

        private void DeletePorts(Guid invoiceId, List<InvoicePort> ports) {
            var existingPorts = context.InvoicesPorts
                .AsNoTracking()
                .Where(x => x.InvoiceId == invoiceId)
                .ToList();
            var portsToUpdate = ports
                .Where(x => x.Id != 0)
                .ToList();
            var portsToDelete = existingPorts
                .Except(portsToUpdate, new PortComparerById())
                .ToList();
            context.InvoicesPorts.RemoveRange(portsToDelete);
        }

        private class PortComparerById : IEqualityComparer<InvoicePort> {
            public bool Equals(InvoicePort x, InvoicePort y) {
                return x.Id == y.Id;
            }
            public int GetHashCode(InvoicePort x) {
                return x.Id.GetHashCode();
            }
        }

    }

}