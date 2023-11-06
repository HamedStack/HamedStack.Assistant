// ReSharper disable CheckNamespace
// ReSharper disable UnusedMember.Global

using System.Runtime.InteropServices;

namespace HamedStack.Assistant.Extensions.RefExtended;

/// <summary>
/// Provides extension methods for operations on collections.
/// </summary>
public static class RefExtensions
{
    /// <summary>
    /// Delegate for actions performed on a reference to a value of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="value">The reference to the value of type <typeparamref name="T"/>.</param>
    public delegate void RefAction<T>(ref T value) where T : struct;

    /// <summary>
    /// Performs the specified action on each item of the <see cref="List{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the list.</typeparam>
    /// <param name="list">The list on which the action is performed.</param>
    /// <param name="action">The action to perform on each element of the <see cref="List{T}"/>.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when the <paramref name="list"/> or the <paramref name="action"/> is null.
    /// </exception>
    /// <example>
    /// <code>
    ///var list = new List&lt;PointStruct&gt;() {
    ///new PointStruct(1, 10),
    ///new PointStruct(2, 20),
    ///new PointStruct(3, 30)
    ///};
    ///list.ForEachRef(static (ref PointStruct p) =&gt; p.Swap());
    ///foreach (var p in list) { Console.WriteLine(p); }
    /// </code>
    /// </example>
    public static void ForEachRef<T>(this List<T> list, RefAction<T> action) where T : struct
    {
        if (list is null) throw new ArgumentNullException(nameof(list));
        if (action is null) throw new ArgumentNullException(nameof(action));
        var span = CollectionsMarshal.AsSpan(list);
        foreach (ref var item in span)
            action(ref item);
    }
}