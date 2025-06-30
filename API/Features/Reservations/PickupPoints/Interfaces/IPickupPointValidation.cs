using API.Infrastructure.Interfaces;

namespace API.Features.Reservations.PickupPoints {

    public interface IPickupPointValidation : IRepository<PickupPoint> {

        int IsValid(PickupPoint x, PickupPointWriteDto pickupPoint);

    }

}