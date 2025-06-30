using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Features.Reservations.CrewSpecialties {

    [Route("api/[controller]")]
    public class CrewSpecialtiesController : ControllerBase {

        #region variables

        private readonly ICrewSpecialtyRepository crewSpecialtyRepo;
        private readonly IMapper mapper;

        #endregion

        public CrewSpecialtiesController(ICrewSpecialtyRepository crewSpecialtyRepo, IMapper mapper) {
            this.crewSpecialtyRepo = crewSpecialtyRepo;
            this.mapper = mapper;
        }

        [HttpGet("[action]")]
        [Authorize(Roles = "user, admin")]
        public async Task<IEnumerable<CrewSpecialtyBrowserVM>> GetForBrowserAsync() {
            return await crewSpecialtyRepo.GetBrowserAsync();
        }

    }

}