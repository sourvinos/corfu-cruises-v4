using API.Infrastructure.Users;
using API.Infrastructure.Classes;
using API.Infrastructure.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace API.Features.Sales.DocumentTypes {

    public class DocumentTypeValidation : Repository<DocumentType>, IDocumentTypeValidation {

        public DocumentTypeValidation(AppDbContext appDbContext, IHttpContextAccessor httpContext, IOptions<TestingEnvironment> settings, UserManager<UserExtended> userManager) : base(appDbContext, httpContext, settings, userManager) { }

        public async Task<int> IsValidAsync(DocumentType z, DocumentTypeWriteDto documentType) {
            return true switch {
                var x when x == !await IsValidShip(documentType) => 454,
                var x when x == !await IsValidShipOwner(documentType) => 449,
                var x when x == IsAlreadyUpdated(z, documentType) => 415,
                _ => 200,
            };
        }

        private async Task<bool> IsValidShip(DocumentTypeWriteDto documentType) {
            if (documentType.ShipId != null && documentType.ShipId != 0) {
                if (documentType.Id == 0) {
                    var ship = await context.Ships
                        .AsNoTracking()
                        .FirstOrDefaultAsync(x => x.Id == documentType.ShipId && x.IsActive);
                    if (ship == null)
                        return false;
                } else {
                    var ship = await context.Ships
                        .AsNoTracking()
                        .FirstOrDefaultAsync(x => x.Id == documentType.ShipId);
                    if (ship == null)
                        return false;
                }
            }
            return true;
        }

        private async Task<bool> IsValidShipOwner(DocumentTypeWriteDto documentType) {
            if (documentType.Id == 0) {
                return await context.ShipOwners
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == documentType.ShipOwnerId && x.IsActive) != null;
            }
            return await context.ShipOwners
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == documentType.ShipOwnerId) != null;
        }

        private static bool IsAlreadyUpdated(DocumentType z, DocumentTypeWriteDto DocumentType) {
            return z != null && z.PutAt != DocumentType.PutAt;
        }

    }

}