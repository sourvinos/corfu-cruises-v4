using System.Collections.Generic;
using System.Linq;
using API.Infrastructure.Users;
using API.Infrastructure.Classes;
using API.Infrastructure.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace API.Features.Reservations.Schedules {

    public class ScheduleValidation : Repository<Schedule>, IScheduleValidation {

        public ScheduleValidation(AppDbContext context, IHttpContextAccessor httpContext, IOptions<TestingEnvironment> settings, UserManager<UserExtended> userManager) : base(context, httpContext, settings, userManager) { }

        public int IsValidOnNew(List<ScheduleWriteDto> schedules) {
            return true switch {
                var x when x == !IsValidDestinationOnNew(schedules) => 451,
                var x when x == !IsValidPortOnNew(schedules) => 411,
                _ => 200,
            };
        }

        public int IsValidOnUpdate(Schedule z, ScheduleWriteDto schedule) {
            return true switch {
                var x when x == !IsValidDestinationOnUpdate(schedule) => 451,
                var x when x == !IsValidPortOnUpdate(schedule) => 411,
                var x when x == IsAlreadyUpdated(z, schedule) => 415,
                _ => 200,
            };
        }

        private bool IsValidDestinationOnNew(List<ScheduleWriteDto> schedules) {
            if (schedules != null) {
                foreach (var schedule in schedules) {
                    if (context.Destinations
                        .AsNoTracking()
                        .SingleOrDefault(x => x.Id == schedule.DestinationId && x.IsActive) == null) return false;
                }
            }
            return true;
        }

        private bool IsValidDestinationOnUpdate(ScheduleWriteDto schedule) {
            return context.Destinations
                .AsNoTracking()
                .SingleOrDefault(x => x.Id == schedule.DestinationId) != null;
        }

        private bool IsValidPortOnNew(List<ScheduleWriteDto> schedules) {
            if (schedules != null) {
                foreach (var schedule in schedules) {
                    if (context.Ports
                        .AsNoTracking()
                        .SingleOrDefault(x => x.Id == schedule.PortId && x.IsActive) == null) return false;
                }
            }
            return true;
        }

        private bool IsValidPortOnUpdate(ScheduleWriteDto schedule) {
            return context.Ports
                .AsNoTracking()
                .SingleOrDefault(x => x.Id == schedule.PortId) != null;
        }

        private static bool IsAlreadyUpdated(Schedule z, ScheduleWriteDto schedule) {
            return z != null && z.PutAt != schedule.PutAt;
        }

    }

}