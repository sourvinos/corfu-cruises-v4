using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Features.Sales.PaymentMethods {

    internal class PaymentMethodsConfig : IEntityTypeConfiguration<PaymentMethod> {

        public void Configure(EntityTypeBuilder<PaymentMethod> entity) {
            // PK
            entity.Property(x => x.Id).ValueGeneratedOnAdd();
            // Fields
            entity.Property(x => x.Description).HasMaxLength(128).IsRequired(true);
            entity.Property(x => x.DescriptionEn).HasMaxLength(128).IsRequired(true);
            entity.Property(x => x.IsCash);
            entity.Property(x => x.IsDefault);
            entity.Property(x => x.IsActive);
            // Metadata
            entity.Property(x => x.PostAt).HasMaxLength(19).IsRequired(true);
            entity.Property(x => x.PostUser).HasMaxLength(255).IsRequired(true);
            entity.Property(x => x.PutAt).HasMaxLength(19);
            entity.Property(x => x.PutUser).HasMaxLength(255);
        }

    }

}