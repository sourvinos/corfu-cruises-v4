using API.Infrastructure.Interfaces;

namespace API.Features.Reservations.Drivers{

    public interface IDriverValidation : IRepository<Driver> {

        int IsValid(Driver x, DriverWriteDto gender);

    }

}