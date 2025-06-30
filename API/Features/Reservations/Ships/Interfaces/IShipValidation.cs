using API.Infrastructure.Interfaces;

namespace API.Features.Reservations.Ships {

    public interface IShipValidation : IRepository<Ship> {

        int IsValid(Ship x, ShipWriteDto ship);

    }

}