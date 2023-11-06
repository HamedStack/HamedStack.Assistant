// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global

using System.Runtime.CompilerServices;

namespace HamedStack.Assistant.Implementations;

/// <summary>
/// Represents a thread-safe key-value store that holds weak references to both keys and values.
/// </summary>
/// <typeparam name="TKey">The type of keys in the store. Must be a class type.</typeparam>
/// <typeparam name="TValue">The type of values in the store. Must be a class type.</typeparam>
public class WeakRefKeyValueStore<TKey, TValue>
    where TKey : class
    where TValue : class
{
    /// <summary>
    /// Object used for synchronizing thread access.
    /// </summary>
    private readonly object _syncRoot = new();

    /// <summary>
    /// Internal storage using ConditionalWeakTable to hold weak references.
    /// </summary>
    private ConditionalWeakTable<TKey, TValue?> _storage = new();

    /// <summary>
    /// Gets or sets the value associated with the specified key.
    /// </summary>
    /// <param name="key">The key of the value to get or set.</param>
    /// <returns>
    /// The value associated with the specified key. Throws a KeyNotFoundException if the key does
    /// not exist in the store.
    /// </returns>
    public TValue? this[TKey key]
    {
        get
        {
            lock (_syncRoot)
            {
                if (TryGet(key, out var value))
                {
                    return value;
                }

                throw new KeyNotFoundException();
            }
        }
        set
        {
            lock (_syncRoot)
            {
                Set(key, value);
            }
        }
    }

    /// <summary>
    /// Adds a range of key-value pairs to the store.
    /// </summary>
    /// <param name="items">The items to add.</param>
    public void AddRange(IEnumerable<KeyValuePair<TKey, TValue?>> items)
    {
        lock (_syncRoot)
        {
            foreach (var item in items)
            {
                _storage.AddOrUpdate(item.Key, item.Value);
            }
        }
    }

    /// <summary>
    /// Removes all keys and values from the store.
    /// </summary>
    public void Clear()
    {
        lock (_syncRoot)
        {
            _storage = new ConditionalWeakTable<TKey, TValue?>();
        }
    }

    /// <summary>
    /// Determines whether the store contains the specified key.
    /// </summary>
    /// <param name="key">The key to locate in the store.</param>
    /// <returns>true if the store contains an element with the specified key; otherwise, false.</returns>
    public bool ContainsKey(TKey key)
    {
        lock (_syncRoot)
        {
            return _storage.TryGetValue(key, out _);
        }
    }

    /// <summary>
    /// Gets the value associated with the specified key, or adds it if the key does not exist.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <param name="valueFactory">A function to produce the value for the specified key.</param>
    /// <returns>The value for the specified key.</returns>
    public TValue? GetOrAdd(TKey key, Func<TKey, TValue> valueFactory)
    {
        lock (_syncRoot)
        {
            return _storage.GetValue(key, k => valueFactory(k));
        }
    }

    /// <summary>
    /// Removes the value with the specified key.
    /// </summary>
    /// <param name="key">The key of the element to remove.</param>
    /// <returns>true if the element is successfully found and removed; otherwise, false.</returns>
    public bool Remove(TKey key)
    {
        lock (_syncRoot)
        {
            return _storage.Remove(key);
        }
    }

    /// <summary>
    /// Removes a range of keys and their associated values from the store.
    /// </summary>
    /// <param name="keys">The keys to remove.</param>
    public void RemoveRange(IEnumerable<TKey> keys)
    {
        lock (_syncRoot)
        {
            foreach (var key in keys)
            {
                _storage.Remove(key);
            }
        }
    }

    /// <summary>
    /// Sets a key-value pair in the store.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <param name="value">The value to set. Nullable.</param>
    public void Set(TKey key, TValue? value)
    {
        lock (_syncRoot)
        {
            _storage.AddOrUpdate(key, value);
        }
    }

    /// <summary>
    /// Attempts to get the value associated with the specified key.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <param name="value">
    /// When this method returns, contains the value associated with the specified key, if found;
    /// otherwise, null.
    /// </param>
    /// <returns>true if the key was found; otherwise, false.</returns>
    public bool TryGet(TKey key, out TValue? value)
    {
        lock (_syncRoot)
        {
            return _storage.TryGetValue(key, out value);
        }
    }
}