using Microsoft.EntityFrameworkCore;
using Profile.Persistence.Convertors;

namespace Profile.Models;
public class ProfileDbContext(DbContextOptions<ProfileDbContext> options) : DbContext(options)
{
    public DbSet<Profile> Profiles {  get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Profile>(builder =>
        {
            builder.ToTable(nameof(Profile));

            builder.HasKey(c => c.UserId);

            builder.Property(c => c.UserId)
            .ValueGeneratedNever()
            .IsRequired();

            builder.Property(c => c.TimeZoneId)
            .IsUnicode(false)
            .IsRequired();

            builder.Property(c => c.DateFormat)
            .IsUnicode(false)
            .HasMaxLength(20)
            .IsRequired();

            builder.Property(c => c.TimeFormat)
            .IsUnicode(false)
            .HasMaxLength(20)
            .IsRequired();

            builder.OwnsMany(c => c.Addresses, addressBuilder =>
            {
                addressBuilder.ToTable(nameof(Address));

                addressBuilder.Property(c => c.Country)
                .IsRequired();

                addressBuilder.Property(c => c.City)
                .IsRequired();

                addressBuilder.Property(c => c.Street)
                .IsRequired();

                addressBuilder.Property(c => c.PostalCode)
                .HasConversion<EncryptionConvertor>()
                .IsRequired();
            });

        });
    }
}
