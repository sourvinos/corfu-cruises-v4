using API.Infrastructure.Interfaces;

namespace API.Features.Sales.PaymentMethods {

    public class PaymentMethodWriteDto : IMetadata {

        // PK
        public int Id { get; set; }
        // Fields
        public int MyDataId { get; set; }
        public string Description { get; set; }
        public string DescriptionEn { get; set; }
        public bool IsCash { get; set; }
        public bool IsDefault { get; set; }
        public bool IsActive { get; set; }
        // Metadata
        public string PostAt { get; set; }
        public string PostUser { get; set; }
        public string PutAt { get; set; }
        public string PutUser { get; set; }

    }

}