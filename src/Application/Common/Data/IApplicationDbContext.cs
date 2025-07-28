using Domain.Articles;
using Domain.Comments;
using Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Data;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; set; }

    DbSet<Article> Articles { get; set; }

    DbSet<Comment> Comments { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
