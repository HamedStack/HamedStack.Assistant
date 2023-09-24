// ReSharper disable CheckNamespace
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

using System.Linq.Expressions;

namespace HamedStack.Assistant.Extensions.PredicateExtended;

public static class PredicateExtensions
{
    public static Expression<Func<T, bool>> ToExpression<T>(this Predicate<T> predicate)
    {
        return x => predicate(x);
    }

    public static Func<T, bool> ToFunc<T>(this Predicate<T> predicate)
    {
        return predicate.ToExpression().Compile();
    }
}