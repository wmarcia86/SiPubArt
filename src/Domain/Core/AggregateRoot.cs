namespace Domain.Core;

/// <summary>
/// Entity. Represents the base class for aggregate roots in the domain model.
/// Author: WMarcia
/// Date: 2025-07-28
/// </summary>
public abstract class AggregateRoot
{
    private readonly List<DomainEvent> _domainEvents = new();

    /// <summary>
    /// Retrieves the collection of domain events.
    /// </summary>
    /// <returns>A read-only collection of domain events.</returns>
    public IReadOnlyCollection<DomainEvent> GetDomainEvents() => _domainEvents.AsReadOnly();

    /// <summary>
    /// Raises a new domain event.
    /// </summary>
    /// <param name="domainEvent">The domain event to raise.</param>
    protected void Raise(DomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    /// <summary>
    /// Clears all domain events from the aggregate root.
    /// </summary>
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}
