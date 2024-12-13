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

            builder.Navigation(c => c.Addresses)
                   .AutoInclude();

            builder.OwnsMany(c => c.Addresses, addressBuilder =>
            {
                addressBuilder.ToTable(nameof(Address));

                addressBuilder.Property(c => c.Country)
                .IsUnicode(true)
                .HasMaxLength(50)
                .IsRequired();

                addressBuilder.Property(c => c.City)
                .IsUnicode(true)
                .HasMaxLength(50)
                .IsRequired();

                addressBuilder.Property(c => c.Street)
                .IsUnicode(true)
                .HasMaxLength(50)
                .IsRequired();

                addressBuilder.Property(c => c.PostalCode)
                .HasConversion<EncryptionConvertor>()
                .HasMaxLength(100)
                .IsRequired();
            });

        });
    }
}
