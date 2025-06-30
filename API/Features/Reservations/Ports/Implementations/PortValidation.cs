using System.Linq;
using API.Infrastructure.Users;
using API.Infrastructure.Classes;
using API.Infrastructure.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace API.Features.Reservations.Ports {

    public class PortValidation : Repository<Port>, IPortValidation {

        public PortValidation(AppDbContext appDbContext, IHttpContextAccessor httpContext, IOptions<TestingEnvironment> settings, UserManager<UserExtended> userManager) : base(appDbContext, httpContext, settings, userManager) { }

        public int IsValid(Port z, PortWriteDto port) {
            return true switch {
                var x when x == NewRecordAndPortStopOrderExists(port) => 493,
                var x when x == EditRecordAndPortStopOrderExists(port) => 493,
                var x when x == IsAlreadyUpdated(z, port) => 415,
                _ => 200,
            };
        }

        private bool NewRecordAndPortStopOrderExists(PortWriteDto port) {
            return port.Id == 0 && context.Ports
                .AsNoTracking()
                .Where(x => x.StopOrder == port.StopOrder)
                .FirstOrDefaultAsync().Result != null;
        }

        private bool EditRecordAndPortStopOrderExists(PortWriteDto port) {
            return port.Id != 0 && context.Ports
                .AsNoTracking()
                .Where(x => x.StopOrder == port.StopOrder && x.Id != port.Id)
                .FirstOrDefaultAsync().Result != null;
        }

        private static bool IsAlreadyUpdated(Port z, PortWriteDto port) {
            return z != null && z.PutAt != port.PutAt;
        }

    }

}