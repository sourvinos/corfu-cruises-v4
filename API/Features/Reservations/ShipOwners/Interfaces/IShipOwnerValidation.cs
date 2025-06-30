using System.Threading.Tasks;
using API.Infrastructure.Interfaces;

namespace API.Features.Reservations.ShipOwners {

    public interface IShipOwnerValidation : IRepository<ShipOwner> {

        Task<int> IsValidAsync(ShipOwner x, ShipOwnerWriteDto shipOwner);

    }

}