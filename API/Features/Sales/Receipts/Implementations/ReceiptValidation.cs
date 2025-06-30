using System;
using API.Infrastructure.Users;
using API.Infrastructure.Classes;
using API.Infrastructure.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using API.Infrastructure.Helpers;
using System.Linq;

namespace API.Features.Sales.Receipts {

    public class ReceiptValidation : Repository<Receipt>, IReceiptValidation {

        public ReceiptValidation(AppDbContext context, IHttpContextAccessor httpContext, IOptions<TestingEnvironment> testingEnvironment, UserManager<UserExtended> userManager) : base(context, httpContext, testingEnvironment, userManager) { }

        public async Task<int> IsValidAsync(Receipt z, ReceiptWriteDto receipt) {
            return true switch {
                var x when x == !IsValidIssueDate(receipt) => 405,
                var x when x == !await IsCompositeKeyValidAsync(receipt) => 466,
                var x when x == !await IsReceiptCountEqualToLastReceiptNo(receipt) => 467,
                var x when x == !await IsValidCustomer(receipt) => 450,
                var x when x == !await IsValidDocumentType(receipt) => 465,
                var x when x == !await IsValidShipOwner(receipt) => 449,
                var x when x == IsAlreadyUpdated(z, receipt) => 415,
                _ => 200,
            };
        }

        private static bool IsValidIssueDate(ReceiptWriteDto receipt) {
            return receipt.InvoiceId != Guid.Empty || DateHelpers.DateToISOString(receipt.Date) == DateHelpers.DateToISOString(DateHelpers.GetLocalDateTime());
        }

        private async Task<bool> IsCompositeKeyValidAsync(ReceiptWriteDto receipt) {
            if (receipt.InvoiceId == Guid.Empty) {
                var x = await context.Transactions
                    .AsNoTracking()
                    .Where(x => receipt.Date.Year == DateHelpers.GetLocalDateTime().Year && x.DocumentTypeId == receipt.DocumentTypeId && x.InvoiceNo == receipt.InvoiceNo)
                    .SingleOrDefaultAsync();
                return x == null;
            } else {
                return await Task.Run(() => true);
            }
        }

        private async Task<bool> IsReceiptCountEqualToLastReceiptNo(ReceiptWriteDto receipt) {
            if (receipt.InvoiceId == Guid.Empty) {
                var x = await context.Transactions
                    .AsNoTracking()
                    .Where(x => receipt.Date.Year == DateHelpers.GetLocalDateTime().Year && x.DocumentTypeId == receipt.DocumentTypeId)
                    .ToListAsync();
                return x.Count == receipt.InvoiceNo - 1;
            } else {
                return await Task.Run(() => true);
            }
        }

        private async Task<bool> IsValidCustomer(ReceiptWriteDto receipt) {
            if (receipt.InvoiceId == Guid.Empty) {
                return await context.Customers
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == receipt.CustomerId && x.IsActive) != null;
            }
            return await context.Customers
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == receipt.CustomerId) != null;
        }

        private async Task<bool> IsValidDocumentType(ReceiptWriteDto receipt) {
            if (receipt.InvoiceId == Guid.Empty) {
                return await context.DocumentTypes
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == receipt.DocumentTypeId && x.IsActive) != null;
            }
            return await context.DocumentTypes
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == receipt.DocumentTypeId) != null;
        }

        private async Task<bool> IsValidShipOwner(ReceiptWriteDto receipt) {
            if (receipt.InvoiceId == Guid.Empty) {
                return await context.ShipOwners
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == receipt.ShipOwnerId && x.IsActive) != null;
            }
            return await context.ShipOwners
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == receipt.ShipOwnerId) != null;
        }

        private static bool IsAlreadyUpdated(Receipt z, ReceiptWriteDto transaction) {
            return z != null && z.PutAt != transaction.PutAt;
        }

    }

}