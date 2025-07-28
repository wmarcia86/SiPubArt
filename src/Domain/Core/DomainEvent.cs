using MediatR;

namespace Domain.Core;

/// <summary>
/// Entity. Represents a domain event for the domain model.
/// Author: WMarcia
/// Date: 2025-07-28
/// </summary>
public record DomainEvent(Guid Id) : INotification;
