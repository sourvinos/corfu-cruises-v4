using API.Features.Sales.Invoices;
using API.Features.Sales.Receipts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Features.Sales.Transactions {

    internal class TransactionConfig : IEntityTypeConfiguration<TransactionsBase> {

        public void Configure(EntityTypeBuilder<TransactionsBase> entity) {
            entity.HasKey(x => x.InvoiceId);
            entity.HasDiscriminator<int>("DiscriminatorId")
                .HasValue<TransactionsBase>(0)
                .HasValue<Invoice>(1)
                .HasValue<Receipt>(2);
        }

    }

}