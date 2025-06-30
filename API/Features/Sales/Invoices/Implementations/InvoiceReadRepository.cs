using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Infrastructure.Users;
using API.Infrastructure.Classes;
using API.Infrastructure.Implementations;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;

namespace API.Features.Sales.Invoices {

    public class InvoiceReadRepository : Repository<Invoice>, IInvoiceReadRepository {

        private readonly IMapper mapper;

        public InvoiceReadRepository(AppDbContext context, IHttpContextAccessor httpContext, IMapper mapper, IOptions<TestingEnvironment> testingEnvironment, UserManager<UserExtended> userManager) : base(context, httpContext, testingEnvironment, userManager) {
            this.mapper = mapper;
        }

        public async Task<IEnumerable<InvoiceListVM>> GetAsync() {
            var invoices = await context.Invoices
                .AsNoTracking()
                .Where(x => x.DiscriminatorId == 1)
                .Include(x => x.Customer)
                .Include(x => x.Destination)
                .Include(x => x.DocumentType)
                .Include(x => x.Ship)
                .OrderBy(x => x.Date)
                .ToListAsync();
            return mapper.Map<IEnumerable<Invoice>, IEnumerable<InvoiceListVM>>(invoices);
        }

        public async Task<IEnumerable<InvoiceListVM>> GetForPeriodAsync(InvoiceListCriteriaVM criteria) {
            var invoices = await context.Invoices
                .AsNoTracking()
                .Where(x => x.DiscriminatorId == 1)
                .Include(x => x.Customer)
                .Include(x => x.Destination)
                .Include(x => x.DocumentType)
                .Include(x => x.Ship)
                .Include(x => x.ShipOwner)
                .Include(x => x.Aade)
                .Where(x => x.Date >= Convert.ToDateTime(criteria.FromDate) && x.Date <= Convert.ToDateTime(criteria.ToDate))
                .OrderBy(x => x.Date).ThenBy(x => x.ShipOwner.Description).ThenBy(x => x.Ship.Description).ThenBy(x => x.InvoiceNo)
                .ToListAsync();
            return mapper.Map<IEnumerable<Invoice>, IEnumerable<InvoiceListVM>>(invoices);
        }

        public InvoicePdfVM GetFirstWithEmailPending() {
            var invoice = context.Invoices
                .AsNoTracking()
                .Include(x => x.Customer).ThenInclude(x => x.TaxOffice)
                .Include(x => x.Customer).ThenInclude(x => x.Nationality)
                .Include(x => x.Destination)
                .Include(x => x.Ship).ThenInclude(x => x.ShipOwner).ThenInclude(x => x.TaxOffice)
                .Include(x => x.Ship).ThenInclude(x => x.ShipOwner).ThenInclude(x => x.Nationality)
                .Include(x => x.Ship).ThenInclude(x => x.ShipOwner).ThenInclude(x => x.BankAccounts.Where(x => x.IsActive)).ThenInclude(x => x.Bank)
                .Include(x => x.DocumentType)
                .Include(x => x.PaymentMethod)
                .Include(x => x.Aade)
                .Include(x => x.InvoicesPorts).ThenInclude(x => x.Port)
                .Where(x => x.IsEmailPending)
                .FirstOrDefault();
            return mapper.Map<Invoice, InvoicePdfVM>(invoice);
        }

        public async Task<Invoice> GetByIdAsync(string invoiceId, bool includeTables) {
            return includeTables
                ? await context.Invoices
                    .AsNoTracking()
                    .Include(x => x.Customer).ThenInclude(x => x.TaxOffice)
                    .Include(x => x.Customer).ThenInclude(x => x.Nationality)
                    .Include(x => x.Destination)
                    .Include(x => x.Ship).ThenInclude(x => x.ShipOwner).ThenInclude(x => x.TaxOffice)
                    .Include(x => x.DocumentType)
                    .Include(x => x.PaymentMethod)
                    .Include(x => x.Aade)
                    .Include(x => x.InvoicesPorts).ThenInclude(x => x.Port)
                    .Where(x => x.InvoiceId.ToString() == invoiceId)
                    .SingleOrDefaultAsync()
               : await context.Invoices
                    .AsNoTracking()
                    .Include(x => x.Aade)
                    .Include(x => x.InvoicesPorts).ThenInclude(x => x.Port)
                    .Where(x => x.InvoiceId.ToString() == invoiceId)
                    .SingleOrDefaultAsync();
        }

        public async Task<Invoice> GetByIdForPatchEmailSent(string invoiceId) {
            return await context.Invoices
                .AsNoTracking()
                .Where(x => x.InvoiceId.ToString() == invoiceId)
                .SingleOrDefaultAsync();
        }

        public async Task<Invoice> GetByIdForPdfAsync(string invoiceId) {
            return await context.Invoices
                .AsNoTracking()
                .Include(x => x.Customer).ThenInclude(x => x.TaxOffice)
                .Include(x => x.Customer).ThenInclude(x => x.Nationality)
                .Include(x => x.Destination)
                .Include(x => x.Ship).ThenInclude(x => x.ShipOwner).ThenInclude(x => x.TaxOffice)
                .Include(x => x.Ship).ThenInclude(x => x.ShipOwner).ThenInclude(x => x.Nationality)
                .Include(x => x.Ship).ThenInclude(x => x.ShipOwner).ThenInclude(x => x.BankAccounts.Where(x => x.IsActive)).ThenInclude(x => x.Bank)
                .Include(x => x.DocumentType)
                .Include(x => x.PaymentMethod)
                .Include(x => x.Aade)
                .Include(x => x.InvoicesPorts).ThenInclude(x => x.Port)
                .Where(x => x.InvoiceId.ToString() == invoiceId)
                .SingleOrDefaultAsync();
        }

        public async Task<InvoiceAade> GetInvoiceAadeByIdAsync(string invoiceId) {
            return await context.InvoicesAade
                .AsNoTracking()
                .Where(x => x.InvoiceId.ToString() == invoiceId)
                .SingleOrDefaultAsync();
        }

        public async Task<Invoice> GetByIdForXmlAsync(string invoiceId) {
            return await context.Invoices
                .AsNoTracking()
                .Include(x => x.Customer).ThenInclude(x => x.Nationality)
                .Include(x => x.Ship).ThenInclude(x => x.ShipOwner).ThenInclude(x => x.Nationality)
                .Include(x => x.DocumentType)
                .Include(x => x.PaymentMethod)
                .Include(x => x.Aade)
                .SingleOrDefaultAsync(x => x.InvoiceId.ToString() == invoiceId);
        }

        public async Task<Invoice> GetByIdForJsonAsync(string invoiceId) {
            return await context.Invoices
                .AsNoTracking()
                .Include(x => x.Customer).ThenInclude(x => x.Nationality)
                .Include(x => x.Ship).ThenInclude(x => x.ShipOwner).ThenInclude(x => x.Nationality)
                .Include(x => x.DocumentType)
                .Include(x => x.PaymentMethod)
                .Include(x => x.Aade)
                .SingleOrDefaultAsync(x => x.InvoiceId.ToString() == invoiceId);
        }

    }

}