using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Features.Sales.BankAccounts {

    internal class BankAccountsConfig : IEntityTypeConfiguration<BankAccount> {

        public void Configure(EntityTypeBuilder<BankAccount> entity) {
            // PK
            entity.Property(x => x.Id).ValueGeneratedOnAdd();
            // FKs
            entity.Property(x => x.ShipOwnerId).IsRequired(true);
            entity.Property(x => x.BankId).IsRequired(true);
            // Fields
            entity.Property(x => x.Iban).HasMaxLength(128).IsRequired(true);
            entity.Property(x => x.IsActive);
            // Metadata
            entity.Property(x => x.PostAt).HasMaxLength(19).IsRequired(true);
            entity.Property(x => x.PostUser).HasMaxLength(255).IsRequired(true);
            entity.Property(x => x.PutAt).HasMaxLength(19);
            entity.Property(x => x.PutUser).HasMaxLength(255);
        }

    }

}