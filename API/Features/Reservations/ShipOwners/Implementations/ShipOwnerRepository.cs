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

namespace API.Features.Reservations.ShipOwners {

    public class ShipOwnerRepository : Repository<ShipOwner>, IShipOwnerRepository {

        private readonly IMapper mapper;

        public ShipOwnerRepository(AppDbContext context, IHttpContextAccessor httpContext, IMapper mapper, IOptions<TestingEnvironment> settings, UserManager<UserExtended> userManager) : base(context, httpContext, settings, userManager) {
            this.mapper = mapper;
        }

        public async Task<IEnumerable<ShipOwnerListVM>> GetAsync() {
            var shipOwners = await context.ShipOwners
                .AsNoTracking()
                .OrderBy(x => x.DescriptionEn)
                .ToListAsync();
            return mapper.Map<IEnumerable<ShipOwner>, IEnumerable<ShipOwnerListVM>>(shipOwners);
        }

        public async Task<IEnumerable<ShipOwnerBrowserVM>> GetForBrowserAsync() {
            var shipOwners = await context.ShipOwners
                .AsNoTracking()
                .OrderBy(x => x.Description)
                .ToListAsync();
            return mapper.Map<IEnumerable<ShipOwner>, IEnumerable<ShipOwnerBrowserVM>>(shipOwners);
        }

        public async Task<ShipOwner> GetByIdAsync(int id) {
            return await context.ShipOwners
                .AsNoTracking()
                .Include(x => x.Nationality)
                .Include(x => x.TaxOffice)
                .SingleOrDefaultAsync(x => x.Id == id);
        }

    }

}