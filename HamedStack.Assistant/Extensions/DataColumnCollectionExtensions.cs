// ReSharper disable CheckNamespace
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

using System.Data;

namespace HamedStack.Assistant.Extensions.DataColumnCollectionExtended;

public static class DataColumnCollectionExtensions
{
    public static void AddRange(this DataColumnCollection @this, IEnumerable<string> columnNames)
    {
        foreach (var columnName in columnNames) @this.Add(columnName);
    }

    public static void AddRange(this DataColumnCollection @this, params string[] columnNames)
    {
        foreach (var columnName in columnNames) @this.Add(columnName);
    }
}