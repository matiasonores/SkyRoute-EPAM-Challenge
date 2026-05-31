using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkyRoute.Domain.Entities;

namespace SkyRoute.Infraestructure.Persistence.Configurations
{
    public class PassengerConfiguration : IEntityTypeConfiguration<Passenger>
    {
        public void Configure(EntityTypeBuilder<Passenger> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.FullName)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(p => p.EmailAddress)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(p => p.NationalId)
                .HasMaxLength(8);

            builder.Property(p => p.PassportNumber)
                .HasMaxLength(9);

            builder.HasIndex(p => p.NationalId);
            builder.HasIndex(p => p.PassportNumber);
            builder.HasIndex(p => p.EmailAddress);
        }
    }
}
