using System.Linq;
using API.Features.Reservations.Nationalities;
using API.Features.Reservations.Reservations;
using API.Infrastructure.Classes;
using API.Infrastructure.Helpers;
using AutoMapper;

namespace API.Features.CheckIn {

    public class CheckInMappingProfile : Profile {

        public CheckInMappingProfile() {
            // GetByRefNo
            CreateMap<Reservation, ReservationReadDto>()
                .ForMember(x => x.Date, x => x.MapFrom(x => DateHelpers.DateToISOString(x.Date)))
                .ForMember(x => x.Customer, x => x.MapFrom(x => new SimpleEntity { Id = x.Customer.Id, Description = x.Customer.Description }))
                .ForMember(x => x.Destination, x => x.MapFrom(x => new SimpleEntity { Id = x.Destination.Id, Description = x.Destination.Description }))
                .ForMember(x => x.PickupPoint, x => x.MapFrom(x => new PickupPointReadDto {
                    Id = x.PickupPoint.Id,
                    Description = x.PickupPoint.Description,
                    ExactPoint = x.PickupPoint.ExactPoint,
                    Time = x.PickupPoint.Time
                }))
                .ForMember(x => x.Passengers, x => x.MapFrom(x => x.Passengers.Select(passenger => new PassengerReadDto {
                    Id = passenger.Id,
                    ReservationId = passenger.ReservationId,
                    Lastname = passenger.Lastname,
                    Firstname = passenger.Firstname,
                    Birthdate = DateHelpers.DateToISOString(passenger.Birthdate),
                    Remarks = passenger.Remarks,
                    SpecialCare = passenger.SpecialCare,
                    Nationality = new SimpleEntity {
                        Id = passenger.Nationality.Id,
                        Description = passenger.Nationality.Description
                    },
                    Gender = new SimpleEntity {
                        Id = passenger.Gender.Id,
                        Description = passenger.Gender.Description
                    }
                })));
            // Read passenger
            CreateMap<Passenger, PassengerReadDto>()
                .ForMember(x => x.Birthdate, x => x.MapFrom(x => DateHelpers.DateToISOString(x.Birthdate)))
                .ForMember(x => x.Nationality, x => x.MapFrom(x => new NationalityBrowserVM {
                    Id = x.Nationality.Id,
                    Description = x.Nationality.Description,
                    Code = x.Nationality.Code
                }));
            // Write reservation
            CreateMap<ReservationWriteDto, Reservation>();
            // Write passenger
            CreateMap<PassengerWriteDto, Passenger>()
                .ForMember(x => x.Lastname, x => x.MapFrom(x => x.Lastname.Trim()))
                .ForMember(x => x.Firstname, x => x.MapFrom(x => x.Firstname.Trim()))
                .ForMember(x => x.SpecialCare, x => x.MapFrom(x => x.SpecialCare.Trim()))
                .ForMember(x => x.Remarks, x => x.MapFrom(x => x.Remarks.Trim()))
                .ForMember(x => x.OccupantId, x => x.MapFrom(x => 2));
            // Boarding pass
            CreateMap<Reservation, CheckInBoardingPassReservationVM>()
               .ForMember(x => x.Date, x => x.MapFrom(x => DateHelpers.DateToISOString(x.Date)))
               .ForMember(x => x.RefNo, x => x.MapFrom(x => x.RefNo))
               .ForMember(x => x.Destination, x => x.MapFrom(x => new SimpleEntity {
                   Id = x.Destination.Id,
                   Description = x.Destination.Description
               }))
               .ForMember(x => x.Customer, x => x.MapFrom(x => new SimpleEntity {
                   Id = x.Customer.Id,
                   Description = x.Customer.Description
               }))
               .ForMember(x => x.PickupPoint, x => x.MapFrom(x => new CheckInBoardingPassPickupPointVM {
                   Description = x.PickupPoint.Description,
                   ExactPoint = x.PickupPoint.ExactPoint,
                   Time = x.PickupPoint.Time
               }))
               .ForMember(x => x.Email, x => x.MapFrom(x => x.Email))
               .ForMember(x => x.Phones, x => x.MapFrom(x => x.Phones))
               .ForMember(x => x.Remarks, x => x.MapFrom(x => x.Remarks))
               .ForMember(x => x.Passengers, x => x.MapFrom(x => x.Passengers.Select(passenger => new CheckInBoardingPassPassengerVM {
                   Lastname = passenger.Lastname,
                   Firstname = passenger.Firstname
               })));
        }

    }

}