using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Features.Reservations.PickupPoints {

    internal class PickupPointsConfig : IEntityTypeConfiguration<PickupPoint> {

        public void Configure(EntityTypeBuilder<PickupPoint> entity) {
            // PK
            entity.Property(x => x.Id).ValueGeneratedOnAdd();
            // FKs
            entity.Property(x => x.PortId).IsRequired(true);
            entity.Property(x => x.CoachRouteId).IsRequired(true);
            // Fields
            entity.Property(x => x.Description).HasMaxLength(128).IsRequired(true);
            entity.Property(x => x.ExactPoint).HasMaxLength(128).IsRequired(true);
            entity.Property(x => x.Time).HasMaxLength(5).IsRequired(true);
            entity.Property(x => x.Remarks).HasMaxLength(2048);
            entity.Property(x => x.IsActive);
            // Metadata
            entity.Property(x => x.PostAt).HasMaxLength(19).IsRequired(true);
            entity.Property(x => x.PostUser).HasMaxLength(255).IsRequired(true);
            entity.Property(x => x.PutAt).HasMaxLength(19);
            entity.Property(x => x.PutUser).HasMaxLength(255);
        }

    }

}