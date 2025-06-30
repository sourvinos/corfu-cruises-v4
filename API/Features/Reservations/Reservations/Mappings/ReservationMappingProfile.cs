using System.Linq;
using API.Features.Reservations.Nationalities;
using API.Features.Reservations.PickupPoints;
using API.Infrastructure.Classes;
using API.Infrastructure.Helpers;
using AutoMapper;

namespace API.Features.Reservations.Reservations {

    public class ReservationMappingProfile : Profile {

        public ReservationMappingProfile() {
            // List
            CreateMap<Reservation, ReservationListVM>()
                .ForMember(x => x.Date, x => x.MapFrom(x => DateHelpers.DateToISOString(x.Date)))
                .ForMember(x => x.Customer, x => x.MapFrom(x => new SimpleEntity { Id = x.Customer.Id, Description = x.Customer.Description }))
                .ForMember(x => x.CoachRoute, x => x.MapFrom(x => new ReservationListCoachRouteVM { Id = x.PickupPoint.CoachRoute.Id, Abbreviation = x.PickupPoint.CoachRoute.Abbreviation }))
                .ForMember(x => x.Destination, x => x.MapFrom(x => new ReservationListDestinationVM { Id = x.Destination.Id, Description = x.Destination.Description, Abbreviation = x.Destination.Abbreviation }))
                .ForMember(x => x.PickupPoint, x => x.MapFrom(x => new ReservationListPickupPointVM { Id = x.PickupPoint.Id, Description = x.PickupPoint.Description, Time = x.PickupPoint.Time }))
                .ForMember(x => x.Driver, x => x.MapFrom(x => new ReservationListDriverVM { Id = x.Driver == null ? 0 : x.Driver.Id, Description = x.Driver == null ? "(EMPTY)" : x.Driver.Description, Phones = x.Driver == null ? "(EMPTY)" : x.Driver.Phones }))
                .ForMember(x => x.Port, x => x.MapFrom(x => new ReservationListPortVM { Id = x.Port.Id, Description = x.Port.Description, Abbreviation = x.Port.Abbreviation }))
                .ForMember(x => x.PortAlternate, x => x.MapFrom(x => new ReservationListPortVM { Id = x.PortAlternate == null ? 0 : x.PortAlternate.Id, Description = x.PortAlternate == null ? "(EMPTY)" : x.PortAlternate.Description, Abbreviation = x.PortAlternate == null ? "(EMPTY)" : x.PortAlternate.Abbreviation }))
                .ForMember(x => x.Ship, x => x.MapFrom(x => new ReservationListShipVM { Id = x.Ship == null ? 0 : x.Ship.Id, Description = x.Ship == null ? "(EMPTY)" : x.Ship.Description, Abbreviation = x.Ship == null ? "(EMPTY)" : x.Ship.Abbreviation }))
                .ForMember(x => x.PassengerCount, x => x.MapFrom(x => x.Passengers.Count))
                .ForMember(x => x.PassengerDifference, x => x.MapFrom(x => x.TotalPax - x.Passengers.Count));
            // DriverList
            CreateMap<Reservation, ReservationDriverListVM>()
                .ForMember(x => x.ExactPoint, x => x.MapFrom(x => x.PickupPoint.ExactPoint))
                .ForMember(x => x.Time, x => x.MapFrom(x => x.PickupPoint.Time))
                .ForMember(x => x.Fullname, x => x.MapFrom(x => x.Passengers.FirstOrDefault().Lastname + " " + x.Passengers.FirstOrDefault().Firstname));
            // GetById
            CreateMap<Reservation, ReservationReadDto>()
                .ForMember(x => x.Date, x => x.MapFrom(x => DateHelpers.DateToISOString(x.Date)))
                .ForMember(x => x.Customer, x => x.MapFrom(x => new SimpleEntity { Id = x.Customer.Id, Description = x.Customer.Description }))
                .ForMember(x => x.Destination, x => x.MapFrom(x => new SimpleEntity { Id = x.Destination.Id, Description = x.Destination.Description }))
                .ForMember(x => x.Driver, x => x.MapFrom(x => x.Driver == null ? new SimpleEntity { Id = 0, Description = "(EMPTY)" } : new SimpleEntity { Id = x.Driver.Id, Description = x.Driver.Description }))
                .ForMember(x => x.Ship, x => x.MapFrom(x => x.Ship == null ? new SimpleEntity { Id = 0, Description = "(EMPTY)" } : new SimpleEntity { Id = x.Ship.Id, Description = x.Ship.Description }))
                .ForMember(x => x.PickupPoint, x => x.MapFrom(x => new PickupPointBrowserVM {
                    Id = x.PickupPoint.Id,
                    Description = x.PickupPoint.Description,
                    ExactPoint = x.PickupPoint.ExactPoint,
                    Time = x.PickupPoint.Time,
                    Port = new SimpleEntity { Id = x.Port.Id, Description = x.Port.Description }
                }))
                .ForMember(x => x.PortAlternate, x => x.MapFrom(x => new SimpleEntity { Id = x.PortAlternate.Id, Description = x.PortAlternate.Description }))
                .ForMember(x => x.Passengers, x => x.MapFrom(x => x.Passengers.Select(passenger => new PassengerReadDto {
                    Id = passenger.Id,
                    ReservationId = passenger.ReservationId,
                    Lastname = passenger.Lastname,
                    Firstname = passenger.Firstname,
                    Birthdate = DateHelpers.DateToISOString(passenger.Birthdate),
                    Remarks = passenger.Remarks,
                    SpecialCare = passenger.SpecialCare,
                    IsBoarded = passenger.IsBoarded,
                    Nationality = new NationalityBrowserVM {
                        Id = passenger.Nationality.Id,
                        Code = passenger.Nationality.Code,
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
            CreateMap<ReservationWriteDto, Reservation>()
                .ForMember(x => x.TicketNo, x => x.MapFrom(x => x.TicketNo.Trim()))
                .ForMember(x => x.Phones, x => x.MapFrom(x => x.Phones.Trim()))
                .ForMember(x => x.Remarks, x => x.MapFrom(x => x.Remarks.Trim()));
            // Write passenger
            CreateMap<PassengerWriteDto, Passenger>()
                .ForMember(x => x.Lastname, x => x.MapFrom(x => x.Lastname.Trim()))
                .ForMember(x => x.Firstname, x => x.MapFrom(x => x.Firstname.Trim()))
                .ForMember(x => x.SpecialCare, x => x.MapFrom(x => x.SpecialCare.Trim()))
                .ForMember(x => x.Remarks, x => x.MapFrom(x => x.Remarks.Trim()))
                .ForMember(x => x.OccupantId, x => x.MapFrom(x => 2));
            // Boarding pass
            CreateMap<Reservation, BoardingPassReservationVM>()
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
               .ForMember(x => x.PickupPoint, x => x.MapFrom(x => new BoardingPassPickupPointVM {
                   Description = x.PickupPoint.Description,
                   ExactPoint = x.PickupPoint.ExactPoint,
                   Time = x.PickupPoint.Time
               }))
               .ForMember(x => x.Email, x => x.MapFrom(x => x.Email))
               .ForMember(x => x.Phones, x => x.MapFrom(x => x.Phones))
               .ForMember(x => x.Remarks, x => x.MapFrom(x => x.Remarks))
               .ForMember(x => x.Passengers, x => x.MapFrom(x => x.Passengers.Select(passenger => new BoardingPassPassengerVM {
                   Lastname = passenger.Lastname,
                   Firstname = passenger.Firstname
               })));
            // CLone reservation
            CreateMap<CloneReservationVM, Reservation>();
        }

    }

}