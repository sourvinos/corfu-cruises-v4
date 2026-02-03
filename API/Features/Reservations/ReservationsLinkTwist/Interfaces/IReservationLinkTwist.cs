using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Features.Reservations.LinkTwist {

    public interface IReservationLinkTwist {

        Task<IEnumerable<LinkTwistStatus>> GetAsync();
        Task<LinkTwistReservation> GetReservationAsync(string code);
        Task<LinkTwistReservation[]> GetReservationsAsync(LinkTwistReservationCriteriaVM criteria);

    }

}