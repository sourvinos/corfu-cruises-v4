using System.Threading.Tasks;

namespace API.Features.Reservations.Reservations {

    public interface IReservationSendToEmail {

        Task SendReservationToEmail(BoardingPassReservationVM reservation);

    }

}