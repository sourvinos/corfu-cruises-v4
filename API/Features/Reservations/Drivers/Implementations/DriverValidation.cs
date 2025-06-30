using API.Infrastructure.Users;
using API.Infrastructure.Classes;
using API.Infrastructure.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace API.Features.Reservations.Drivers {

    public class DriverValidation : Repository<Driver>, IDriverValidation {

        public DriverValidation(AppDbContext appDbContext, IHttpContextAccessor httpContext, IOptions<TestingEnvironment> settings, UserManager<UserExtended> userManager) : base(appDbContext, httpContext, settings, userManager) { }

        public int IsValid(Driver z, DriverWriteDto driver) {
            return true switch {
                var x when x == IsAlreadyUpdated(z, driver) => 415,
                _ => 200,
            };
        }

        private static bool IsAlreadyUpdated(Driver z, DriverWriteDto driver) {
            return z != null && z.PutAt != driver.PutAt;
        }

    }

}