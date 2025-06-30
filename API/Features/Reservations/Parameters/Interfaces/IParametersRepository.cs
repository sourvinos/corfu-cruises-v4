using System.Threading.Tasks;
using API.Infrastructure.Interfaces;

namespace API.Features.Reservations.Parameters {

    public interface IReservationParametersRepository : IRepository<ReservationParameter> {

        Task<ReservationParameter> GetAsync();

    }

}