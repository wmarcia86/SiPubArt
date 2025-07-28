namespace Domain.Common;

/// <summary>
/// Value Object. Provides a strongly-typed wrapper for Guid identifiers.
/// Author: WMarcia
/// Date: 2025-07-28
/// </summary>
public abstract class StronglyTypedId<T> where T : StronglyTypedId<T>
{
    public Guid Value { get; }

    /// <summary>
    /// Constructor.
    /// Initializes a new instance of the StronglyTypedId class.
    /// </summary>
    /// <param name="value">The Guid value for the identifier.</param>
    protected StronglyTypedId(Guid value)
    {
        Value = value;
    }

    public override string ToString() => Value.ToString();

    public override bool Equals(object? obj)
    {
        return obj is StronglyTypedId<T> other && Value.Equals(other.Value);
    }

    public override int GetHashCode() => Value.GetHashCode();

    public static bool operator ==(StronglyTypedId<T> left, StronglyTypedId<T> right) =>
        left?.Equals(right) ?? right is null;

    public static bool operator !=(StronglyTypedId<T> left, StronglyTypedId<T> right) =>
        !(left == right);
}