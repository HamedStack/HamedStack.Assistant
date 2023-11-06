// ReSharper disable UnusedMember.Global

namespace HamedStack.Assistant.Implementations;

/// <summary>
/// Provides an ad hoc way to implement equality checks for objects of type <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">The type of objects to compare.</typeparam>
public sealed class AdHocEquatable<T> : IEquatable<T>
{
    private readonly Func<T, bool> _equals;

    /// <summary>
    /// Initializes a new instance of the <see cref="AdHocEquatable{T}"/> class.
    /// </summary>
    /// <param name="eq">
    /// A function that determines equality for objects of type <typeparamref name="T"/>.
    /// </param>
    public AdHocEquatable(Func<T, bool> eq)
    {
        _equals = eq ?? throw new ArgumentNullException(nameof(eq));
    }

    /// <summary>
    /// Determines whether the specified object is equal to the current object.
    /// </summary>
    /// <param name="other">The object to compare with the current object.</param>
    /// <returns>
    /// <c>true</c> if the specified object is equal to the current object; otherwise, <c>false</c>.
    /// </returns>
    public bool Equals(T? other)
    {
        return other is not null && _equals(other);
    }
}