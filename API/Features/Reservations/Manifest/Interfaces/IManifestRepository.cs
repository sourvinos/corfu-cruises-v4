using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Features.Reservations.Manifest {

    public interface IManifestRepository {

        Task<IEnumerable<ManifestPassengerVM>> GetPassengersAsync(string date, int destinationId, int portId, int shipId);
        Task<IEnumerable<ManifestCrewVM>> GetCrewAsync(int shipId);

    }

}