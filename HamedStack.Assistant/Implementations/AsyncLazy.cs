// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

namespace HamedStack.Assistant.Implementations;

/// <summary>
/// Provides support for asynchronous lazy initialization.
/// </summary>
/// <typeparam name="T">The type of the lazily initialized value.</typeparam>
public class AsyncLazy<T> : Lazy<Task<T>>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AsyncLazy{T}"/> class.
    /// </summary>
    /// <param name="valueFactory">The delegate that produces the lazily initialized value.</param>
    public AsyncLazy(Func<T> valueFactory) :
        base(() => Task.Factory.StartNew(valueFactory))
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AsyncLazy{T}"/> class.
    /// </summary>
    /// <param name="taskFactory">
    /// The asynchronous delegate that produces the lazily initialized value.
    /// </param>
    public AsyncLazy(Func<Task<T>> taskFactory) :
        base(() => Task.Factory.StartNew(taskFactory).Unwrap())
    {
    }
}