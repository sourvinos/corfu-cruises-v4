namespace API.Features.Sales.PaymentMethods {

    public class PaymentMethodBrowserVM {

        public int Id { get; set; }
        public string Description { get; set; }
        public bool IsCash { get; set; }
        public bool IsDefault { get; set; }
        public bool IsActive { get; set; }

    }

}