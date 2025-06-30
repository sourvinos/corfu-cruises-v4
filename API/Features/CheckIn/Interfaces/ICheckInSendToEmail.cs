using System.Threading.Tasks;

namespace API.Features.CheckIn {

    public interface ICheckInSendToEmail {

        Task SendReservationToEmail(CheckInBoardingPassReservationVM reservation);

    }

}