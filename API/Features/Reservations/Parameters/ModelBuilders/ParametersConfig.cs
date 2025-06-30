using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Features.Reservations.Parameters {

    internal class ParametersConfig : IEntityTypeConfiguration<ReservationParameter> {

        public void Configure(EntityTypeBuilder<ReservationParameter> entity) {
            // PK
            entity.Property(x => x.Id).ValueGeneratedOnAdd();
            // Fields
            entity.Property(x => x.ClosingTime).HasMaxLength(5).IsRequired(true);
            entity.Property(x => x.Phones).HasMaxLength(128).IsRequired(true);
            entity.Property(x => x.Email).HasMaxLength(128).IsRequired(true);
            // Metadata
            entity.Property(x => x.PostAt).HasMaxLength(19).IsRequired(true);
            entity.Property(x => x.PostUser).HasMaxLength(255).IsRequired(true);
            entity.Property(x => x.PutAt).HasMaxLength(19);
            entity.Property(x => x.PutUser).HasMaxLength(255);
        }

    }

}