using API.Infrastructure.Classes;

namespace API.Features.Reservations.Manifest {

    public class ManifestPassengerVM {

        public int Id { get; set; }
        public string RefNo { get; set; }
        public string Lastname { get; set; }
        public string Firstname { get; set; }
        public string Birthdate { get; set; }
        public string Phones { get; set; }
        public string Remarks { get; set; }
        public string SpecialCare { get; set; }
        public SimpleEntity Gender { get; set; }
        public ManifestNationalityVM Nationality { get; set; }
        public ManifestPortVM Port { get; set; }

    }

}