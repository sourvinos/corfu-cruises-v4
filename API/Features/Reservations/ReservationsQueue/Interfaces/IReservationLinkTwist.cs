using System.Threading.Tasks;

namespace API.Features.Reservations.LinkTwist {

    public interface IReservationLinkTwist {

        Task<LinkTwistReservation> GetReservationAsync(string code);
        Task<LinkTwistReservation[]> GetReservationsAsync(LinkTwistReservationCriteriaVM criteria);
        Task GetFreshReservationsAsync(LinkTwistReservationCriteriaVM criteria);

    }

}