using API.Infrastructure.Users;
using API.Infrastructure.Classes;
using API.Infrastructure.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace API.Features.Reservations.Destinations {

    public class DestinationValidation : Repository<Destination>, IDestinationValidation {

        public DestinationValidation(AppDbContext appDbContext, IHttpContextAccessor httpContext, IOptions<TestingEnvironment> settings, UserManager<UserExtended> userManager) : base(appDbContext, httpContext, settings, userManager) { }

        public int IsValid(Destination z, DestinationWriteDto destination) {
            return true switch {
                var x when x == IsAlreadyUpdated(z, destination) => 415,
                _ => 200,
            };
        }

        private static bool IsAlreadyUpdated(Destination z, DestinationWriteDto destination) {
            return z != null && z.PutAt != destination.PutAt;
        }

    }

}