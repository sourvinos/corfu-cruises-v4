namespace API.Features.Reservations.CoachRoutes {

    public class CoachRouteListVM {

        public int Id { get; set; }
        public string Abbreviation { get; set; }
        public string Description { get; set; }
        public bool HasTransfer { get; set; }
        public bool IsActive { get; set; }

    }

}