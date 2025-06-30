using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Features.Reservations.Reservations {

    internal class ReservationsConfig : IEntityTypeConfiguration<Reservation> {

        public void Configure(EntityTypeBuilder<Reservation> entity) {
            // PK
            entity.Property(x => x.ReservationId).IsFixedLength().HasMaxLength(36).IsRequired(true);
            // FKs
            entity.Property(x => x.CustomerId).IsRequired(true);
            entity.Property(x => x.DestinationId).IsRequired(true);
            entity.Property(x => x.PickupPointId).IsRequired(true);
            // Fields
            entity.Property(x => x.Date).HasColumnType("date").IsRequired(true);
            entity.Property(x => x.RefNo).HasDefaultValue("").HasMaxLength(11);
            entity.Property(x => x.TicketNo).HasMaxLength(128).IsRequired(true);
            entity.Property(x => x.Adults).HasDefaultValue(0).IsRequired(true);
            entity.Property(x => x.Kids).HasDefaultValue(0).IsRequired(true);
            entity.Property(x => x.Free).HasDefaultValue(0).IsRequired(true);
            entity.Property(x => x.TotalPax).HasComputedColumnSql("((`Adults` + `Kids`) + `Free`)", stored: false);
            entity.Property(x => x.Email).HasDefaultValue("").HasMaxLength(128);
            entity.Property(x => x.Phones).HasDefaultValue("").HasMaxLength(128);
            entity.Property(x => x.Remarks).HasDefaultValue("").HasMaxLength(128);
            // Metadata
            entity.Property(x => x.PostAt).HasMaxLength(19).IsRequired(true);
            entity.Property(x => x.PostUser).HasMaxLength(255).IsRequired(true);
            entity.Property(x => x.PutAt).HasMaxLength(19);
            entity.Property(x => x.PutUser).HasMaxLength(255);
        }

    }

}