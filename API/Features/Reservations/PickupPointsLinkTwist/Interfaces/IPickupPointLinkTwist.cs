using System.Threading.Tasks;

namespace API.Features.Reservations.PickupPointsLinkTwist {

    public interface IPickupPointLinkTwist {

        Task<PickupPointLinkTwistVM[]> GetAllAsync();
        Task<PickupPointLinkTwistVM> GetByAliasAsync(string alias);

    }

}