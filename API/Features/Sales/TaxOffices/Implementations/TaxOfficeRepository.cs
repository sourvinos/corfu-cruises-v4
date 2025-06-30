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

namespace API.Features.Sales.TaxOffices {

    public class TaxOfficeRepository : Repository<TaxOffice>, ITaxOfficeRepository {

        private readonly IMapper mapper;

        public TaxOfficeRepository(AppDbContext appDbContext, IHttpContextAccessor httpContext, IMapper mapper, IOptions<TestingEnvironment> settings, UserManager<UserExtended> userManager) : base(appDbContext, httpContext, settings, userManager) {
            this.mapper = mapper;
        }

        public async Task<IEnumerable<TaxOfficeListVM>> GetAsync() {
            var taxOffices = await context.TaxOffices
                .AsNoTracking()
                .OrderBy(x => x.Description)
                .ToListAsync();
            return mapper.Map<IEnumerable<TaxOffice>, IEnumerable<TaxOfficeListVM>>(taxOffices);
        }

        public async Task<IEnumerable<TaxOfficeBrowserVM>> GetForBrowserAsync() {
            var taxOffices = await context.TaxOffices
                .AsNoTracking()
                .OrderBy(x => x.Description)
                .ToListAsync();
            return mapper.Map<IEnumerable<TaxOffice>, IEnumerable<TaxOfficeBrowserVM>>(taxOffices);
        }

        public async Task<TaxOffice> GetByIdAsync(int id) {
            return await context.TaxOffices
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == id);
        }

    }

}