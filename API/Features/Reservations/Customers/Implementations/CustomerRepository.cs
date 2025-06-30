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

namespace API.Features.Reservations.Customers {

    public class CustomerRepository : Repository<Customer>, ICustomerRepository {

        private readonly IMapper mapper;

        public CustomerRepository(AppDbContext appDbContext, IHttpContextAccessor httpContext, IMapper mapper, IOptions<TestingEnvironment> settings, UserManager<UserExtended> userManager) : base(appDbContext, httpContext, settings, userManager) {
            this.mapper = mapper;
        }

        public async Task<IEnumerable<CustomerListVM>> GetAsync() {
            var customers = await context.Customers
                .AsNoTracking()
                .OrderBy(x => x.Description)
                .ToListAsync();
            return mapper.Map<IEnumerable<Customer>, IEnumerable<CustomerListVM>>(customers);
        }

        public async Task<IEnumerable<CustomerBrowserVM>> GetForBrowserAsync() {
            var customers = await context.Customers
                .AsNoTracking()
                .OrderBy(x => x.Description)
                .ToListAsync();
            return mapper.Map<IEnumerable<Customer>, IEnumerable<CustomerBrowserVM>>(customers);
        }

        public async Task<IEnumerable<SimpleEntity>> GetForCriteriaAsync() {
            var customers = await context.Customers
                .AsNoTracking()
                .OrderBy(x => x.Description)
                .ToListAsync();
            return mapper.Map<IEnumerable<Customer>, IEnumerable<SimpleEntity>>(customers);
        }

        public async Task<CustomerBrowserVM> GetByIdForBrowserAsync(int id) {
            var record = await context.Customers
                .AsNoTracking()
                .Include(x => x.Nationality)
                .OrderBy(x => x.Description)
                .SingleOrDefaultAsync(x => x.Id == id);
            return mapper.Map<Customer, CustomerBrowserVM>(record);
        }

        public async Task<Customer> GetByIdAsync(int id, bool includeTables) {
            return includeTables
                ? await context.Customers
                    .AsNoTracking()
                    .Include(x => x.Nationality)
                    .Include(x => x.TaxOffice)
                    .SingleOrDefaultAsync(x => x.Id == id)
                : await context.Customers
                    .AsNoTracking()
                    .SingleOrDefaultAsync(x => x.Id == id);
        }

        public CustomerValidVM GetCustomerData(Customer x) {
            return new CustomerValidVM {
                Id = x.Id,
                Description = x.Description,
                IsValid = x.FullDescription != "" && x.VatNumber != "" && x.PostalCode != "" & x.City != ""
            };
        }

        public async Task<IList<CustomerListVM>> GetForBalanceSheetAsync() {
            var customers = await context.Customers
                .AsNoTracking()
                .OrderBy(x => x.Description)
                .ToListAsync();
            return mapper.Map<IEnumerable<Customer>, IList<CustomerListVM>>(customers);
        }

    }

}