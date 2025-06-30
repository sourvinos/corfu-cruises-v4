using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Features.Sales.Prices {

    internal class PricesConfig : IEntityTypeConfiguration<Price> {

        public void Configure(EntityTypeBuilder<Price> entity) {
            // PK
            entity.Property(x => x.Id).IsFixedLength().HasMaxLength(36).IsRequired(true);
            // FKs
            entity.Property(x => x.CustomerId).IsRequired(true);
            entity.Property(x => x.DestinationId).IsRequired(true);
            entity.Property(x => x.PortId).IsRequired(true);
            // Fields
            entity.Property(x => x.From).HasColumnType("date").IsRequired(true);
            entity.Property(x => x.To).HasColumnType("date").IsRequired(true);
            entity.Property(x => x.AdultsWithTransfer).HasDefaultValue(0).IsRequired(true);
            entity.Property(x => x.AdultsWithoutTransfer).HasDefaultValue(0).IsRequired(true);
            entity.Property(x => x.KidsWithTransfer).HasDefaultValue(0).IsRequired(true);
            entity.Property(x => x.KidsWithoutTransfer).HasDefaultValue(0).IsRequired(true);
            // Metadata
            entity.Property(x => x.PostAt).HasMaxLength(19).IsRequired(true);
            entity.Property(x => x.PostUser).HasMaxLength(255).IsRequired(true);
            entity.Property(x => x.PutAt).HasMaxLength(19);
            entity.Property(x => x.PutUser).HasMaxLength(255);

        }

    }

}