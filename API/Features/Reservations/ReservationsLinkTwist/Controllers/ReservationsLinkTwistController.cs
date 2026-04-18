using System.Collections.Generic;
using System.Threading.Tasks;
using API.Features.Reservations.Reservations;
using API.Infrastructure.Classes;
using API.Infrastructure.Extensions;
using API.Infrastructure.Helpers;
using API.Infrastructure.Responses;
using API.Infrastructure.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Features.Reservations.LinkTwist {

    [Route("api/[controller]")]
    public class ReservationsLinkTwistController : ControllerBase {

        #region variables

        private readonly IReservationLinkTwist linkTwist;
        private readonly IReservationUpdateRepository reservationUpdateRepo;
        private readonly IReservationValidation reservationValidation;
        protected readonly AppDbContext context;
        private readonly UserManager<UserExtended> userManager;
        private readonly IHttpContextAccessor httpContextAccessor;

        #endregion

        public ReservationsLinkTwistController(IHttpContextAccessor httpContextAccessor, AppDbContext context, IReservationLinkTwist linkTwist, IReservationUpdateRepository reservationUpdateRepo, IReservationValidation reservationValidation, UserManager<UserExtended> userManager) {
            this.context = context;
            this.linkTwist = linkTwist;
            this.reservationUpdateRepo = reservationUpdateRepo;
            this.reservationValidation = reservationValidation;
            this.userManager = userManager;
            this.httpContextAccessor = httpContextAccessor;
        }

        [HttpGet("{code}")]
        [Authorize(Roles = "admin")]
        public async Task<LinkTwistReservation> GetByCode(string code) {
            return await linkTwist.GetReservationAsync(code);
        }
        [HttpPost()]

        [HttpPost("getByDateRange")]
        [Authorize(Roles = "admin")]
        public async Task<LinkTwistReservation[]> GetByDateRange([FromBody] LinkTwistReservationCriteriaVM criteria) {
            return await linkTwist.GetReservationsAsync(criteria);
        }

        [HttpPost("getFreshByDateRange")]
        [Authorize(Roles = "admin")]
        public async Task<List<LinkTwistReservation>> GetFreshReservationsAsync([FromBody] LinkTwistReservationCriteriaVM criteria) {
            return await linkTwist.GetFreshReservationsAsync(criteria);
        }

        [HttpPost("saveRange")]
        [Authorize(Roles = "admin")]
        public Response SaveRange([FromBody] List<ReservationWriteDto> reservations) {
            foreach (var x in reservations) {
                UpdateDriverIdWithNull(x);
                UpdateShipIdWithNull(x);
                AttachPortIdToDto(x);
                AttachNewRefNoToDto(x);
            }
            using var transaction = context.Database.BeginTransaction();
            var z = new List<Reservation>();
            foreach (var x in reservations) {
                var i = new Reservation();
                i.RefNo = x.RefNo;
                i.PickupPointId = x.PickupPointId;
                i.TicketNo = x.LinkTwistId;
                i.Adults = x.Adults;
                i.CustomerId = x.CustomerId;
                i.Date = DateHelpers.StringToDate(x.Date);
                i.DestinationId = x.DestinationId;
                i.DriverId = x.DriverId;
                i.Email = x.Email;
                i.Free = x.Free;
                i.Kids = x.Kids;
                i.LinkTwistId = x.LinkTwistId;
                i.Phones = x.Phones;
                i.PortId = x.PortId;
                i.PortAlternateId = (int)x.PortAlternateId;
                i.Remarks = x.Remarks;
                i.PostAt = DateHelpers.DateTimeToISOString(DateHelpers.GetLocalDateTime());
                i.PostUser = Identity.GetConnectedUserDetails(userManager, Identity.GetConnectedUserId(httpContextAccessor)).UserName;
                i.PutAt = DateHelpers.DateTimeToISOString(DateHelpers.GetLocalDateTime());
                i.PutUser = Identity.GetConnectedUserDetails(userManager, Identity.GetConnectedUserId(httpContextAccessor)).UserName;
                i.TicketNo = x.LinkTwistId;
                i.Notes = x.Notes ?? "";
                i.Passengers = MapPassengers(x.Passengers);
                z.Add(i);
            }
            context.AddRange(z);
            context.SaveChanges();
            transaction.Commit();
            return new Response {
                Code = 200,
                Icon = Icons.Success.ToString(),
                Id = null,
                Message = ApiMessages.OK()
            };
        }

        private List<Passenger> MapPassengers(List<PassengerWriteDto> passengers) {
            var z = new List<Passenger>();
            foreach (var x in passengers) {
                var i = new Passenger();
                i.Lastname = x.Lastname;
                i.Firstname = x.Firstname;
                i.GenderId = x.GenderId;
                i.NationalityId = x.NationalityId;
                i.OccupantId = 2;
                i.Remarks = x.Remarks;
                i.SpecialCare = x.SpecialCare;
                i.Birthdate = DateHelpers.StringToDate(x.Birthdate);
                z.Add(i);
            }
            return z;
        }

        private static ReservationWriteDto UpdateDriverIdWithNull(ReservationWriteDto reservation) {
            if (reservation.DriverId == 0) reservation.DriverId = null;
            return reservation;
        }

        private static ReservationWriteDto UpdateShipIdWithNull(ReservationWriteDto reservation) {
            if (reservation.ShipId == 0) reservation.ShipId = null;
            return reservation;
        }

        private ReservationWriteDto AttachPortIdToDto(ReservationWriteDto reservation) {
            reservation.PortId = reservationValidation.GetPortIdFromPickupPointId(reservation);
            reservation.PortAlternateId = reservation.PortId;
            return reservation;
        }

        private ReservationWriteDto AttachNewRefNoToDto(ReservationWriteDto reservation) {
            reservation.RefNo = reservationUpdateRepo.AssignRefNoToNewDto(reservation);
            return reservation;
        }

    }

}