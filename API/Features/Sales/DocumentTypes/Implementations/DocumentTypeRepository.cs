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

namespace API.Features.Sales.DocumentTypes {

    public class DocumentTypeRepository : Repository<DocumentType>, IDocumentTypeRepository {

        private readonly IMapper mapper;
        private readonly TestingEnvironment testingEnvironment;

        public DocumentTypeRepository(AppDbContext appDbContext, IHttpContextAccessor httpContext, IMapper mapper, IOptions<TestingEnvironment> testingEnvironment, UserManager<UserExtended> userManager) : base(appDbContext, httpContext, testingEnvironment, userManager) {
            this.mapper = mapper;
            this.testingEnvironment = testingEnvironment.Value;
        }

        public async Task<IEnumerable<DocumentTypeListVM>> GetAsync() {
            var DocumentTypes = await context.DocumentTypes
                .AsNoTracking()
                .Include(x => x.Ship)
                .Include(x => x.ShipOwner)
                .OrderBy(x => x.ShipOwner.DescriptionEn).ThenBy(x => x.Ship.Description).ThenBy(x => x.Description).ThenBy(x => x.Batch)
                .ToListAsync();
            return mapper.Map<IEnumerable<DocumentType>, IEnumerable<DocumentTypeListVM>>(DocumentTypes);
        }

        public async Task<IEnumerable<DocumentTypeBrowserVM>> GetForBrowserAsync(int discriminatorId) {
            var documentTypes = await context.DocumentTypes
                .AsNoTracking()
                .Include(x => x.Ship)
                .Include(x => x.ShipOwner)
                .Where(x => x.DiscriminatorId == discriminatorId)
                .OrderBy(x => x.ShipOwner.Description).ThenBy(x => x.Description).ThenBy(x => x.Ship.Description)
                .ToListAsync();
            return mapper.Map<IEnumerable<DocumentType>, IEnumerable<DocumentTypeBrowserVM>>(documentTypes);
        }

        public async Task<DocumentTypeBrowserVM> GetByIdForBrowserAsync(int id) {
            var record = await context.DocumentTypes
                .AsNoTracking()
                .Include(x => x.Ship)
                .Include(x => x.ShipOwner)
                .SingleOrDefaultAsync(x => x.Id == id);
            return mapper.Map<DocumentType, DocumentTypeBrowserVM>(record);
        }

        public async Task<DocumentType> GetByIdAsync(int id) {
            return await context.DocumentTypes
                .AsNoTracking()
                .Include(x => x.Ship)
                .Include(x => x.ShipOwner)
                .SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<int> GetLastDocumentTypeNoAsync(int documentTypeId) {
            var lastInvoiceNo = await context.Transactions
                .AsNoTracking()
                .Where(x => x.DocumentTypeId == documentTypeId)
                .OrderBy(x => x.InvoiceNo)
                .Select(x => x.InvoiceNo)
                .LastOrDefaultAsync();
            return lastInvoiceNo;
        }

    }

}