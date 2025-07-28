namespace Domain.Core;

/// <summary>
/// Infrastructure interface. Defines the contract for the unit of work pattern.
/// Author: WMarcia
/// Date: 2025-07-28
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    /// Commits all changes to the data store asynchronously.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>The number of state entries written to the database.</returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
