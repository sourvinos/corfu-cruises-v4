using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Features.Reservations.ShipOwners {

    internal class ShipOwnersConfig : IEntityTypeConfiguration<ShipOwner> {

        public void Configure(EntityTypeBuilder<ShipOwner> entity) {
            // PK
            entity.Property(x => x.Id).ValueGeneratedOnAdd();
            // FKs
            entity.Property(x => x.NationalityId).IsRequired(true);
            entity.Property(x => x.TaxOfficeId).IsRequired(true);
            entity.Property(x => x.VatExemptionId).IsRequired(true);
            // Fields
            entity.Property(x => x.Description).HasMaxLength(128).IsRequired(true);
            entity.Property(x => x.DescriptionEn).HasMaxLength(128).IsRequired(true);
            entity.Property(x => x.VatPercent).HasDefaultValue(0).HasMaxLength(5);
            entity.Property(x => x.VatNumber).HasDefaultValue("").HasMaxLength(36);
            entity.Property(x => x.Profession).HasDefaultValue("").HasMaxLength(128);
            entity.Property(x => x.Street).HasDefaultValue("").HasMaxLength(128).IsRequired();
            entity.Property(x => x.Number).HasDefaultValue("").HasMaxLength(4).IsRequired();
            entity.Property(x => x.PostalCode).HasDefaultValue("").HasMaxLength(10).IsRequired();
            entity.Property(x => x.City).HasDefaultValue("").HasMaxLength(128).IsRequired();
            entity.Property(x => x.Phones).HasDefaultValue("").HasMaxLength(128);
            entity.Property(x => x.PersonInCharge).HasDefaultValue("").HasMaxLength(128);
            entity.Property(x => x.Email).HasDefaultValue("").HasMaxLength(128);
            entity.Property(x => x.IsActive).IsRequired(true);
            // Metadata
            entity.Property(x => x.PostAt).HasMaxLength(19).IsRequired(true);
            entity.Property(x => x.PostUser).HasMaxLength(255).IsRequired(true);
            entity.Property(x => x.PutAt).HasMaxLength(19);
            entity.Property(x => x.PutUser).HasMaxLength(255);
        }

    }

}