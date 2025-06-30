using API.Infrastructure.Classes;
using API.Infrastructure.Implementations;
using API.Infrastructure.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Linq;

namespace API.Features.Sales.Invoices {

    public class InvoiceCalculateBalanceRepo : Repository<Invoice>, IInvoiceCalculateBalanceRepo {

        public InvoiceCalculateBalanceRepo(AppDbContext appDbContext, IHttpContextAccessor httpContext, IOptions<TestingEnvironment> settings, UserManager<UserExtended> userManager) : base(appDbContext, httpContext, settings, userManager) { }

        public InvoiceBalanceVM CalculateBalances(InvoiceCreateDto invoice, int customerId, int shipOwnerId) {
            decimal previousBalance = CalculatePreviousBalance(customerId, shipOwnerId);
            decimal newAmount = DetermineDebitOrCreditForNewRecord(invoice);
            return new InvoiceBalanceVM {
                PreviousBalance = previousBalance,
                NewBalance = previousBalance + newAmount
            };
        }

        public InvoiceCreateDto AttachBalancesToCreateDto(InvoiceCreateDto invoice, InvoiceBalanceVM balances) {
            invoice.PreviousBalance = balances.PreviousBalance;
            invoice.NewBalance = balances.NewBalance;
            return invoice;
        }

        public decimal CalculatePreviousBalance(int customerId, int shipOwnerId) {
            var records = context.Transactions
                .AsNoTracking()
                .Include(x => x.Customer)
                .Include(x => x.DocumentType)
                .Where(x => x.CustomerId == customerId && x.ShipOwnerId == shipOwnerId && !x.IsCancelled)
                .OrderBy(x => x.Date)
                .ToList();
            decimal previousBalance = 0;
            decimal debit = 0;
            decimal credit = 0;
            foreach (var record in records) {
                debit = (record.DocumentType.Customers == "+" || record.DocumentType.Suppliers == "-") ? record.GrossAmount : 0;
                credit = (record.DocumentType.Customers == "-" || record.DocumentType.Suppliers == "+") ? record.GrossAmount : 0;
                previousBalance += debit - credit;
            }
            return previousBalance;
        }

        public decimal ValidateCreditLimit(int customerId) {
            var records = context.Transactions
                .AsNoTracking()
                .Include(x => x.Customer)
                .Include(x => x.DocumentType)
                .Where(x => x.CustomerId == customerId && !x.IsCancelled)
                .OrderBy(x => x.Date)
                .ToList();
            decimal balance = 0;
            decimal debit = 0;
            decimal credit = 0;
            foreach (var record in records) {
                debit = (record.DocumentType.Customers == "+" || record.DocumentType.Suppliers == "-") ? record.GrossAmount : 0;
                credit = (record.DocumentType.Customers == "-" || record.DocumentType.Suppliers == "+") ? record.GrossAmount : 0;
                balance += debit - credit;
            }
            return balance;
        }

        private decimal DetermineDebitOrCreditForNewRecord(InvoiceCreateDto invoice) {
            var documentType = context.DocumentTypes.Where(x => x.Id == invoice.DocumentTypeId).SingleOrDefaultAsync().Result;
            decimal debit = (documentType.Customers == "+" || documentType.Suppliers == "-") ? invoice.GrossAmount : 0;
            decimal credit = (documentType.Customers == "-" || documentType.Suppliers == "+") ? invoice.GrossAmount : 0;
            return debit - credit;
        }

    }

}