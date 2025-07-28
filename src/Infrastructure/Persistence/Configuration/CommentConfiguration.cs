using Domain.Articles;
using Domain.Comments;
using Domain.Comments.ValueObjects;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration;

/// <summary>
/// Configuration class. Configures the Comment entity for EF Core.
/// Author: WMarcia
/// Date: 2025-07-28
/// </summary>
internal class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
    /// <summary>
    /// Configures the Comment entity properties and relationships.
    /// </summary>
    /// <param name="builder">The entity type builder for Comment.</param>
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.ToTable("Comments");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .HasConversion(
                id => id.Value,
                value => new CommentId(value))
            .ValueGeneratedNever();

        builder.Property(c => c.ArticleId)
            .HasConversion(
                id => id.Value,
                value => new ArticleId(value))
            .IsRequired();

        builder.HasIndex(c => c.ArticleId);

        builder.Property(c => c.Content)
            .HasConversion(
                content => content.Value,
                value => new CommentContent(value))
            .HasMaxLength(2000)
            .IsRequired();

        builder.Property(c => c.AuthorId)
            .HasConversion(
                id => id.Value,
                value => new UserId(value))
            .IsRequired();

        builder.HasIndex(c => c.AuthorId);

        builder.Property(c => c.PublicationDate)
            .IsRequired();

        builder.HasOne<Article>()
            .WithMany(a => a.Comments)
            .HasForeignKey(c => c.ArticleId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(c => c.Author)
            .WithMany()
            .HasForeignKey(c => c.AuthorId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
