using System.Threading.Tasks;
using API.Features.Reservations.Reservations;
using API.Infrastructure.Extensions;
using API.Infrastructure.Helpers;
using API.Infrastructure.Responses;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace API.Features.CheckIn {

    [Route("api/[controller]")]
    public class CheckInController : ControllerBase {

        #region variables

        private readonly ICheckInReadRepository checkInReadRepo;
        private readonly ICheckInSendToEmail checkInSendToEmail;
        private readonly ICheckInUpdateRepository checkInUpdateRepo;
        private readonly ICheckInValidation checkInValidation;
        private readonly IMapper mapper;

        #endregion

        public CheckInController(ICheckInReadRepository checkInReadRepo, ICheckInSendToEmail checkInSendToEmail, ICheckInUpdateRepository checkInUpdateRepo, ICheckInValidation checkInValidation, IMapper mapper) {
            this.checkInReadRepo = checkInReadRepo;
            this.checkInSendToEmail = checkInSendToEmail;
            this.checkInUpdateRepo = checkInUpdateRepo;
            this.checkInValidation = checkInValidation;
            this.mapper = mapper;
        }

        [HttpGet("refNo/{refNo}")]
        public async Task<ResponseWithBody> GetByRefNo(string refNo) {
            var x = await checkInReadRepo.GetByRefNo(refNo);
            if (x != null) {
                var z = checkInValidation.IsValidOnRead(x);
                if (z == 200) {
                    return new ResponseWithBody {
                        Code = 200,
                        Icon = Icons.Info.ToString(),
                        Message = ApiMessages.OK(),
                        Body = mapper.Map<Reservation, ReservationReadDto>(x)
                    };
                } else {
                    throw new CustomException() {
                        ResponseCode = 403
                    };
                };
            } else {
                throw new CustomException() {
                    ResponseCode = 404
                };
            }
        }

        [HttpGet("date/{date}/destinationId/{destinationId}/lastname/{lastname}/firstname/{firstname}")]
        public async Task<ResponseWithBody> GetByDate(string date, int destinationId, string lastname, string firstname) {
            var x = await checkInReadRepo.GetByDate(date, destinationId, lastname, firstname);
            if (x != null) {
                var z = checkInValidation.IsValidOnRead(x);
                if (z == 200) {
                    return new ResponseWithBody {
                        Code = 200,
                        Icon = Icons.Info.ToString(),
                        Message = ApiMessages.OK(),
                        Body = mapper.Map<Reservation, ReservationReadDto>(x)
                    };
                } else {
                    throw new CustomException() {
                        ResponseCode = 403
                    };
                };
            } else {
                throw new CustomException() {
                    ResponseCode = 404
                };
            }
        }

        [HttpPut]
        [ServiceFilter(typeof(ModelValidationAttribute))]
        public async Task<Response> Put([FromBody] ReservationWriteDto reservation) {
            var x = await checkInReadRepo.GetByIdAsync(reservation.ReservationId.ToString(), false);
            if (x != null) {
                var z = checkInValidation.IsValidOnUpdate(x, reservation);
                if (z == 200) {
                    checkInUpdateRepo.Update(reservation.ReservationId, mapper.Map<ReservationWriteDto, Reservation>(reservation));
                    return new Response {
                        Code = 200,
                        Icon = Icons.Success.ToString(),
                        Id = null,
                        Message = x.RefNo
                    };
                } else {
                    throw new CustomException() {
                        ResponseCode = z
                    };
                }
            } else {
                throw new CustomException() {
                    ResponseCode = 404
                };
            }
        }

        [HttpPatch]
        public async Task<Response> UpdateEmail([FromBody] CheckInUpdateEmailVM vm) {
            var x = await checkInReadRepo.GetByIdAsync(vm.ReservationId, false);
            if (x != null) {
                checkInUpdateRepo.UpdateEmail(x, vm.Email);
                return new Response {
                    Code = 200,
                    Icon = Icons.Success.ToString(),
                    Id = x.ReservationId.ToString(),
                    Message = ApiMessages.OK()
                };
            } else {
                throw new CustomException() {
                    ResponseCode = 404
                };
            }
        }

        [HttpGet("emailBoardingPass/{reservationId}")]
        public async Task<Response> EmailBoardingPass(string reservationId) {
            var x = await checkInReadRepo.GetByIdAsync(reservationId, true);
            if (x != null) {
                await checkInSendToEmail.SendReservationToEmail(mapper.Map<Reservation, CheckInBoardingPassReservationVM>(x));
                return new Response {
                    Code = 200,
                    Icon = Icons.Success.ToString(),
                    Id = x.ReservationId.ToString(),
                    Message = ApiMessages.OK()
                };
            } else {
                throw new CustomException() {
                    ResponseCode = 404
                };
            }
        }

    }

}