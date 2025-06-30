using System.Linq;
using API.Features.Reservations.Reservations;
using API.Infrastructure.Classes;
using AutoMapper;

namespace API.Features.Reservations.Boarding {

    public class BoardingMappingProfile : Profile {

        public BoardingMappingProfile() {
            CreateMap<BoardingInitialGroupVM, BoardingFinalGroupVM>()
                .ForMember(x => x.TotalPax, x => x.MapFrom(x => x.TotalPax))
                .ForMember(x => x.EmbarkedPassengers, x => x.MapFrom(x => x.EmbarkedPassengers))
                .ForMember(x => x.PendingPax, x => x.MapFrom(x => x.PendingPax))
                .ForMember(x => x.Reservations, x => x.MapFrom(x => x.Reservations.Select(reservation => new BoardingFinalVM {
                    RefNo = reservation.RefNo,
                    TicketNo = reservation.TicketNo,
                    Remarks = reservation.Remarks,
                    Customer = new SimpleEntity { Id = reservation.Customer.Id, Description = reservation.Customer.Description },
                    Destination = new BoardingFinalDestinationListVM { Id = reservation.Destination.Id, Abbreviation = reservation.Destination.Abbreviation, Description = reservation.Destination.Description },
                    Driver = new SimpleEntity {
                        Id = reservation.Driver != null ? reservation.Driver.Id : 0,
                        Description = reservation.Driver != null ? reservation.Driver.Description : "(EMPTY)"
                    },
                    PickupPoint = new SimpleEntity { Id = reservation.PickupPoint.Id, Description = reservation.PickupPoint.Description },
                    Port = new BoardingFinalPortListVM { Id = reservation.PortAlternate.Id, Abbreviation = reservation.PortAlternate.Abbreviation, Description = reservation.PortAlternate.Description },
                    Ship = new BoardingFinalShipListVM {
                        Id = reservation.Ship != null ? reservation.Ship.Id : 0,
                        Description = reservation.Ship != null ? reservation.Ship.Description : "(EMPTY)",
                        Abbreviation = reservation.Ship != null ? reservation.Ship.Abbreviation : "(EMPTY)"
                    },
                    TotalPax = reservation.TotalPax,
                    EmbarkedPassengers = reservation.Passengers.Count(x => x.IsBoarded),
                    BoardingStatus = DetermineBoardingStatus(reservation),
                    PassengerIds = reservation.Passengers.Select(x => x.Id).ToArray(),
                    Passengers = reservation.Passengers.Select(passenger => new BoardingFinalPassengerVM {
                        Id = passenger.Id,
                        Lastname = passenger.Lastname,
                        Firstname = passenger.Firstname,
                        Nationality = new BoardingFinalPassengerNationalityVM {
                            Id = passenger.Nationality.Id,
                            Code = passenger.Nationality.Code,
                            Description = passenger.Nationality.Description,
                        },
                        IsBoarded = passenger.IsBoarded
                    }).ToList()
                })));
        }

        private static SimpleEntity DetermineBoardingStatus(Reservation reservation) {
            var passengers = reservation.Passengers.Count;
            var embarkedPassengers = reservation.Passengers.Count(x => x.IsBoarded);
            if (passengers == 0 || embarkedPassengers == 0) {
                return BoardingStatus(2, "None");
            } else {
                if (passengers == embarkedPassengers) {
                    return BoardingStatus(1, "All");
                } else {
                    return BoardingStatus(3, "Some");
                }
            }
        }

        private static SimpleEntity BoardingStatus(int id, string status) {
            return new SimpleEntity {
                Id = id,
                Description = status
            };
        }

    }

}