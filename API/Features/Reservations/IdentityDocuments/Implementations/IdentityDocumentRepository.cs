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

namespace API.Features.Reservations.IdentityDocuments {

    public class IdentityDocumentRepository : Repository<IdentityDocument>, IIdentityDocumentRepository {

        private readonly IMapper mapper;

        public IdentityDocumentRepository(AppDbContext appDbContext, IHttpContextAccessor httpContext, IMapper mapper, IOptions<TestingEnvironment> settings, UserManager<UserExtended> userManager) : base(appDbContext, httpContext, settings, userManager) {
            this.mapper = mapper;
        }

        public async Task<IEnumerable<IdentityDocumentListVM>> GetAsync() {
            var identityDocuments = await context.IdentityDocuments
                .AsNoTracking()
                .ToListAsync();
            return mapper.Map<IEnumerable<IdentityDocument>, IEnumerable<IdentityDocumentListVM>>(identityDocuments);
        }

        public async Task<IEnumerable<IdentityDocumentBrowserVM>> GetForBrowserAsync() {
            var identityDocuments = await context.IdentityDocuments
                .AsNoTracking()
                .OrderBy(x => x.Description)
                .ToListAsync();
            return mapper.Map<IEnumerable<IdentityDocument>, IEnumerable<IdentityDocumentBrowserVM>>(identityDocuments);
        }

        public async Task<IdentityDocument> GetByIdAsync(int id) {
            return await context.IdentityDocuments
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == id);
        }

    }

}