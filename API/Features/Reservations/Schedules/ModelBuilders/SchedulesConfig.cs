using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Features.Reservations.Schedules {

    internal class SchedulesConfig : IEntityTypeConfiguration<Schedule> {

        public void Configure(EntityTypeBuilder<Schedule> entity) {
            // PK
            entity.Property(x => x.Id).ValueGeneratedOnAdd();
            // FKs
            entity.Property(x => x.PortId).IsRequired(true);
            entity.Property(x => x.DestinationId).IsRequired(true);
            // Fields
            entity.Property(x => x.Date).HasColumnType("date").HasMaxLength(10).IsRequired(true);
            entity.Property(x => x.MaxPax).HasDefaultValue(0);
            entity.Property(x => x.Time).HasMaxLength(5).IsRequired(true);
            entity.Property(x => x.IsActive);
            // Metadata
            entity.Property(x => x.PostAt).HasMaxLength(19).IsRequired(true);
            entity.Property(x => x.PostUser).HasMaxLength(255).IsRequired(true);
            entity.Property(x => x.PutAt).HasMaxLength(19);
            entity.Property(x => x.PutUser).HasMaxLength(255);
        }

    }

}