// ReSharper disable PropertyCanBeMadeInitOnly.Global

using System.Reflection;

namespace HamedStack.Assistant.Implementations.Internals;

/// <summary>
/// Represents a mapping between a source property and a destination property.
/// </summary>
internal class PropertyMap
{
    /// <summary>
    /// Gets or sets the destination property information.
    /// </summary>
    internal PropertyInfo? DestinationProperty { get; set; }

    /// <summary>
    /// Gets or sets the source property information.
    /// </summary>
    internal PropertyInfo? SourceProperty { get; set; }
}