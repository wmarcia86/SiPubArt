using Domain.Users;
using Domain.Users.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration;

/// <summary>
/// Configuration class. Configures the User entity for EF Core.
/// Author: WMarcia
/// Date: 2025-07-28
/// </summary>
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    /// <summary>
    /// Configures the User entity properties and relationships.
    /// </summary>
    /// <param name="builder">The entity type builder for User.</param>
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
            .HasConversion(
                userId => userId.Value,
                value => new UserId(value));

        builder.Property(u => u.FirstName)
            .HasConversion(
                firstName => firstName.Value,
                value => FirstName.Create(value).Value)
            .HasMaxLength(50);

        builder.Property(u => u.LastName)
            .HasConversion(
                lastName => lastName.Value,
                value => LastName.Create(value).Value)
            .HasMaxLength(50);

        builder.Property(u => u.Username)
            .HasConversion(
                userName => userName.Value,
                value => Username.Create(value).Value)
            .HasMaxLength(50);

        builder.HasIndex(u => u.Username).IsUnique();

        builder.Property(u => u.Email)
            .HasConversion(
                email => email.Value,
                value => Email.Create(value).Value)
            .HasMaxLength(254);

        builder.HasIndex(u => u.Email).IsUnique();

        builder.Property(u => u.Password)
            .HasConversion(
                password => password.Value,
                value => Password.Create(value).Value)
            .HasMaxLength(150);

        builder.Property(u => u.Role)
            .HasConversion<string>()
            .HasMaxLength(5);

        builder.Property(u => u.Active);

        builder.Ignore(u => u.FullName);

        builder.HasMany(u => u.Articles)
            .WithOne(a => a.Author)
            .HasForeignKey(a => a.AuthorId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}