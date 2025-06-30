using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Features.Reservations.Reservations;
using API.Infrastructure.Users;
using API.Infrastructure.Classes;
using API.Infrastructure.Implementations;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Options;

namespace API.Features.Reservations.Boarding {

    public class BoardingRepository : Repository<Reservation>, IBoardingRepository {

        private readonly IMapper mapper;
        private readonly TestingEnvironment testingSettings;

        public BoardingRepository(AppDbContext appDbContext, IHttpContextAccessor httpContext, IMapper mapper, IOptions<TestingEnvironment> testingSettings, UserManager<UserExtended> userManager) : base(appDbContext, httpContext, testingSettings, userManager) {
            this.mapper = mapper;
            this.testingSettings = testingSettings.Value;
        }

        public async Task<BoardingFinalGroupVM> Get(string date, int[] destinationIds, int[] portIds, int?[] shipIds) {
            var reservations = await context.Reservations
                .AsNoTracking()
                .Include(x => x.Customer)
                .Include(x => x.Destination)
                .Include(x => x.Driver)
                .Include(x => x.PickupPoint)
                .Include(x => x.PortAlternate)
                .Include(x => x.Ship)
                .Include(x => x.Passengers).ThenInclude(x => x.Nationality)
                .Where(x => x.Date == Convert.ToDateTime(date)
                    && destinationIds.Contains(x.DestinationId)
                    && portIds.Contains(x.PortAlternateId)
                    && (shipIds.Contains(x.ShipId) || x.ShipId == null))
                .ToListAsync();
            int TotalPax = reservations.Sum(x => x.TotalPax);
            int embarkedPassengers = reservations.SelectMany(c => c.Passengers).Count(x => x.IsBoarded);
            int remainingPersons = TotalPax - embarkedPassengers;
            var mainResult = new BoardingInitialGroupVM {
                TotalPax = TotalPax,
                EmbarkedPassengers = embarkedPassengers,
                PendingPax = remainingPersons,
                Reservations = reservations.ToList()
            };
            return mapper.Map<BoardingInitialGroupVM, BoardingFinalGroupVM>(mainResult);
        }

        public void EmbarkPassengers(bool ignoreCurrentStatus, int[] ids) {
            using var transaction = context.Database.BeginTransaction();
            var records = context.Passengers
                .Where(x => ids.Contains(x.Id))
                .ToList();
            records.ForEach(x => x.IsBoarded = ignoreCurrentStatus || !x.IsBoarded);
            context.SaveChanges();
            DisposeOrCommit(transaction);
        }

        private void DisposeOrCommit(IDbContextTransaction transaction) {
            if (testingSettings.IsTesting) {
                transaction.Dispose();
            } else {
                transaction.Commit();
            }
        }

    }

}
