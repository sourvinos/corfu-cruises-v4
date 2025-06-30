using System;
using System.Collections.Generic;
using System.Linq;
using API.Infrastructure.Users;
using API.Infrastructure.Classes;
using API.Infrastructure.Extensions;
using API.Infrastructure.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using API.Features.Sales.Transactions;
using AutoMapper;
using System.Threading.Tasks;

namespace API.Features.Sales.Ledgers {

    public class LedgerSalesRepository : Repository<LedgerSalesRepository>, ILedgerSalesRepository {

        private readonly IHttpContextAccessor httpContext;
        private readonly UserManager<UserExtended> userManager;
        private readonly IMapper mapper;

        public LedgerSalesRepository(AppDbContext appDbContext, IHttpContextAccessor httpContext, IOptions<TestingEnvironment> settings, UserManager<UserExtended> userManager, IMapper mapper) : base(appDbContext, httpContext, settings, userManager) {
            this.httpContext = httpContext;
            this.userManager = userManager;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<LedgerVM>> GetForLedger(string fromDate, string toDate, int customerId, int? shipOwnerId) {
            var connectedCustomerId = GetConnectedCustomerIdForConnectedUser();
            var records = await context.Transactions
                .AsNoTracking()
                .Include(x => x.Customer)
                .Include(x => x.DocumentType)
                .Include(x => x.ShipOwner)
                .Where(x => x.Date <= Convert.ToDateTime(toDate)
                    && (!x.IsCancelled)
                    && (shipOwnerId == null || x.ShipOwner.Id == shipOwnerId)
                    && (connectedCustomerId == null
                        ? x.CustomerId == customerId
                        : x.CustomerId == connectedCustomerId))
                .OrderBy(x => x.Date)
                .ToListAsync();
            return mapper.Map<IEnumerable<TransactionsBase>, IEnumerable<LedgerVM>>(records);
        }

        public IEnumerable<LedgerVM> BuildBalanceForLedger(IEnumerable<LedgerVM> records) {
            decimal balance = 0;
            foreach (var record in records) {
                balance = balance + record.Debit - record.Credit;
                record.Balance = balance;
            }
            return records;
        }

        public LedgerVM BuildPrevious(IEnumerable<LedgerVM> records, string fromDate) {
            decimal debit = 0;
            decimal credit = 0;
            decimal balance = 0;
            foreach (var record in records) {
                if (Convert.ToDateTime(record.Date) < Convert.ToDateTime(fromDate)) {
                    debit += record.Debit;
                    credit += record.Credit;
                    balance = balance + record.Debit - record.Credit;
                }
            }
            var total = BuildTotalLine(debit, credit, balance, "ΣΥΝΟΛΑ ΠΡΟΗΓΟΥΜΕΝΗΣ ΠΕΡΙΟΔΟΥ");
            return total;
        }

        public List<LedgerVM> BuildRequested(IEnumerable<LedgerVM> records, string fromDate) {
            decimal debit = 0;
            decimal credit = 0;
            decimal balance = 0;
            var requestedPeriod = new List<LedgerVM> { };
            foreach (var record in records) {
                if (Convert.ToDateTime(record.Date) >= Convert.ToDateTime(fromDate)) {
                    requestedPeriod.Add(record);
                    debit += record.Debit;
                    credit += record.Credit;
                    balance += record.Debit - record.Credit;
                }
            }
            var total = BuildTotalLine(debit, credit, balance, "ΣΥΝΟΛΑ ΖΗΤΟΥΜΕΝΗΣ ΠΕΡΙΟΔΟΥ");
            requestedPeriod.Add(total);
            return requestedPeriod;
        }

        public LedgerVM BuildTotal(IEnumerable<LedgerVM> records) {
            decimal debit = 0;
            decimal credit = 0;
            decimal balance = 0;
            foreach (var record in records) {
                debit += record.Debit;
                credit += record.Credit;
                balance += record.Debit - record.Credit;
            }
            var total = BuildTotalLine(debit, credit, balance, "ΓΕΝΙΚΑ ΣΥΝΟΛΑ");
            return total;
        }

        public List<LedgerVM> MergePreviousRequestedAndTotal(LedgerVM previousPeriod, List<LedgerVM> requestedPeriod, LedgerVM total) {
            var final = new List<LedgerVM> {
                previousPeriod
            };
            foreach (var record in requestedPeriod) {
                final.Add(record);
            }
            final.Add(total);
            return final;
        }

        public async Task<IEnumerable<LedgerVM>> GetForBalanceAsync(int customerId) {
            var records = await context.Transactions
                .AsNoTracking()
                .Include(x => x.Customer)
                .Include(x => x.DocumentType)
                .Where(x => x.CustomerId == customerId)
                .ToListAsync();
            return mapper.Map<IEnumerable<TransactionsBase>, IEnumerable<LedgerVM>>(records);
        }

        private static LedgerVM BuildTotalLine(decimal debit, decimal credit, decimal balance, string label) {
            var total = new LedgerVM {
                Date = "",
                ShipOwner = new SimpleEntity {
                    Id = 0,
                    Description = ""
                },
                Customer = new SimpleEntity {
                    Id = 0,
                    Description = ""
                },
                DocumentType = new DocumentTypeVM {
                    Id = 0,
                    Description = label,
                    Batch = ""
                },
                InvoiceNo = "",
                Debit = debit,
                Credit = credit,
                Balance = balance
            };
            return total;
        }

        private int? GetConnectedCustomerIdForConnectedUser() {
            var isUserAdmin = Identity.IsUserAdmin(httpContext);
            if (!isUserAdmin) {
                var simpleUser = Identity.GetConnectedUserId(httpContext);
                var connectedUserDetails = Identity.GetConnectedUserDetails(userManager, simpleUser);
                return (int)connectedUserDetails.CustomerId;
            }
            return null;
        }

    }

}