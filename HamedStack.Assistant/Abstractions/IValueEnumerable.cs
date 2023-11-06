// ReSharper disable UnusedMember.Global

namespace HamedStack.Assistant.Abstractions;

/// <summary>
/// Represents a strongly-typed, read-only collection of elements that can be enumerated.
/// </summary>
/// <typeparam name="T">The type of the elements in the enumerable collection.</typeparam>
/// <typeparam name="TEnumerator">The type of the enumerator that iterates through the collection.</typeparam>
/// <remarks>
/// This interface is intended for scenarios where you want to avoid heap allocations when
/// enumerating, by requiring that the enumerator is a value type (struct).
/// </remarks>
public interface IValueEnumerable<out T, out TEnumerator> : IEnumerable<T>
    where TEnumerator : struct, IEnumerator<T>
{
    /// <summary>
    /// Returns a struct enumerator that iterates through the collection.
    /// </summary>
    /// <returns>A struct enumerator that can be used to iterate through the collection.</returns>
    /// <remarks>
    /// The enumerator is a value type to eliminate heap allocations during enumeration. This can be
    /// especially beneficial in performance-critical paths.
    /// </remarks>
    new TEnumerator GetEnumerator();
}