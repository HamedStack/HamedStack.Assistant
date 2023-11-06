// ReSharper disable UnusedMember.Global

namespace HamedStack.Assistant.Implementations;

/// <summary>
/// Provides a high-resolution date and time utility ensuring unique ticks for subsequent calls.
/// </summary>
/// <remarks>
/// The <see cref="HiResDateTime"/> class is designed to generate unique ticks for subsequent calls
/// by incrementing the previous timestamp by 1 tick if the current time matches the previous one.
/// This helps in scenarios where uniqueness in ticks is required within the same application.
/// </remarks>
public class HiResDateTime
{
    private static long _lastTimeStamp = DateTime.Now.Ticks;
    private static long _lastUtcTimeStamp = DateTime.UtcNow.Ticks;

    /// <summary>
    /// Gets the current local date and time in ticks, ensuring that it's unique and never decreases.
    /// </summary>
    /// <value>The current local date and time in ticks.</value>
    /// <remarks>
    /// This property ensures uniqueness by incrementing the previous timestamp by 1 tick if the
    /// current time matches the previous one.
    /// </remarks>
    public static long NowTicks
    {
        get
        {
            long original, newValue;
            do
            {
                original = _lastTimeStamp;
                var now = DateTime.UtcNow.Ticks;
                newValue = Math.Max(now, original + 1);
            } while (Interlocked.CompareExchange
                         (ref _lastTimeStamp, newValue, original) != original);

            return newValue;
        }
    }

    /// <summary>
    /// Gets the current UTC date and time in ticks, ensuring that it's unique and never decreases.
    /// </summary>
    /// <value>The current UTC date and time in ticks.</value>
    /// <remarks>
    /// This property ensures uniqueness by incrementing the previous UTC timestamp by 1 tick if the
    /// current UTC time matches the previous one.
    /// </remarks>
    public static long UtcNowTicks
    {
        get
        {
            long original, newValue;
            do
            {
                original = _lastUtcTimeStamp;
                var now = DateTime.UtcNow.Ticks;
                newValue = Math.Max(now, original + 1);
            } while (Interlocked.CompareExchange
                         (ref _lastUtcTimeStamp, newValue, original) != original);

            return newValue;
        }
    }
}