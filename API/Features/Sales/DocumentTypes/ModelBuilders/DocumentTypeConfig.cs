using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Features.Sales.DocumentTypes {

    internal class DocumentTypeConfig : IEntityTypeConfiguration<DocumentType> {

        public void Configure(EntityTypeBuilder<DocumentType> entity) {
            // PK
            entity.Property(x => x.Id).ValueGeneratedOnAdd();
            // FKs
            entity.Property(x => x.ShipOwnerId).IsRequired(true);
            // Fields
            entity.Property(x => x.Abbreviation).HasMaxLength(5).IsRequired(true);
            entity.Property(x => x.AbbreviationEn).HasMaxLength(5).IsRequired(true);
            entity.Property(x => x.Description).HasMaxLength(128).IsRequired(true);
            entity.Property(x => x.Batch).HasMaxLength(5).IsRequired(true);
            entity.Property(x => x.BatchEn).HasMaxLength(5).IsRequired(true);
            entity.Property(x => x.DiscriminatorId).IsRequired(true);
            entity.Property(x => x.IsActive);
            // Plus or Minus
            entity.Property(x => x.Customers).HasMaxLength(1).IsRequired(true);
            entity.Property(x => x.Suppliers).HasMaxLength(1);
            // myData
            entity.Property(x => x.IsMyData);
            entity.Property(x => x.Table8_1).HasMaxLength(32);
            entity.Property(x => x.Table8_8).HasMaxLength(32);
            entity.Property(x => x.Table8_9).HasMaxLength(32);
            // Metadata
            entity.Property(x => x.PostAt).HasMaxLength(19).IsRequired(true);
            entity.Property(x => x.PostUser).HasMaxLength(255).IsRequired(true);
            entity.Property(x => x.PutAt).HasMaxLength(19);
            entity.Property(x => x.PutUser).HasMaxLength(255);
        }

    }

}