namespace HamedStack.Assistant.Implementations;

/// <summary>
/// Represents the detailed information of an enumeration item of type <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">The type of the enumeration.</typeparam>
public class EnumDetail<T> where T : Enum
{
    /// <summary>
    /// Gets or sets the description of the enumeration item.
    /// </summary>
    /// <value>The description of the enumeration item.</value>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the enumeration item.
    /// </summary>
    /// <value>The enumeration item of type <typeparamref name="T"/>.</value>
    public T? Item { get; set; }

    /// <summary>
    /// Gets or sets the name of the enumeration item.
    /// </summary>
    /// <value>The name of the enumeration item.</value>
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the numeric value of the enumeration item.
    /// </summary>
    /// <value>The numeric value of the enumeration item.</value>
    public long Value { get; set; }
}
