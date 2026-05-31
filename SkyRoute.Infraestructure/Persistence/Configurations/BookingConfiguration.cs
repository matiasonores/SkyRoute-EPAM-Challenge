using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkyRoute.Domain.Entities;
using SkyRoute.Domain.Enums;

namespace SkyRoute.Infraestructure.Persistence.Configurations
{
    // Booking-Passenger is N:N: the same passenger (identified by NationalId or PassportNumber)
    // can appear in multiple bookings over time. This avoids duplicating personal data per booking
    // and enables passenger history queries.
    public class BookingConfiguration : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder.HasKey(b => b.Id);

            builder.Property(b => b.BookingReference)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(b => b.FlightNumber)
                .HasMaxLength(20);

            builder.Property(b => b.Price)
                .HasColumnType("decimal(18,2)");

            builder.Property(b => b.TotalPrice)
                .HasColumnType("decimal(18,2)");

            builder.Property(b => b.Status)
                .HasConversion<int>()
                .HasDefaultValue(BookingStatus.Pending);

            builder.HasOne(b => b.Flight)
                .WithMany()
                .HasForeignKey(b => b.FlightId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(b => b.Passengers)
                .WithMany()
                .UsingEntity("BookingPassengers");

            builder.HasIndex(b => b.BookingReference)
                .IsUnique();

            builder.HasIndex(b => b.FlightId);
            builder.HasIndex(b => b.Status);
        }
    }
}
