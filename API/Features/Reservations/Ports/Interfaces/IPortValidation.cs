using API.Infrastructure.Interfaces;

namespace API.Features.Reservations.Ports {

    public interface IPortValidation : IRepository<Port> {

        int IsValid(Port x, PortWriteDto port);

    }

}