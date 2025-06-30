using API.Infrastructure.Classes;

namespace API.Features.Sales.Invoices {

    public class InvoiceListVM {

        public string InvoiceId { get; set; }
        public string Date { get; set; }
        public int InvoiceNo { get; set; }
        public SimpleEntity Customer { get; set; }
        public SimpleEntity Destination { get; set; }
        public SimpleEntity DocumentType { get; set; }
        public SimpleEntity Ship { get; set; }
        public SimpleEntity ShipOwner { get; set; }
        public decimal GrossAmount { get; set; }
        public bool IsEmailPending { get; set; }
        public bool IsEmailSent { get; set; }
        public InvoiceListAadeVM Aade { get; set; }

    }

}