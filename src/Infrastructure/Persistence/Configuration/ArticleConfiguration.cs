using Domain.Articles;
using Domain.Articles.ValueObjects;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration;

/// <summary>
/// Configuration class. Configures the Article entity for EF Core.
/// Author: WMarcia
/// Date: 2025-07-28
/// </summary>
internal class ArticleConfiguration : IEntityTypeConfiguration<Article>
{
    /// <summary>
    /// Configures the Article entity properties and relationships.
    /// </summary>
    /// <param name="builder">The entity type builder for Article.</param>
    public void Configure(EntityTypeBuilder<Article> builder)
    {
        builder.ToTable("Articles");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Id)
            .HasConversion(
                id => id.Value,
                value => new ArticleId(value))
            .ValueGeneratedNever();

        builder.Property(a => a.Title)
            .HasConversion(
                title => title.Value,
                value => new ArticleTitle(value))
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(a => a.Content)
            .HasConversion(
                content => content.Value,
                value => new ArticleContent(value))
            .HasMaxLength(10000)
            .IsRequired();

        builder.Property(a => a.AuthorId)
            .HasConversion(
                id => id.Value,
                value => new UserId(value))
            .IsRequired();

        builder.HasIndex(a => a.AuthorId);

        builder.Property(a => a.PublicationDate)
            .IsRequired();

        builder.HasOne(a => a.Author)
            .WithMany(u => u.Articles)
            .HasForeignKey(a => a.AuthorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(a => a.Comments)
            .WithOne()
            .HasForeignKey(c => c.ArticleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
