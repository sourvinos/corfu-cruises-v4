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

namespace API.Features.Sales.Prices {

    public class PriceRepository : Repository<Price>, IPriceRepository {

        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly UserManager<UserExtended> userManager;

        public PriceRepository(AppDbContext context, IHttpContextAccessor httpContextAccessor, IMapper mapper, IOptions<TestingEnvironment> settings, UserManager<UserExtended> userManager) : base(context, httpContextAccessor, settings, userManager) {
            this.httpContextAccessor = httpContextAccessor;
            this.mapper = mapper;
            this.userManager = userManager;
        }

        public async Task<IEnumerable<PriceListVM>> GetAsync() {
            var prices = await context.Prices
                .AsNoTracking()
                .Include(x => x.Customer)
                .Include(x => x.Destination)
                .Include(x => x.Port)
                .OrderBy(x => x.Customer.Description).ThenBy(x => x.Destination.Description).ThenBy(x => x.Port.Description).ThenBy(x => x.From).ThenBy(x => x.To)
                .ToListAsync();
            return mapper.Map<IEnumerable<Price>, IEnumerable<PriceListVM>>(prices);
        }

        public async Task<Price> GetByIdAsync(string id, bool includeTables) {
            return includeTables
                ? await context.Prices
                    .AsNoTracking()
                    .Include(p => p.Customer)
                    .Include(p => p.Destination)
                    .Include(x => x.Port)
                    .SingleOrDefaultAsync(x => x.Id.ToString() == id)
                : await context.Prices
                    .AsNoTracking()
                    .SingleOrDefaultAsync(x => x.Id.ToString() == id);
        }

        public void DeleteRange(string[] ids) {
            context.Prices
                .RemoveRange(context.Prices
                .Where(x => ids.Contains(x.Id.ToString()))
                .ToList());
            context.SaveChanges();
        }

        public async Task<IEnumerable<PriceListVM>> GetPricesForSalesAsync(SalesCriteriaVM criteria) {
            var x = await context.Prices
               .AsNoTracking()
               .Include(x => x.Customer)
               .Include(x => x.Destination)
               .Include(x => x.Port)
               .Where(x => x.From <= Convert.ToDateTime(criteria.Date) && x.To >= Convert.ToDateTime(criteria.Date) && x.CustomerId == criteria.CustomerId && x.DestinationId == criteria.DestinationId)
               .OrderBy(x => x.PortId)
               .ToListAsync();
            return mapper.Map<IEnumerable<Price>, IEnumerable<PriceListVM>>(x);
        }

    }

}