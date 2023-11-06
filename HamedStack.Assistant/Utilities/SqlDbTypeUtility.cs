// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable StringLiteralTypo

namespace HamedStack.Assistant.Utilities;

/// <summary>
/// Provides utility methods to map SQL data types to their corresponding .NET types.
/// </summary>
public static class SqlDbTypeUtility
{
    private static Dictionary<string, Type>? _mappings;

    /// <summary>
    /// Converts a SQL data type to its corresponding .NET type name as a string.
    /// </summary>
    /// <param name="sqlDataType">The SQL data type as a string (e.g., "varchar").</param>
    /// <returns>The name of the corresponding .NET type (e.g., "String").</returns>
    /// <exception cref="KeyNotFoundException">
    /// Thrown when the provided SQL data type doesn't have a corresponding .NET type.
    /// </exception>
    public static string FromString(string sqlDataType)
    {
        sqlDataType = sqlDataType.ToLower().Trim();
        _mappings = new Dictionary<string, Type>
        {
            {"bigint", typeof(long)},
            {"binary", typeof(byte[])},
            {"bit", typeof(bool)},
            {"char", typeof(string)},
            {"date", typeof(DateTime)},
            {"datetime", typeof(DateTime)},
            {"datetime2", typeof(DateTime)},
            {"datetimeoffset", typeof(DateTimeOffset)},
            {"decimal", typeof(decimal)},
            {"float", typeof(double)},
            {"image", typeof(byte[])},
            {"int", typeof(int)},
            {"money", typeof(decimal)},
            {"nchar", typeof(string)},
            {"ntext", typeof(string)},
            {"numeric", typeof(decimal)},
            {"nvarchar", typeof(string)},
            {"real", typeof(float)},
            {"rowversion", typeof(byte[])},
            {"smalldatetime", typeof(DateTime)},
            {"smallint", typeof(short)},
            {"smallmoney", typeof(decimal)},
            {"text", typeof(string)},
            {"time", typeof(TimeSpan)},
            {"timestamp", typeof(byte[])},
            {"tinyint", typeof(byte)},
            {"uniqueidentifier", typeof(Guid)},
            {"varbinary", typeof(byte[])},
            {"varchar", typeof(string)}
        };
        return _mappings[sqlDataType].Name;
    }

    /// <summary>
    /// Converts a SQL data type to its corresponding .NET type.
    /// </summary>
    /// <param name="sqlDataType">The SQL data type as a string (e.g., "varchar").</param>
    /// <returns>The corresponding .NET type (e.g., typeof(string)).</returns>
    /// <exception cref="KeyNotFoundException">
    /// Thrown when the provided SQL data type doesn't have a corresponding .NET type.
    /// </exception>
    public static Type ToType(string sqlDataType)
    {
        sqlDataType = sqlDataType.ToLower().Trim();
        _mappings = new Dictionary<string, Type>
        {
            {"bigint", typeof(long)},
            {"binary", typeof(byte[])},
            {"bit", typeof(bool)},
            {"char", typeof(string)},
            {"date", typeof(DateTime)},
            {"datetime", typeof(DateTime)},
            {"datetime2", typeof(DateTime)},
            {"datetimeoffset", typeof(DateTimeOffset)},
            {"decimal", typeof(decimal)},
            {"float", typeof(double)},
            {"image", typeof(byte[])},
            {"int", typeof(int)},
            {"money", typeof(decimal)},
            {"nchar", typeof(string)},
            {"ntext", typeof(string)},
            {"numeric", typeof(decimal)},
            {"nvarchar", typeof(string)},
            {"real", typeof(float)},
            {"rowversion", typeof(byte[])},
            {"smalldatetime", typeof(DateTime)},
            {"smallint", typeof(short)},
            {"smallmoney", typeof(decimal)},
            {"text", typeof(string)},
            {"time", typeof(TimeSpan)},
            {"timestamp", typeof(byte[])},
            {"tinyint", typeof(byte)},
            {"uniqueidentifier", typeof(Guid)},
            {"varbinary", typeof(byte[])},
            {"varchar", typeof(string)}
        };
        return _mappings[sqlDataType];
    }
}