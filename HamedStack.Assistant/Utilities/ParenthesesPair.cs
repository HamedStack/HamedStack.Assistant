namespace HamedStack.Assistant.Utilities;

/// <summary>
/// Represents a pair of parentheses within a string or expression, providing details like its position, depth, and the value contained within.
/// </summary>
public struct ParenthesesPair
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ParenthesesPair"/> struct.
    /// </summary>
    /// <param name="id">A unique identifier for the parentheses pair.</param>
    /// <param name="startIndex">The starting index of the parentheses pair in the string or expression.</param>
    /// <param name="endIndex">The ending index of the parentheses pair in the string or expression.</param>
    /// <param name="depth">The depth or nesting level of the parentheses pair.</param>
    /// <param name="value">The value or content within the parentheses pair.</param>
    /// <exception cref="ArgumentException">Thrown when startIndex is greater than endIndex.</exception>
    public ParenthesesPair(int id, int startIndex, int endIndex, int depth, string value)
    {
        if (startIndex > endIndex)
            throw new ArgumentException("startIndex must be less than endIndex");

        StartIndex = startIndex;
        EndIndex = endIndex;
        Depth = depth;
        Value = value;
        Id = id;
    }

    /// <summary>
    /// Gets the depth or nesting level of the parentheses pair.
    /// </summary>
    public int Depth { get; }

    /// <summary>
    /// Gets the ending index of the parentheses pair in the string or expression.
    /// </summary>
    public int EndIndex { get; }

    /// <summary>
    /// Gets a unique identifier for the parentheses pair.
    /// </summary>
    public int Id { get; }

    /// <summary>
    /// Gets the starting index of the parentheses pair in the string or expression.
    /// </summary>
    public int StartIndex { get; }

    /// <summary>
    /// Gets the value or content within the parentheses pair.
    /// </summary>
    public string Value { get; }
}