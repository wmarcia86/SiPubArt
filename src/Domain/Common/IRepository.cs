namespace Domain.Common;

/// <summary>
/// Infrastructure interface. Defines the contract for a generic repository.
/// Author: WMarcia
/// Date: 2025-07-28
/// </summary>
    public interface IRepository<TEntity, TId>
    where TEntity : class
    where TId : StronglyTypedId<TId>
{
    /// <summary>
    /// Adds a new entity to the repository.
    /// </summary>
    /// <param name="entity">The entity to add.</param>
    void Add(TEntity entity);

    /// <summary>
    /// Retrieves an entity by its identifier.
    /// </summary>
    /// <param name="id">The entity identifier.</param>
    /// <returns>The entity if found; otherwise, null.</returns>
    Task<TEntity?> GetByIdAsync(TId id);

    /// <summary>
    /// Checks if an entity exists by its identifier.
    /// </summary>
    /// <param name="id">The entity identifier.</param>
    /// <returns>True if the entity exists; otherwise, false.</returns>
    Task<bool> ExistsAsync(TId id);

    /// <summary>
    /// Retrieves all entities from the repository.
    /// </summary>
    /// <returns>A list of all entities.</returns>
    Task<List<TEntity>> GetAll();

    /// <summary>
    /// Gets the total number of entities in the repository.
    /// </summary>
    /// <returns>The count of entities.</returns>
    Task<int> Count();

    /// <summary>
    /// Retrieves a paged list of entities.
    /// </summary>
    /// <param name="pageNumber">The page number.</param>
    /// <param name="pageSize">The page size.</param>
    /// <returns>A list of entities for the specified page.</returns>
    Task<List<TEntity>> GetPaged(int pageNumber, int pageSize);

    /// <summary>
    /// Updates an existing entity in the repository.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    void Update(TEntity entity);

    /// <summary>
    /// Deletes an entity from the repository.
    /// </summary>
    /// <param name="entity">The entity to delete.</param>
    void Delete(TEntity entity);
}