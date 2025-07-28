using Domain.Common;

namespace Domain.Users;

/// <summary>
/// Type Id. Represents the unique identifier for a user.
/// Author: WMarcia
/// Date: 2025-07-28
/// </summary>
public sealed class UserId : StronglyTypedId<UserId>
{
    /// <summary>
    /// Constructor.
    /// Initializes a new instance of the UserId class.
    /// </summary>
    /// <param name="value">The GUID value for the user identifier.</param>
    public UserId(Guid value) : base(value) { }
}
