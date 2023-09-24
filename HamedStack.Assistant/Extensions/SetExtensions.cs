// ReSharper disable CheckNamespace
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

namespace HamedStack.Assistant.Extensions.SetExtended;

public static class SetExtensions
{
    public static int AddRange<T>(this ISet<T> source, IEnumerable<T> items)
    {
        return items.Count(source.Add);
    }
}