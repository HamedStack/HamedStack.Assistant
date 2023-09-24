// ReSharper disable IdentifierTypo

namespace HamedStack.Assistant.Enums;

/// <summary>
/// Represents the different algorithms available for computing string similarity.
/// </summary>
public enum SimilarityAlgorithm
{
    /// <summary>
    /// Represents the Levenshtein distance algorithm, which calculates the minimum number of single-character edits (insertions, deletions, or substitutions) required to change one string into the other.
    /// </summary>
    Levenshtein,

    /// <summary>
    /// Represents the Jaro similarity algorithm, which is a measure of similarity between two strings. It is defined as the mean of the proportion of matched characters from each string.
    /// </summary>
    Jaro,

    /// <summary>
    /// Represents the Jaro-Winkler similarity algorithm, which is an extension of the Jaro similarity that gives more weight to the prefix of the strings.
    /// </summary>
    JaroWinkler
}