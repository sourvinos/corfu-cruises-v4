using System;
using System.Collections.Generic;
using System.Linq;
using API.Infrastructure.Users;
using API.Infrastructure.Classes;
using API.Infrastructure.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using API.Features.Sales.Transactions;
using AutoMapper;
using System.Threading.Tasks;
using API.Features.Reservations.Customers;

namespace API.Features.Sales.BalanceSheet {

    public class BalanceSheetRepository : Repository<BalanceSheetRepository>, IBalanceSheetRepository {

        private readonly IMapper mapper;

        public BalanceSheetRepository(AppDbContext appDbContext, IHttpContextAccessor httpContext, IOptions<TestingEnvironment> settings, UserManager<UserExtended> userManager, IMapper mapper) : base(appDbContext, httpContext, settings, userManager) {
            this.mapper = mapper;
        }

        public async Task<IEnumerable<BalanceSheetVM>> GetForBalanceSheet(string fromDate, string toDate, int customerId, int? shipOwnerId) {
            var records = await context.Transactions
                .AsNoTracking()
                .Include(x => x.Customer)
                .Include(x => x.DocumentType)
                .Where(x => x.Date <= Convert.ToDateTime(toDate)
                    && (x.CustomerId == customerId)
                    && (x.ShipOwner.Id == shipOwnerId || shipOwnerId == null)
                    && (x.IsCancelled == false))
                .OrderBy(x => x.Date)
                .ToListAsync();
            return mapper.Map<IEnumerable<TransactionsBase>, IEnumerable<BalanceSheetVM>>(records);
        }

        public IEnumerable<BalanceSheetVM> BuildBalanceForBalanceSheet(IEnumerable<BalanceSheetVM> records) {
            decimal balance = 0;
            foreach (var record in records) {
                balance = balance + record.Debit - record.Credit;
                record.Balance = balance;
            }
            return records;
        }

        public BalanceSheetVM BuildPrevious(CustomerListVM customer, IEnumerable<BalanceSheetVM> records, string fromDate) {
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
            var total = BuildTotalLine(customer, debit, credit, balance, "ΣΥΝΟΛΑ ΠΡΟΗΓΟΥΜΕΝΗΣ ΠΕΡΙΟΔΟΥ");
            return total;
        }

        public List<BalanceSheetVM> BuildRequested(CustomerListVM customer, IEnumerable<BalanceSheetVM> records, string fromDate) {
            decimal debit = 0;
            decimal credit = 0;
            decimal balance = 0;
            var requestedPeriod = new List<BalanceSheetVM> { };
            foreach (var record in records) {
                if (Convert.ToDateTime(record.Date) >= Convert.ToDateTime(fromDate)) {
                    requestedPeriod.Add(record);
                    debit += record.Debit;
                    credit += record.Credit;
                    balance += record.Debit - record.Credit;
                }
            }
            var total = BuildTotalLine(customer, debit, credit, balance, "ΣΥΝΟΛΑ ΖΗΤΟΥΜΕΝΗΣ ΠΕΡΙΟΔΟΥ");
            requestedPeriod.Add(total);
            return requestedPeriod;
        }

        public BalanceSheetVM BuildTotal(CustomerListVM customer, IEnumerable<BalanceSheetVM> records) {
            decimal debit = 0;
            decimal credit = 0;
            decimal balance = 0;
            foreach (var record in records) {
                debit += record.Debit;
                credit += record.Credit;
                balance += record.Debit - record.Credit;
            }
            var total = BuildTotalLine(customer, debit, credit, balance, "ΓΕΝΙΚΑ ΣΥΝΟΛΑ");
            return total;
        }

        public List<BalanceSheetVM> MergePreviousRequestedAndTotal(BalanceSheetVM previousPeriod, List<BalanceSheetVM> requestedPeriod, BalanceSheetVM total) {
            var final = new List<BalanceSheetVM> {
                previousPeriod
            };
            foreach (var record in requestedPeriod) {
                final.Add(record);
            }
            final.Add(total);
            return final;
        }

        public BalanceSheetSummaryVM Summarize(CustomerListVM customer, IEnumerable<BalanceSheetVM> records) {
            var previousBalance = records.First().Balance;
            var requestedDebit = records.SkipLast(1).Last().Debit;
            var requestedCredit = records.SkipLast(1).Last().Credit;
            var requestedBalance = records.SkipLast(1).Last().Balance;
            var actualBalance = previousBalance + requestedBalance;
            var summary = new BalanceSheetSummaryVM {
                Customer = new SimpleEntity {
                    Id = customer.Id,
                    Description = customer.Description
                },
                PreviousBalance = previousBalance,
                Debit = requestedDebit,
                Credit = requestedCredit,
                Balance = requestedDebit - requestedCredit,
                ActualBalance = actualBalance
            };
            return summary;
        }

        public async Task<IEnumerable<BalanceSheetVM>> GetForBalanceAsync(int customerId) {
            var records = await context.Transactions
                .AsNoTracking()
                .Include(x => x.Customer)
                .Include(x => x.DocumentType)
                .Where(x => x.CustomerId == customerId)
                .ToListAsync();
            return mapper.Map<IEnumerable<TransactionsBase>, IEnumerable<BalanceSheetVM>>(records);
        }

        private static BalanceSheetVM BuildTotalLine(CustomerListVM customer, decimal debit, decimal credit, decimal balance, string label) {
            var total = new BalanceSheetVM {
                Date = "",
                Customer = new SimpleEntity {
                    Id = customer.Id,
                    Description = customer.Description
                },
                Debit = debit,
                Credit = credit,
                Balance = balance
            };
            return total;
        }

    }

}