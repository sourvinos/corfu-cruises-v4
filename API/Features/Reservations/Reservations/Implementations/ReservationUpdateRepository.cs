using System;
using System.Collections.Generic;
using System.Linq;
using API.Infrastructure.Users;
using API.Infrastructure.Classes;
using API.Infrastructure.Extensions;
using API.Infrastructure.Helpers;
using API.Infrastructure.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Options;

namespace API.Features.Reservations.Reservations {

    public class ReservationUpdateRepository : Repository<Reservation>, IReservationUpdateRepository {

        private readonly IHttpContextAccessor httpContext;
        private readonly TestingEnvironment testingEnvironment;

        public ReservationUpdateRepository(AppDbContext context, IHttpContextAccessor httpContext, IOptions<TestingEnvironment> testingEnvironment, UserManager<UserExtended> userManager) : base(context, httpContext, testingEnvironment, userManager) {
            this.httpContext = httpContext;
            this.testingEnvironment = testingEnvironment.Value;
        }

        public Reservation Update(Guid reservationId, Reservation reservation) {
            using var transaction = context.Database.BeginTransaction();
            if (Identity.IsUserAdmin(httpContext)) {
                UpdateReservation(reservation);
                DeletePassengers(reservationId, reservation.Passengers);
            } else {
                AddPassengers(reservation.Passengers);
                UpdatePassengers(reservation.Passengers);
                DeletePassengers(reservationId, reservation.Passengers);
            }
            context.SaveChanges();
            DisposeOrCommit(transaction);
            return reservation;
        }

        public void AssignToDriver(int driverId, string[] reservationIds) {
            using var transaction = context.Database.BeginTransaction();
            var reservations = context.Reservations
                .Where(x => reservationIds.Contains(x.ReservationId.ToString()))
                .ToList();
            reservations.ForEach(x => x.DriverId = driverId);
            context.SaveChanges();
            if (testingEnvironment.IsTesting) {
                transaction.Dispose();
            } else {
                transaction.Commit();
            }
        }

        public void AssignToPort(int portId, string[] reservationIds) {
            using var transaction = context.Database.BeginTransaction();
            var reservations = context.Reservations
                .Where(x => reservationIds.Contains(x.ReservationId.ToString()))
                .ToList();
            reservations.ForEach(x => x.PortAlternateId = portId);
            context.SaveChanges();
            if (testingEnvironment.IsTesting) {
                transaction.Dispose();
            } else {
                transaction.Commit();
            }
        }

        public void AssignToShip(int shipId, string[] reservationIds) {
            using var transaction = context.Database.BeginTransaction();
            var reservations = context.Reservations
                .Where(x => reservationIds.Contains(x.ReservationId.ToString()))
                .ToList();
            reservations.ForEach(x => x.ShipId = shipId);
            context.SaveChanges();
            if (testingEnvironment.IsTesting) {
                transaction.Dispose();
            } else {
                transaction.Commit();
            }
        }

        public string AssignRefNoToNewDto(ReservationWriteDto reservation) {
            return GetDestinationAbbreviation(reservation) + DateHelpers.GetTrimmedUnixTime();
        }

        public void DeleteRange(string[] ids) {
            context.Reservations
                .RemoveRange(context.Reservations
                .Where(x => ids.Contains(x.ReservationId.ToString()))
                .ToList());
            context.SaveChanges();
        }

        private void DisposeOrCommit(IDbContextTransaction transaction) {
            if (testingEnvironment.IsTesting) {
                transaction.Dispose();
            } else {
                transaction.Commit();
            }
        }

        private string GetDestinationAbbreviation(ReservationWriteDto reservation) {
            var destination = context.Destinations
                .AsNoTracking()
                .Where(x => x.Id == reservation.DestinationId)
                .SingleOrDefault();
            return destination.Abbreviation;
        }

        private void UpdateReservation(Reservation reservation) {
            context.Reservations.Update(reservation);
        }

        private void AddPassengers(List<Passenger> passengers) {
            if (passengers.Any(x => x.Id == 0)) {
                context.Passengers.AddRange(passengers.Where(x => x.Id == 0));
            }
        }

        private void UpdatePassengers(List<Passenger> passengers) {
            context.Passengers.UpdateRange(passengers.Where(x => x.Id != 0));
        }

        private void DeletePassengers(Guid reservationId, List<Passenger> passengers) {
            var existingPassengers = context.Passengers
                .AsNoTracking()
                .Where(x => x.ReservationId == reservationId)
                .ToList();
            var passengersToUpdate = passengers
                .Where(x => x.Id != 0)
                .ToList();
            var passengersToDelete = existingPassengers
                .Except(passengersToUpdate, new PassengerComparerById())
                .ToList();
            context.Passengers.RemoveRange(passengersToDelete);
        }

        private class PassengerComparerById : IEqualityComparer<Passenger> {
            public bool Equals(Passenger x, Passenger y) {
                return x.Id == y.Id;
            }
            public int GetHashCode(Passenger x) {
                return x.Id.GetHashCode();
            }
        }

    }

}