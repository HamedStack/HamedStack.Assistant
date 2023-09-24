// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

namespace HamedStack.Assistant.Implementations;

/// <summary>
/// Represents the result of a similarity computation between a source string and a candidate string.
/// </summary>
public class SimilarityResult
{
    /// <summary>
    /// Gets or sets the candidate string that was compared to the source string.
    /// </summary>
    /// <value>The candidate string.</value>
    public string Candidate { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the computed similarity score between the source string and the candidate string.
    /// The score is a value between 0 and 1, where 1 indicates the strings are identical, and 0 indicates they are completely different.
    /// </summary>
    /// <value>The similarity score.</value>
    public double SimilarityScore { get; set; }
}
