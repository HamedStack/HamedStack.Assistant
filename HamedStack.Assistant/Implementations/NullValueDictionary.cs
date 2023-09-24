using HamedStack.Assistant.Abstractions;

namespace HamedStack.Assistant.Implementations;

/// <summary>
/// Represents a dictionary that returns a default value when attempting to access a non-existent key.
/// </summary>
/// <typeparam name="TKey">The type of the keys in the dictionary. This cannot be null.</typeparam>
/// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
public class NullValueDictionary<TKey, TValue> : Dictionary<TKey, TValue>, INullValueDictionary<TKey, TValue> where TKey : notnull
{
    private readonly TValue? _defaultValue;

    /// <summary>
    /// Initializes a new instance of the <see cref="NullValueDictionary{TKey, TValue}"/> class with the default value for TValue.
    /// </summary>
    public NullValueDictionary()
    {
        _defaultValue = default;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="NullValueDictionary{TKey, TValue}"/> class with a specified default value.
    /// </summary>
    /// <param name="defaultValue">The value to use as the default when a key does not exist in the dictionary.</param>
    public NullValueDictionary(TValue? defaultValue)
    {
        _defaultValue = defaultValue;
    }

    /// <summary>
    /// Gets or sets the value associated with the specified key. Returns the specified default value or the default for TValue if the key does not exist.
    /// </summary>
    /// <param name="key">The key of the value to get or set.</param>
    /// <value>The value associated with the specified key or the default value.</value>
    public new TValue this[TKey key]
    {
        get => TryGetValue(key, out var val) ? val : _defaultValue!;
        set => base[key] = value;
    }
}