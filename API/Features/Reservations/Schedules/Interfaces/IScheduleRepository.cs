using System.Collections.Generic;
using System.Threading.Tasks;
using API.Infrastructure.Interfaces;

namespace API.Features.Reservations.Schedules {

    public interface IScheduleRepository : IRepository<Schedule> {

        Task<IEnumerable<ScheduleListVM>> GetAsync();
        Task<Schedule> GetByIdAsync(int id, bool includeTables);
        List<ScheduleWriteDto> AttachMetadataToPostDto(List<ScheduleWriteDto> schedules);

    }

}