using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkyRoute.Domain.Entities;

namespace SkyRoute.Infraestructure.Persistence.Configurations
{
    public class FlightConfiguration : IEntityTypeConfiguration<Flight>
    {
        public void Configure(EntityTypeBuilder<Flight> builder)
        {
            builder.HasKey(f => f.Id);

            builder.Property(f => f.Airline)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(f => f.Provider)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(f => f.FlightNumber)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(f => f.Departure)
                .IsRequired();

            builder.Property(f => f.Arrival)
                .IsRequired();

            // SQL Server has no native TimeSpan column — persist as ticks (bigint)
            builder.Property(f => f.Duration)
                .HasConversion(
                    v => v.Ticks,
                    v => TimeSpan.FromTicks(v))
                .HasColumnType("bigint");

            builder.Property(f => f.Price)
                .HasColumnType("decimal(18,2)");

            builder.Property(f => f.TotalPrice)
                .HasColumnType("decimal(18,2)");

            builder.Property(f => f.CabinClass)
                .HasConversion<int>();

            builder.Property(f => f.Status)
                .HasConversion<int>()
                .HasDefaultValue(SkyRoute.Domain.Enums.FlightStatus.Available);

            builder.HasOne(f => f.Origin)
                .WithMany()
                .HasForeignKey(f => f.OriginAirportId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(f => f.Destination)
                .WithMany()
                .HasForeignKey(f => f.DestinationAirportId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(f => f.FlightNumber);
            builder.HasIndex(f => f.Provider);
            builder.HasIndex(f => f.Departure);
            builder.HasIndex(f => f.OriginAirportId);
            builder.HasIndex(f => f.DestinationAirportId);
            builder.HasIndex(f => f.Status);
        }
    }
}
