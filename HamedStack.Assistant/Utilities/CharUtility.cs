// ReSharper disable UnusedMember.Global
namespace HamedStack.Assistant.Utilities;

/// <summary>
/// Provides utility methods for extracting specific subsets of characters.
/// </summary>
public static class CharUtility
{
    /// <summary>
    /// Retrieves all digit characters from the printable characters set.
    /// </summary>
    /// <returns>A collection of digit characters.</returns>
    public static IEnumerable<char> GetAllDigits()
    {
        var chars = GetAllPrintableChars()
            .Where(char.IsDigit)
            .ToArray();
        return chars;
    }

    /// <summary>
    /// Retrieves all letter characters from the printable characters set.
    /// </summary>
    /// <returns>A collection of letter characters.</returns>
    public static IEnumerable<char> GetAllLetters()
    {
        var chars = GetAllPrintableChars()
            .Where(char.IsLetter)
            .ToArray();
        return chars;
    }

    /// <summary>
    /// Retrieves all letter and digit characters from the entire Unicode character set.
    /// </summary>
    /// <returns>A collection of characters that are either letters or digits.</returns>
    public static IEnumerable<char> GetAllLettersAndDigits()
    {
        var chars = GetAllUnicodeChars()
            .Where(char.IsLetterOrDigit)
            .ToArray();
        return chars;
    }

    /// <summary>
    /// Retrieves all characters from the Unicode character set excluding control and whitespace characters.
    /// </summary>
    /// <returns>A collection of printable characters.</returns>
    public static IEnumerable<char> GetAllPrintableChars()
    {
        var chars = GetAllUnicodeChars()
            .Where(c => !char.IsControl(c) && !char.IsWhiteSpace(c))
            .ToArray();
        return chars;
    }

    /// <summary>
    /// Retrieves all characters from the entire Unicode character set.
    /// </summary>
    /// <returns>A collection of all Unicode characters.</returns>
    public static IEnumerable<char> GetAllUnicodeChars()
    {
        var chars = Enumerable.Range(0, char.MaxValue + 1)
            .Select(i => (char)i)
            .ToArray();
        return chars;
    }
}