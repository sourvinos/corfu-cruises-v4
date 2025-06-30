using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Features.Sales.Invoices {

    internal class InvoicesPortsConfig : IEntityTypeConfiguration<InvoicePort> {

        public void Configure(EntityTypeBuilder<InvoicePort> entity) {
            // PK
            entity.Property(x => x.InvoiceId).IsFixedLength().HasMaxLength(36).IsRequired(true);
            // FKs
            entity.Property(x => x.PortId).IsRequired(true);
            // Fields
            entity.Property(x => x.TotalPax).HasComputedColumnSql("(((((`AdultsWithTransfer` + `AdultsWithoutTransfer`) + `KidsWithTransfer`) + `KidsWithoutTransfer`) + `FreeWithTransfer`) + `FreeWithoutTransfer`)", stored: false);
            entity.Property(x => x.TotalAmount).HasComputedColumnSql("((((`AdultsWithTransfer` * `AdultsPriceWithTransfer`) + (`AdultsWithoutTransfer` * `AdultsPriceWithoutTransfer`)) + (`KidsWithTransfer` * `KidsPriceWithTransfer`)) + (`KidsWithoutTransfer` * `KidsPriceWithoutTransfer`))", stored: false);
        }

    }

}