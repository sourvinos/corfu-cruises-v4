using API.Infrastructure.Interfaces;

namespace API.Features.Reservations.CoachRoutes {

    public interface ICoachRouteValidation : IRepository<CoachRoute> {

        int IsValid(CoachRoute x, CoachRouteWriteDto coachRoute);

    }

}