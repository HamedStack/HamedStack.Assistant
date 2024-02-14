// ReSharper disable CheckNamespace
// ReSharper disable UnusedMember.Global

using System.Runtime.CompilerServices;
using System.Threading;

namespace HamedStack.Assistant.Extensions.AsyncEnumerableExtended;

public static class AsyncEnumerableExtensions
{
    public static async Task<TAccumulate> AggregateAsync<TSource, TAccumulate>(
        this IAsyncEnumerable<TSource> source,
        TAccumulate seed,
        Func<TAccumulate, TSource, Task<TAccumulate>> func, CancellationToken cancellationToken = default)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (func == null) throw new ArgumentNullException(nameof(func));

        TAccumulate accumulator = seed;
        await foreach (var element in source.WithCancellation(cancellationToken))
        {
            accumulator = await func(accumulator, element);
        }
        return accumulator;
    }

    public static async Task<bool> AllAsync<T>(
        this IAsyncEnumerable<T> source,
        Func<T, Task<bool>> predicate, CancellationToken cancellationToken = default)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (predicate == null) throw new ArgumentNullException(nameof(predicate));

        await foreach (var element in source.WithCancellation(cancellationToken))
        {
            if (!await predicate(element))
            {
                return false; // As soon as any element does not satisfy the condition, return false
            }
        }
        return true; // If all elements satisfy the condition, return true
    }

    public static async Task<bool> AnyAsync<T>(
        this IAsyncEnumerable<T> source,
        Func<T, Task<bool>> predicate, CancellationToken cancellationToken = default)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (predicate == null) throw new ArgumentNullException(nameof(predicate));

        await foreach (var element in source.WithCancellation(cancellationToken))
        {
            if (await predicate(element))
            {
                return true; // Return true as soon as any element satisfies the condition
            }
        }
        return false; // Return false if no elements satisfy the condition
    }

    public static async IAsyncEnumerable<T> ConcatAsync<T>(
        this IAsyncEnumerable<T> first,
        IAsyncEnumerable<T> second,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        if (first == null) throw new ArgumentNullException(nameof(first));
        if (second == null) throw new ArgumentNullException(nameof(second));

        await foreach (var element in first.WithCancellation(cancellationToken))
        {
            yield return element;
        }

        await foreach (var element in second.WithCancellation(cancellationToken))
        {
            yield return element;
        }
    }

    public static async Task<int> CountAsync<T>(this IAsyncEnumerable<T> source, CancellationToken cancellationToken = default)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        var count = 0;
        await foreach (var _ in source.WithCancellation(cancellationToken))
        {
            count++;
        }
        return count;
    }

    public static async IAsyncEnumerable<T> DistinctAsync<T>(
        this IAsyncEnumerable<T> source,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        var seen = new HashSet<T>();
        await foreach (var element in source.WithCancellation(cancellationToken))
        {
            if (seen.Add(element))
            {
                yield return element;
            }
        }
    }

    public static async Task<T> FirstAsync<T>(
        this IAsyncEnumerable<T> source, CancellationToken cancellationToken = default)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        await foreach (var element in source.WithCancellation(cancellationToken))
        {
            return element; // Return the first element found
        }

        throw new InvalidOperationException("Sequence contains no elements");
    }

    public static async Task<T?> FirstOrDefaultAsync<T>(
        this IAsyncEnumerable<T> source, CancellationToken cancellationToken = default)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        await foreach (var element in source.WithCancellation(cancellationToken))
        {
            return element; // Return the first element found
        }

        return default; // Return default if sequence is empty
    }

    public static async Task ForEachAsync<T>(
        this IAsyncEnumerable<T> source,
        Func<T, Task> action, CancellationToken cancellationToken = default)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (action == null) throw new ArgumentNullException(nameof(action));

        await foreach (var element in source.WithCancellation(cancellationToken))
        {
            await action(element);
        }
    }

    public static async Task<T> LastAsync<T>(
        this IAsyncEnumerable<T> source,
        CancellationToken cancellationToken = default)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        var hasElement = false;
        T? lastElement = default;

        await foreach (var element in source.WithCancellation(cancellationToken))
        {
            hasElement = true;
            lastElement = element;
        }

        if (!hasElement)
        {
            throw new InvalidOperationException("Sequence contains no elements");
        }

        return lastElement!;
    }

    public static async Task<T?> LastOrDefaultAsync<T>(
        this IAsyncEnumerable<T> source,
        CancellationToken cancellationToken = default)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        T? lastElement = default;

        await foreach (var element in source.WithCancellation(cancellationToken))
        {
            lastElement = element;
        }

        return lastElement;
    }

    public static async Task<long> LongCountAsync<T>(this IAsyncEnumerable<T> source, CancellationToken cancellationToken = default)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        long count = 0;
        await foreach (var _ in source.WithCancellation(cancellationToken))
        {
            count++;
        }
        return count;
    }

    public static async Task<T?> MaxAsync<T>(
        this IAsyncEnumerable<T> source, CancellationToken cancellationToken = default) where T : IComparable<T>
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        var hasValue = false;
        T? max = default;
        await foreach (var element in source.WithCancellation(cancellationToken))
        {
            if (!hasValue)
            {
                max = element;
                hasValue = true;
            }
            else if (element.CompareTo(max) > 0)
            {
                max = element;
            }
        }

        if (!hasValue)
        {
            throw new InvalidOperationException("Sequence contains no elements");
        }

        return max;
    }

    public static async Task<T?> MinAsync<T>(
        this IAsyncEnumerable<T> source, CancellationToken cancellationToken = default) where T : IComparable<T>
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        var hasValue = false;
        T? min = default;
        await foreach (var element in source.WithCancellation(cancellationToken))
        {
            if (!hasValue)
            {
                min = element;
                hasValue = true;
            }
            else if (element.CompareTo(min) < 0)
            {
                min = element;
            }
        }

        if (!hasValue)
        {
            throw new InvalidOperationException("Sequence contains no elements");
        }

        return min;
    }

    public static async IAsyncEnumerable<T> OrderByAsync<T, TKey>(
        this IAsyncEnumerable<T> source,
        Func<T, TKey> keySelector,
        [EnumeratorCancellation] CancellationToken cancellationToken = default) where TKey : IComparable<TKey>
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));

        var list = await source.ToListAsync(cancellationToken);
        foreach (var element in list.OrderBy(keySelector))
        {
            cancellationToken.ThrowIfCancellationRequested();
            yield return element;
        }
    }

    public static async IAsyncEnumerable<TResult> SelectAsync<TSource, TResult>(
        this IAsyncEnumerable<TSource> source,
        Func<TSource, Task<TResult>> selector, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (selector == null) throw new ArgumentNullException(nameof(selector));

        await foreach (var element in source.WithCancellation(cancellationToken))
        {
            yield return await selector(element);
        }
    }

    public static async IAsyncEnumerable<TResult> SelectManyAsync<TSource, TResult>(
        this IAsyncEnumerable<TSource> source,
        Func<TSource, IAsyncEnumerable<TResult>> selector,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (selector == null) throw new ArgumentNullException(nameof(selector));

        await foreach (var element in source.WithCancellation(cancellationToken))
        {
            await foreach (var subElement in selector(element).WithCancellation(cancellationToken))
            {
                yield return subElement;
            }
        }
    }

    public static async Task<bool> SequenceEqualAsync<TSource>(
        this IAsyncEnumerable<TSource> first,
        IAsyncEnumerable<TSource> second,
        IEqualityComparer<TSource>? comparer = default,
        CancellationToken cancellationToken = default)
    {
        if (first == null) throw new ArgumentNullException(nameof(first));
        if (second == null) throw new ArgumentNullException(nameof(second));
        comparer ??= EqualityComparer<TSource>.Default;

        await using var e1 = first.GetAsyncEnumerator(cancellationToken);
        await using var e2 = second.GetAsyncEnumerator(cancellationToken);

        while (await e1.MoveNextAsync())
        {
            if (!await e2.MoveNextAsync() || !comparer.Equals(e1.Current, e2.Current))
            {
                return false;
            }
        }

        if (await e2.MoveNextAsync())
        {
            return false;
        }

        return true;
    }

    public static async IAsyncEnumerable<T> SkipAsync<T>(
                                this IAsyncEnumerable<T> source,
        int count,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        var skipped = 0;
        await foreach (var element in source.WithCancellation(cancellationToken))
        {
            if (skipped++ >= count)
            {
                yield return element;
            }
        }
    }


    public static async Task<decimal> SumAsync(
        this IAsyncEnumerable<decimal> source, CancellationToken cancellationToken = default)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        decimal sum = 0;
        await foreach (var number in source.WithCancellation(cancellationToken))
        {
            sum += number;
        }
        return sum;
    }

    public static async IAsyncEnumerable<T> TakeAsync<T>(
            this IAsyncEnumerable<T> source,
        int count,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (count <= 0) yield break;

        var taken = 0;
        await foreach (var element in source.WithCancellation(cancellationToken))
        {
            if (taken++ < count)
            {
                yield return element;
            }
            else
            {
                break;
            }
        }
    }


    public static async Task<IList<TSource>> ToListAsync<TSource>(this IAsyncEnumerable<TSource> source, CancellationToken cancellationToken = default)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        var list = new List<TSource>();
        await foreach (var item in source.WithCancellation(cancellationToken))
            list.Add(item);

        return list;
    }

    public static async ValueTask<IList<TSource>> ToValueTaskListAsync<TSource>(this IAsyncEnumerable<TSource> source, CancellationToken cancellationToken = default)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        var list = new List<TSource>();
        await foreach (var item in source.WithCancellation(cancellationToken))
            list.Add(item);

        return list;
    }
    public static async IAsyncEnumerable<T> WhereAsync<T>(
        this IAsyncEnumerable<T> source,
        Func<T, Task<bool>> predicate, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (predicate == null) throw new ArgumentNullException(nameof(predicate));

        await foreach (var element in source.WithCancellation(cancellationToken))
        {
            if (await predicate(element))
            {
                yield return element;
            }
        }
    }

    public static async IAsyncEnumerable<TResult> ZipAsync<TFirst, TSecond, TResult>(
        this IAsyncEnumerable<TFirst> first,
        IAsyncEnumerable<TSecond> second,
        Func<TFirst, TSecond, TResult> resultSelector,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        if (first == null) throw new ArgumentNullException(nameof(first));
        if (second == null) throw new ArgumentNullException(nameof(second));
        if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

        await using var e1 = first.GetAsyncEnumerator(cancellationToken);
        await using var e2 = second.GetAsyncEnumerator(cancellationToken);

        while (await e1.MoveNextAsync() && await e2.MoveNextAsync())
        {
            yield return resultSelector(e1.Current, e2.Current);
        }
    }
}