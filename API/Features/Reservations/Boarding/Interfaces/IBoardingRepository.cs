using System.Threading.Tasks;

namespace API.Features.Reservations.Boarding {

    public interface IBoardingRepository {

        Task<BoardingFinalGroupVM> Get(string date, int[] destinationIds, int[] portIds, int?[] shipIds);
        void EmbarkPassengers(bool ignoreCurrentStatus, int[] ids);
    }

}