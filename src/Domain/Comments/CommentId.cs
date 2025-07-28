using Domain.Common;

namespace Domain.Comments;

/// <summary>
/// Typed Id. Represents the unique identifier for a comment.
/// Author: WMarcia
/// Date: 2025-07-28
/// </summary>
public sealed class CommentId : StronglyTypedId<CommentId>
{
    /// <summary>
    /// Constructor.
    /// Initializes a new instance of the CommentId class.
    /// </summary>
    /// <param name="value">The GUID value for the comment identifier.</param>
    public CommentId(Guid value) : base(value) { }
}