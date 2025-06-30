using API.Infrastructure.Classes;

namespace API.Features.Reservations.Manifest {

    public class ManifestCrewVM {

        public int Id { get; set; }
        public string Lastname { get; set; }
        public string Firstname { get; set; }
        public SimpleEntity Gender { get; set; }
        public SimpleEntity Specialty { get; set; }
        public ManifestNationalityVM Nationality { get; set; }
        public string Birthdate { get; set; }

    }

}