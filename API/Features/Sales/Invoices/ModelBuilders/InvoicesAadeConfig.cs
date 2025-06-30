using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Features.Sales.Invoices {

    internal class InvoicesAadeConfig : IEntityTypeConfiguration<InvoiceAade> {

        public void Configure(EntityTypeBuilder<InvoiceAade> entity) {
            // PK
            entity.HasKey("InvoiceId");
        }

    }

}