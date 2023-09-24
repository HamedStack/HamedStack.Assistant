
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

using System.ComponentModel;
using System.Reflection;
using HamedStack.Assistant.Implementations;

namespace HamedStack.Assistant.Utilities;

/// <summary>
/// Provides utility methods for operations on enumerations.
/// </summary>
public static class EnumUtility
{
    /// <summary>
    /// Determines if a specified enumeration of type <typeparamref name="TEnum"/> contains a given name.
    /// </summary>
    /// <param name="name">The name to check.</param>
    /// <param name="ignoreCase">Determines if the comparison should be case insensitive.</param>
    /// <typeparam name="TEnum">The enumeration type.</typeparam>
    /// <returns>True if the name exists, false otherwise.</returns>
    public static bool ContainsName<TEnum>(string? name, bool ignoreCase = false) where TEnum : Enum
    {
        if (name == null) return false;
        var stringComparison = ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
        return GetNames<TEnum>().Any(item => item.Contains(name, stringComparison));
    }

    /// <summary>
    /// Determines if a specified enumeration of type <typeparamref name="TEnum"/> contains a given value as a string.
    /// </summary>
    /// <param name="value">The value to check.</param>
    /// <param name="ignoreCase">Determines if the comparison should be case insensitive.</param>
    /// <typeparam name="TEnum">The enumeration type.</typeparam>
    /// <returns>True if the value exists, false otherwise.</returns>
    public static bool ContainsValue<TEnum>(string? value, bool ignoreCase = false) where TEnum : Enum
    {
        if (value == null) return false;
        var stringComparison = ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
        return GetValuesAsString<TEnum>().Any(item => item.Contains(value, stringComparison));
    }

    /// <summary>
    /// Determines if the specified enumeration of type <typeparamref name="TEnum"/> contains a given enumeration value.
    /// </summary>
    /// <typeparam name="TEnum">The type of the enumeration.</typeparam>
    /// <param name="value">The enumeration value to check.</param>
    /// <returns>true if the enumeration contains the value; otherwise, false.</returns>
    public static bool ContainsValue<TEnum>(TEnum value) where TEnum : Enum
    {
        return GetValues<TEnum>().Contains(value);
    }

    /// <summary>
    /// Gets all the descriptions of the values in an enumeration of type <typeparamref name="TEnum"/>.
    /// </summary>
    /// <typeparam name="TEnum">The enumeration type.</typeparam>
    /// <param name="replaceNullWithEnumName">If true, replaces null descriptions with the enumeration name.</param>
    /// <returns>An enumeration of descriptions.</returns>
    public static IEnumerable<string> GetDescriptions<TEnum>(bool replaceNullWithEnumName = false) where TEnum : Enum
    {
        return GetValues<TEnum>().Select(e => e.GetDescription(replaceNullWithEnumName)).Where(x => x != null)!;
    }

    /// <summary>
    /// Retrieves a list of details for each value in the specified enumeration of type <typeparamref name="TEnum"/>.
    /// </summary>
    /// <typeparam name="TEnum">The type of the enumeration.</typeparam>
    /// <returns>A collection of enumeration details.</returns>
    public static IEnumerable<EnumDetail<TEnum>> GetEnumDetails<TEnum>() where TEnum : Enum
    {
        var result = new List<EnumDetail<TEnum>>();
        var names = Enum.GetNames(typeof(TEnum));
        foreach (var name in names)
        {
            var parsed = Enum.Parse(typeof(TEnum), name);
            var item = (TEnum)parsed;
            var value = Convert.ToInt64(parsed);
            var description = item.GetDescription();
            result.Add(new EnumDetail<TEnum>
            {
                Name = name,
                Value = value,
                Description = description,
                Item = item
            });
        }

        return result;
    }

    /// <summary>
    /// Retrieves a filtered list of details for each value in the specified enumeration of type <typeparamref name="TEnum"/>.
    /// </summary>
    /// <typeparam name="TEnum">The type of the enumeration.</typeparam>
    /// <param name="predicate">The function to test each enumeration detail for a condition.</param>
    /// <returns>A collection of filtered enumeration details.</returns>
    public static IEnumerable<EnumDetail<TEnum>> GetEnumDetails<TEnum>(Func<EnumDetail<TEnum>, bool> predicate) where TEnum : Enum
    {
        var result = GetEnumDetails<TEnum>().Where(predicate);
        return result;
    }

    /// <summary>
    /// Retrieves the names of all values in the specified enumeration of type <typeparamref name="TEnum"/>.
    /// </summary>
    /// <typeparam name="TEnum">The type of the enumeration.</typeparam>
    /// <returns>A collection of names.</returns>
    public static IEnumerable<string> GetNames<TEnum>() where TEnum : Enum
    {
        return Enum.GetNames(typeof(TEnum));
    }

    /// <summary>
    /// Retrieves all values in the specified enumeration of type <typeparamref name="TEnum"/>.
    /// </summary>
    /// <typeparam name="TEnum">The type of the enumeration.</typeparam>
    /// <returns>A collection of enumeration values.</returns>
    public static IEnumerable<TEnum> GetValues<TEnum>() where TEnum : Enum
    {
        return (TEnum[])Enum.GetValues(typeof(TEnum));
    }

    /// <summary>
    /// Retrieves the string representations of all values in the specified enumeration of type <typeparamref name="TEnum"/>.
    /// </summary>
    /// <typeparam name="TEnum">The type of the enumeration.</typeparam>
    /// <returns>A collection of string representations of the enumeration values.</returns>
    public static IEnumerable<string> GetValuesAsString<TEnum>() where TEnum : Enum
    {
        return GetValues<TEnum>().Select(e => e.ToString());
    }

    /// <summary>
    /// Determines if the specified name is defined in the enumeration of type <typeparamref name="TEnum"/>.
    /// </summary>
    /// <typeparam name="TEnum">The type of the enumeration.</typeparam>
    /// <param name="name">The name to check.</param>
    /// <returns>true if the name is defined in the enumeration; otherwise, false.</returns>
    public static bool IsDefined<TEnum>(this string name) where TEnum : Enum
    {
        return Enum.IsDefined(typeof(TEnum), name);
    }

    /// <summary>
    /// Determines if the specified value is defined in the enumeration of type <typeparamref name="TEnum"/>.
    /// </summary>
    /// <typeparam name="TEnum">The type of the enumeration.</typeparam>
    /// <param name="value">The enumeration value to check.</param>
    /// <returns>true if the value is defined in the enumeration; otherwise, false.</returns>
    public static bool IsDefined<TEnum>(this TEnum value) where TEnum : Enum
    {
        return Enum.IsDefined(typeof(TEnum), value);
    }

    /// <summary>
    /// Determines if the specified string value is present in the enumeration of type <typeparamref name="TEnum"/>.
    /// </summary>
    /// <typeparam name="TEnum">The type of the enumeration.</typeparam>
    /// <param name="value">The string value to check.</param>
    /// <param name="ignoreCase">If set to true, performs a case-insensitive comparison. Default is false.</param>
    /// <returns>true if the string value is present in the enumeration; otherwise, false.</returns>
    public static bool IsInEnum<TEnum>(this string value, bool ignoreCase = false) where TEnum : Enum
    {
        var enums = GetValuesAsString<TEnum>().Select(e => ignoreCase ? e.ToLower() : e);
        return enums.Contains(ignoreCase ? value.ToLower() : value);
    }

    /// <summary>
    /// Determines if the specified string value matches any description in the enumeration of type <typeparamref name="TEnum"/>.
    /// </summary>
    /// <typeparam name="TEnum">The type of the enumeration.</typeparam>
    /// <param name="value">The string value to check.</param>
    /// <param name="ignoreCase">If set to true, performs a case-insensitive comparison. Default is false.</param>
    /// <returns>true if the string value matches any description in the enumeration; otherwise, false.</returns>
    public static bool IsInEnumDescription<TEnum>(this string value, bool ignoreCase = false) where TEnum : Enum
    {
        var enums = GetDescriptions<TEnum>().Select(e => ignoreCase ? e.ToLower() : e);
        return enums.Contains(ignoreCase ? value.ToLower() : value);
    }

    /// <summary>
    /// Gets the description associated with an enumeration value.
    /// </summary>
    /// <param name="enum">The enumeration value.</param>
    /// <param name="returnEnumNameInsteadOfNull">If true, returns the enumeration name if a description is not available.</param>
    /// <returns>The description or the enumeration name.</returns>
    private static string? GetDescription(this Enum @enum, bool returnEnumNameInsteadOfNull = false)
    {
        if (@enum == null) throw new ArgumentNullException(nameof(@enum));

        return
            @enum
                .GetType()
                .GetMember(@enum.ToString())
                .FirstOrDefault()
                ?.GetCustomAttribute<DescriptionAttribute>()
                ?.Description
            ?? (!returnEnumNameInsteadOfNull ? null : @enum.ToString());
    }
}