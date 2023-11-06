namespace HamedStack.Assistant.Abstractions;

/// <summary>
/// Defines methods and properties for a dictionary that can return a default value for non-existing keys.
/// </summary>
/// <typeparam name="TKey">The type of the keys in the dictionary. This cannot be null.</typeparam>
/// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
public interface INullValueDictionary<TKey, TValue> : IDictionary<TKey, TValue> where TKey : notnull
{
    /// <summary>
    /// Gets or sets the value associated with the specified key. Returns the default value if the
    /// key does not exist.
    /// </summary>
    /// <param name="key">The key of the value to get or set.</param>
    /// <value>The value associated with the specified key.</value>
    new TValue this[TKey key] { get; set; }
}