// ReSharper disable CheckNamespace
// ReSharper disable UnusedMember.Global

namespace HamedStack.Assistant.Extensions.ParsableExtended;

#if NET7_0_OR_GREATER
public static class ParsableExtensions
{
    public static T Parse<T>(this string input, IFormatProvider? formatProvider = null)
        where T : IParsable<T>
    {
        return T.Parse(input, formatProvider);
    }
}
#endif

