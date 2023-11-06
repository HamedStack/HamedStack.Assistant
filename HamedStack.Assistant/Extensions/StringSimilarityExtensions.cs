// ReSharper disable UnusedMember.Global
// ReSharper disable IdentifierTypo
// ReSharper disable CheckNamespace

using HamedStack.Assistant.Enums;
using HamedStack.Assistant.Extensions.EnumerableExtended;
using HamedStack.Assistant.Implementations;

namespace HamedStack.Assistant.Extensions.StringExtended;

/// <summary>
/// Provides methods to compute string similarity using various algorithms.
/// </summary>
public static class StringSimilarityExtensions
{
    /// <summary>
    /// Computes the Jaro similarity between two strings.
    /// </summary>
    /// <param name="first">The first string to compare.</param>
    /// <param name="second">The second string to compare.</param>
    /// <returns>
    /// A value between 0 and 1 representing the similarity. 1 indicates the strings are identical,
    /// and 0 indicates they are completely different.
    /// </returns>
    public static double ComputeJaroSimilarity(this string first, string second)
    {
        if (first == null) throw new ArgumentNullException(nameof(first));
        if (second == null) throw new ArgumentNullException(nameof(second));

        var sLen = first.Length;
        var tLen = second.Length;

        if (sLen == 0 && tLen == 0) return 1;

        var matchDistance = Math.Max(sLen, tLen) / 2 - 1;

        var sMatches = new bool[sLen];
        var tMatches = new bool[tLen];

        var matches = 0;
        var transpositions = 0;

        for (var i = 0; i < sLen; i++)
        {
            var start = Math.Max(0, i - matchDistance);
            var end = Math.Min(i + matchDistance + 1, tLen);

            for (var j = start; j < end; j++)
            {
                if (tMatches[j]) continue;
                if (first[i] != second[j]) continue;
                sMatches[i] = true;
                tMatches[j] = true;
                matches++;
                break;
            }
        }

        if (matches == 0) return 0;

        var k = 0;
        for (var i = 0; i < sLen; i++)
        {
            if (!sMatches[i]) continue;
            while (!tMatches[k]) k++;
            if (first[i] != second[k]) transpositions++;
            k++;
        }

        var transpositionFraction = transpositions / 2.0;
        var jaro = ((double)matches / sLen + (double)matches / tLen + (matches - transpositionFraction) / matches) / 3;
        return jaro;
    }

    /// <summary>
    /// Computes the Jaro-Winkler similarity between two strings.
    /// </summary>
    /// <param name="first">The first string to compare.</param>
    /// <param name="second">The second string to compare.</param>
    /// <param name="scaling">The scaling factor for the Jaro-Winkler similarity. Defaults to 0.1.</param>
    /// <returns>
    /// A value between 0 and 1 representing the similarity. 1 indicates the strings are identical,
    /// and 0 indicates they are completely different.
    /// </returns>
    public static double ComputeJaroWinklerSimilarity(this string first, string second, double scaling = 0.1)
    {
        if (first == null) throw new ArgumentNullException(nameof(first));
        if (second == null) throw new ArgumentNullException(nameof(second));

        var jaroSimilarity = ComputeJaroSimilarity(first, second);

        var prefixLength = 0;
        for (var i = 0; i < Math.Min(first.Length, second.Length) && first[i] == second[i] && prefixLength < 4; i++)
        {
            prefixLength++;
        }

        return jaroSimilarity + prefixLength * scaling * (1 - jaroSimilarity);
    }

    /// <summary>
    /// Computes the least similar string from a list of strings to the given string using the
    /// specified similarity algorithm.
    /// </summary>
    /// <param name="source">The source string to compare.</param>
    /// <param name="candidates">A list of strings to compare against the source string.</param>
    /// <param name="similarityAlgorithm">The similarity algorithm to use. Defaults to Levenshtein.</param>
    /// <returns>The least similar string from the list of candidates.</returns>
    public static string ComputeLeastSimilar(this string source, IEnumerable<string?> candidates, SimilarityAlgorithm similarityAlgorithm = SimilarityAlgorithm.Levenshtein)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (candidates == null) throw new ArgumentNullException(nameof(candidates));

        var minSimilarity = double.MaxValue;
        var leastSimilarString = string.Empty;

        foreach (var candidate in candidates)
        {
            if (candidate == null)
            {
                continue;
            }
            var similarity = source.ComputeSimilarity(candidate, similarityAlgorithm);
            if (similarity < minSimilarity)
            {
                minSimilarity = similarity;
                leastSimilarString = candidate;
            }
        }
        return leastSimilarString;
    }

    /// <summary>
    /// Computes the similarity between two strings using the Levenshtein distance.
    /// </summary>
    /// <param name="first">The first string to compare.</param>
    /// <param name="second">The second string to compare.</param>
    /// <returns>
    /// A value between 0 and 1 representing the similarity. 1 indicates the strings are identical,
    /// and 0 indicates they are completely different.
    /// </returns>
    public static double ComputeLevenshteinSimilarity(this string first, string second)
    {
        if (first == null) throw new ArgumentNullException(nameof(first));
        if (second == null) throw new ArgumentNullException(nameof(second));

        var maxLen = Math.Max(first.Length, second.Length);
        if (maxLen == 0)
            return 1.0;

        var distance = ComputeLevenshteinDistance(first, second);
        return 1.0 - (double)distance / maxLen;

        static int ComputeLevenshteinDistance(string f, string? s)
        {
            if (string.IsNullOrEmpty(f))
                return string.IsNullOrEmpty(s) ? 0 : s.Length;

            if (string.IsNullOrEmpty(s))
                return f.Length;

            var distances = new int[f.Length + 1, s.Length + 1];

            for (var i = 0; i <= f.Length; i++)
                distances[i, 0] = i;

            for (var j = 0; j <= s.Length; j++)
                distances[0, j] = j;

            for (var i = 1; i <= f.Length; i++)
            {
                for (var j = 1; j <= s.Length; j++)
                {
                    var cost = f[i - 1] == s[j - 1] ? 0 : 1;
                    distances[i, j] = Math.Min(Math.Min(distances[i - 1, j] + 1, distances[i, j - 1] + 1), distances[i - 1, j - 1] + cost);
                }
            }

            return distances[f.Length, s.Length];
        }
    }

    /// <summary>
    /// Computes the most similar string from a list of strings to the given string using the
    /// specified similarity algorithm.
    /// </summary>
    /// <param name="source">The source string to compare.</param>
    /// <param name="candidates">A list of strings to compare against the source string.</param>
    /// <param name="similarityAlgorithm">The similarity algorithm to use. Defaults to Levenshtein.</param>
    /// <returns>The most similar string from the list of candidates.</returns>
    public static string ComputeMostSimilar(this string source, IEnumerable<string?> candidates, SimilarityAlgorithm similarityAlgorithm = SimilarityAlgorithm.Levenshtein)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (candidates == null) throw new ArgumentNullException(nameof(candidates));

        var maxSimilarity = double.MinValue;
        var mostSimilarString = string.Empty;

        foreach (var candidate in candidates)
        {
            if (candidate == null)
            {
                continue;
            }
            var similarity = source.ComputeSimilarity(candidate, similarityAlgorithm);
            if (similarity > maxSimilarity)
            {
                maxSimilarity = similarity;
                mostSimilarString = candidate;
            }
        }

        return mostSimilarString;
    }

    /// <summary>
    /// Computes the similarity between two strings using the specified similarity algorithm.
    /// </summary>
    /// <param name="first">The first string to compare.</param>
    /// <param name="second">The second string to compare.</param>
    /// <param name="similarityAlgorithm">
    /// The similarity algorithm to use for comparison. Default is Levenshtein.
    /// </param>
    /// <returns>The similarity score between the two strings.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when either of the input strings is null.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when an unsupported similarity algorithm is provided.
    /// </exception>
    public static double ComputeSimilarity(this string first, string second, SimilarityAlgorithm similarityAlgorithm = SimilarityAlgorithm.Levenshtein)
    {
        if (first == null) throw new ArgumentNullException(nameof(first));
        if (second == null) throw new ArgumentNullException(nameof(second));

        return similarityAlgorithm switch
        {
            SimilarityAlgorithm.Levenshtein => ComputeLevenshteinSimilarity(first, second),
            SimilarityAlgorithm.Jaro => ComputeJaroSimilarity(first, second),
            SimilarityAlgorithm.JaroWinkler => ComputeJaroWinklerSimilarity(first, second),
            _ => throw new ArgumentOutOfRangeException(nameof(similarityAlgorithm), similarityAlgorithm, null)
        };
    }

    /// <summary>
    /// Computes the similarity for each string in a list of strings compared to the source string.
    /// </summary>
    /// <param name="source">The source string to compare.</param>
    /// <param name="candidates">A list of strings to compare against the source string.</param>
    /// <param name="similarityAlgorithm">The similarity algorithm to use. Defaults to Levenshtein.</param>
    /// <returns>
    /// A list of results containing each candidate string and its computed similarity score.
    /// </returns>
    public static IEnumerable<SimilarityResult> ComputeSimilarityForEach(this string source, IEnumerable<string> candidates, SimilarityAlgorithm similarityAlgorithm = SimilarityAlgorithm.Levenshtein)
    {
        var results = new List<SimilarityResult>();

        foreach (var candidate in candidates)
        {
            var similarity = source.ComputeSimilarity(candidate, similarityAlgorithm);
            results.Add(new SimilarityResult
            {
                Candidate = candidate,
                SimilarityScore = similarity
            });
        }

        return results;
    }

    /// <summary>
    /// Filters an array of strings to return those that contain the search string with a similarity
    /// score above the specified threshold.
    /// </summary>
    /// <param name="search">The string to search for.</param>
    /// <param name="source">The source array of strings.</param>
    /// <param name="threshold">The similarity score threshold. Default is 0.7.</param>
    /// <param name="similarityAlgorithm">The similarity algorithm to use. Default is Levenshtein.</param>
    /// <param name="manipulator">An optional function to manipulate each string before comparison.</param>
    /// <returns>An IEnumerable of strings that match the search criteria.</returns>
    public static IEnumerable<string> ContainsFuzzy(this string search, string[] source, double threshold = 0.7,
        SimilarityAlgorithm similarityAlgorithm = SimilarityAlgorithm.Levenshtein, Func<string, string>? manipulator = null)
    {
        foreach (var item in source)
        {
            var data = manipulator == null ? item : manipulator(item ?? throw new InvalidOperationException());
            var status = data.ContainsFuzzy(search, threshold, similarityAlgorithm);
            if (status)
            {
                yield return item;
            }
        }
    }

    /// <summary>
    /// Determines if the source string contains the search string with a similarity score above the
    /// specified threshold.
    /// </summary>
    /// <param name="search">The string to search for.</param>
    /// <param name="source">The source string.</param>
    /// <param name="threshold">The similarity score threshold. Default is 0.7.</param>
    /// <param name="similarityAlgorithm">The similarity algorithm to use. Default is Levenshtein.</param>
    /// <param name="manipulator">An optional function to manipulate the source string before comparison.</param>
    /// <returns>
    /// True if the source string contains the search string with a similarity score above the
    /// threshold, otherwise false.
    /// </returns>
    public static bool ContainsFuzzy(this string search, string source, double threshold = 0.7,
        SimilarityAlgorithm similarityAlgorithm = SimilarityAlgorithm.Levenshtein, Func<string, string>? manipulator = null)
    {
        if (source.ContainsIgnoreCase(search)) return true;

        var words = source.Split(' ').Select(x => x.Trim()).ToList();
        if (manipulator != null)
        {
            words = words.Select(manipulator).ToList();
        }

        if (words.ContainsIgnoreCase(search)) return true;
        foreach (var word in words)
        {
            var score = search.ToLowerInvariant().ComputeSimilarity(word.ToLowerInvariant(), similarityAlgorithm);
            if (score >= threshold)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Filters an IEnumerable of strings to return those that contain the search string with a
    /// similarity score above the specified threshold.
    /// </summary>
    /// <param name="search">The string to search for.</param>
    /// <param name="source">The source IEnumerable of strings.</param>
    /// <param name="threshold">The similarity score threshold. Default is 0.7.</param>
    /// <param name="similarityAlgorithm">The similarity algorithm to use. Default is Levenshtein.</param>
    /// <param name="manipulator">An optional function to manipulate each string before comparison.</param>
    /// <returns>An IEnumerable of strings that match the search criteria.</returns>

    public static IEnumerable<string> ContainsFuzzy(this string search, IEnumerable<string> source,
        double threshold = 0.7, SimilarityAlgorithm similarityAlgorithm = SimilarityAlgorithm.Levenshtein,
        Func<string, string>? manipulator = null)
    {
        foreach (var item in source)
        {
            var data = manipulator == null ? item : manipulator(item ?? throw new InvalidOperationException());
            var status = data.ContainsFuzzy(search, threshold, similarityAlgorithm);
            if (status)
            {
                yield return item;
            }
        }
    }
}