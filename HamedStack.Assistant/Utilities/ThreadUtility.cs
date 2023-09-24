
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

namespace HamedStack.Assistant.Utilities;

/// <summary>
/// Provides utility methods for working with threads.
/// </summary>
public static class ThreadUtility
{
    /// <summary>
    /// Repeats the specified action at regular intervals, using a new thread.
    /// </summary>
    /// <param name="interval">The time interval between each repetition of the action.</param>
    /// <param name="action">The action to be executed.</param>
    /// <param name="isBackground">If set to <c>true</c>, the created thread is marked as a background thread; otherwise, it's a foreground thread. The default is <c>false</c>.</param>
    /// <param name="name">The name of the thread. It's optional and can be <c>null</c>.</param>
    /// <returns>The thread that was created to execute the action.</returns>
    public static Thread Repeat(TimeSpan interval, Action action, bool isBackground = false, string? name = null)
    {
        return new Thread(() =>
        {
            _ = new Timer(_ => action(), null, TimeSpan.Zero, interval);
        })
        {
            IsBackground = isBackground,
            Name = name
        };
    }

    /// <summary>
    /// Repeats the specified action at regular intervals until a stop condition is met, using a new thread.
    /// </summary>
    /// <param name="interval">The time interval between each repetition of the action.</param>
    /// <param name="action">The action to be executed.</param>
    /// <param name="isStopped">A function that indicates when the action should stop repeating. If it returns <c>true</c>, the repetition stops.</param>
    /// <param name="isBackground">If set to <c>true</c>, the created thread is marked as a background thread; otherwise, it's a foreground thread. The default is <c>false</c>.</param>
    /// <param name="name">The name of the thread. It's optional and can be <c>null</c>.</param>
    /// <returns>The thread that was created to execute the action.</returns>
    public static Thread Repeat(TimeSpan interval, Action action, Func<bool> isStopped, bool isBackground = false,
        string? name = null)
    {
        return new Thread(() =>
        {
            var timer = new Timer(_ => { action(); }, null, TimeSpan.Zero, interval);
            if (isStopped()) timer.Dispose();
        })
        {
            IsBackground = isBackground,
            Name = name
        };
    }
}