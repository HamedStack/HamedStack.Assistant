// ReSharper disable UnusedMember.Global

namespace HamedStack.Assistant.Abstractions;

/// <summary>
/// Defines properties for an object that can represent a node in a tree structure.
/// </summary>
/// <typeparam name="T">The type used for the identifier of the tree node.</typeparam>
public interface ITreeObject<T>
{
    /// <summary>
    /// Gets or sets the list of child nodes for this tree node.
    /// </summary>
    /// <value>A list of child tree nodes.</value>
    IList<ITreeObject<T>> Children { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier for this tree node.
    /// </summary>
    /// <value>The identifier of the tree node.</value>
    T Id { get; set; }

    /// <summary>
    /// Gets or sets the name or label of this tree node.
    /// </summary>
    /// <value>The name or label of the tree node.</value>
    string? Name { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the parent tree node, if any.
    /// </summary>
    /// <remarks>
    /// If this property is null, it indicates that this tree node is a root node.
    /// </remarks>
    /// <value>The identifier of the parent tree node or null if it's a root node.</value>
    T? ParentId { get; set; }
}
