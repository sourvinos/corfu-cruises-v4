using System.Threading.Tasks;

namespace API.Features.Reservations.Reservations {

    public interface IReservationLinkTwist {

        Task<LinkTwistReservation> GetReservationAsync(string code);
        Task<LinkTwistReservation[]> GetReservationsAsync(LinkTwistReservationCriteriaVM criteria);

    }

}