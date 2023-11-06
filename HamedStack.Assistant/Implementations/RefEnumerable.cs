// ReSharper disable UnusedType.Global

namespace HamedStack.Assistant.Implementations;

/// <summary>
/// Represents a read-only reference-counted enumerable collection of elements.
/// </summary>
/// <typeparam name="T">The type of the elements.</typeparam>
public readonly struct RefEnumerable<T>
{
    /// <summary>
    /// Internal array that holds the data.
    /// </summary>
    private readonly T[] _value;

    /// <summary>
    /// Initializes a new instance of the <see cref="RefEnumerable{T}"/> struct.
    /// </summary>
    /// <param name="val">The array to enumerate.</param>
    public RefEnumerable(T[] val) => _value = val;

    /// <summary>
    /// Gets an enumerator to iterate over the array.
    /// </summary>
    /// <returns>An enumerator for the array.</returns>
    public Enumerator GetEnumerator() => new(_value);

    /// <summary>
    /// Represents an enumerator for <see cref="RefEnumerable{T}"/>.
    /// </summary>
    public struct Enumerator
    {
        /// <summary>
        /// Internal array to enumerate.
        /// </summary>
        private readonly T[] _value;

        /// <summary>
        /// Current index of the enumerator.
        /// </summary>
        private int _index;

        /// <summary>
        /// Initializes a new instance of the <see cref="Enumerator"/> struct.
        /// </summary>
        /// <param name="val">The array to enumerate.</param>
        public Enumerator(T[] val) => (_value, _index) = (val, -1);

        /// <summary>
        /// Gets the element at the current position of the enumerator as a reference.
        /// </summary>
        /// <value>The element at the current position of the enumerator.</value>
        /// <example>
        /// <code>
        ///var myArray = new int[] { 1, 2, 3, 4, 5 };
        ///var myRefEnumerable = new RefEnumerable&lt;int&gt;(myArray);
        ///foreach (ref var item in myRefEnumerable)
        ///{
        ///// Double each item
        ///item *= 2;
        ///}
        ///// myArray is now { 2, 4, 6, 8, 10 }
        /// </code>
        /// </example>
        public ref T Current => ref _value[_index];

        /// <summary>
        /// Advances the enumerator to the next element of the collection.
        /// </summary>
        /// <returns>
        /// True if the enumerator was successfully advanced to the next element; False if the
        /// enumerator has passed the end of the collection.
        /// </returns>
        public bool MoveNext() => ++_index < _value.Length;
    }
}