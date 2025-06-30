using API.Infrastructure.Users;
using API.Infrastructure.Classes;
using API.Infrastructure.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace API.Features.Reservations.IdentityDocuments {

    public class IdentityDocumentValidation : Repository<IdentityDocument>, IIdentityDocumentValidation {

        public IdentityDocumentValidation(AppDbContext appDbContext, IHttpContextAccessor httpContext, IOptions<TestingEnvironment> settings, UserManager<UserExtended> userManager) : base(appDbContext, httpContext, settings, userManager) { }

        public int IsValid(IdentityDocument z, IdentityDocumentWriteDto identityDocument) {
            return true switch {
                var x when x == IsAlreadyUpdated(z, identityDocument) => 415,
                _ => 200,
            };
        }

        private static bool IsAlreadyUpdated(IdentityDocument z, IdentityDocumentWriteDto identityDocument) {
            return z != null && z.PutAt != identityDocument.PutAt;
        }

    }

}