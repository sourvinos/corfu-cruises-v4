using System;
using System.Collections.Generic;
using System.Linq;
using API.Features.Reservations.Reservations;
using API.Infrastructure.Classes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Options;

namespace API.Features.CheckIn {

    public class CheckInUpdateRepository : ICheckInUpdateRepository {

        private readonly AppDbContext context;
        private readonly TestingEnvironment testingEnvironment;

        public CheckInUpdateRepository(AppDbContext context, IOptions<TestingEnvironment> testingEnvironment) {
            this.context = context;
            this.testingEnvironment = testingEnvironment.Value;
        }

        public Reservation Update(Guid reservationId, Reservation reservation) {
            using var transaction = context.Database.BeginTransaction();
            AddPassengers(reservation.Passengers);
            UpdatePassengers(reservation.Passengers);
            DeletePassengers(reservationId, reservation.Passengers);
            context.SaveChanges();
            transaction.Commit();
            return reservation;
        }

        public void UpdateEmail(Reservation reservation, string email) {
            using var transaction = context.Database.BeginTransaction();
            reservation.Email = email;
            reservation.IsEmailPending = true;
            reservation.IsEmailSent = false;
            context.Reservations.Attach(reservation);
            context.Entry(reservation).Property(x => x.Email).IsModified = true;
            context.Entry(reservation).Property(x => x.IsEmailPending).IsModified = true;
            context.Entry(reservation).Property(x => x.IsEmailSent).IsModified = true;
            context.SaveChanges();
            DisposeOrCommit(transaction);
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

        private void DisposeOrCommit(IDbContextTransaction transaction) {
            if (testingEnvironment.IsTesting) {
                transaction.Dispose();
            } else {
                transaction.Commit();
            }
        }

    }

}