using API.Infrastructure.Interfaces;

namespace API.Features.Reservations.ShipCrews {

    public interface IShipCrewValidation : IRepository<ShipCrew> {

        int IsValid(ShipCrew x, ShipCrewWriteDto shipCrew);

    }

}