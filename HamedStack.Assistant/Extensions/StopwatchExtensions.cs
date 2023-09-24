// ReSharper disable CheckNamespace
// ReSharper disable UnusedMember.Global

using System.Diagnostics;

namespace HamedStack.Assistant.Extensions.StopwatchExtended;

public static class StopwatchExtensions
{
    public static long ElapsedSeconds(this Stopwatch sw)
    {
        return sw.ElapsedMilliseconds / 1000;
    }

    public static TimeSpan GetElapsedAndRestart(this Stopwatch stopwatch)
    {
        stopwatch.Stop();
        var result = stopwatch.Elapsed;
        stopwatch.Restart();
        return result;
    }
}