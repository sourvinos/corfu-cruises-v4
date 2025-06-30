using API.Infrastructure.Interfaces;

namespace API.Features.Reservations.Parameters {

    public interface IReservationParameterValidation : IRepository<ReservationParameter> {

        int IsValid(ReservationParameter x, ParameterWriteDto parameter);

    }

}